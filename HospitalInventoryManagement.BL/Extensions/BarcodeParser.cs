using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HospitalInventoryManagement.BL.Extensions
{
    public static class BarcodeParser
    {
        // Barkod verisini ayrıştıran metod. Hiçbir zaman exception fırlatmaz;
        // ayrıştırılamayan alanlar null döner. Kritik alan yalnızca ReferenceNumber'dır.
        public static BarcodeData Parse(string barcode)
        {
            var data = new BarcodeData();

            if (string.IsNullOrWhiteSpace(barcode))
                return data;

            try { data.ExpiryDate = ParseExpiryDate(barcode); } catch { data.ExpiryDate = null; }
            try { data.LotNumber = ParseLotNumber(barcode); } catch { data.LotNumber = null; }
            try { data.ReferenceNumber = ParseReferenceNumber(barcode); } catch { data.ReferenceNumber = null; }

            return data;
        }

        // Yardımcı: barkodun i konumunda pat dizisi var mı?
        private static bool HasAt(string s, int i, string pat)
            => i >= 0 && i + pat.Length <= s.Length && s.Substring(i, pat.Length) == pat;

        // 240 kodunu kullanarak referans numarasını ayrıştırır.
        // GTIN (01) ve tarih (17/11/15/13) alanlarını baştan atlar; böylece bu alanların
        // içinde geçen "240"/"17" dizileri yanlış eşleşmez.
        private static string ParseReferenceNumber(string barcode)
        {
            if (string.IsNullOrEmpty(barcode))
                return null;

            int cursor = 0;

            // 01 = GTIN (14 hane sabit)
            if (HasAt(barcode, cursor, "01") && barcode.Length >= cursor + 16)
                cursor += 16;

            // 17/11/15/13 = tarih (6 hane sabit), arka arkaya olabilir
            while (barcode.Length >= cursor + 8 &&
                   (HasAt(barcode, cursor, "17") || HasAt(barcode, cursor, "11") ||
                    HasAt(barcode, cursor, "15") || HasAt(barcode, cursor, "13")))
            {
                cursor += 8;
            }

            // 240'ı cursor'dan itibaren ara. (01 ile başlamayan barkodlarda cursor=0,
            // yani eski davranış korunur.)
            int index = barcode.IndexOf("240", cursor);
            if (index < 0 || barcode.Length <= index + 3)
                return null;

            // "240" kodundan sonra gelen kısmı al
            string rawRefNumber = barcode.Substring(index + 3);

            // Baştaki boşluk, "0" ve "*" karakterlerini temizle
            rawRefNumber = Regex.Replace(rawRefNumber, @"^[\s0*]+", "");

            // İlk 6 alfanumerik karakteri topla
            var refNumberBuilder = new StringBuilder();
            foreach (char ch in rawRefNumber)
            {
                if (char.IsLetterOrDigit(ch))
                    refNumberBuilder.Append(ch);

                if (refNumberBuilder.Length == 6)
                    break;
            }

            return refNumberBuilder.Length > 0 ? refNumberBuilder.ToString() : null;
        }

        // 17 kodunu kullanarak son kullanma tarihini ayrıştırır
        private static DateTime? ParseExpiryDate(string barcode)
        {
            int index = barcode.IndexOf("17");
            if (index >= 0 && barcode.Length > index + 8)
            {
                // "17" kodundan sonra gelen kısmı al
                string rawExpiryDate = barcode.Substring(index + 2);

                // Başlangıçtaki boşluk ve "0" karakterlerini temizleyin
                rawExpiryDate = Regex.Replace(rawExpiryDate, @"^[\s0]+", "");

                // İlk 6 karakteri alıp tarihi parse etme
                string dateStr = new string(rawExpiryDate.Take(6).ToArray());
                if (DateTime.TryParseExact(dateStr, "yyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime expiryDate))
                {
                    return expiryDate;
                }

                Console.WriteLine("Geçersiz son kullanma tarihi formatı.");
            }
            return null;
        }

        // 10 kodunu kullanarak lot numarasını ayrıştırır. Bulunamazsa null döner (fırlatmaz).
        private static string ParseLotNumber(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
                return null;

            // İlk olarak "17" kodunun sonunu bul
            int expiryDateEndIndex = barcode.IndexOf("17");
            if (expiryDateEndIndex >= 0 && barcode.Length > expiryDateEndIndex + 8)
            {
                // Tarih formatının bitiş noktası
                expiryDateEndIndex += 8; // "17" + "yyMMdd" = 8 karakter
            }
            else
            {
                expiryDateEndIndex = 0; // Eğer "17" yoksa baştan başla
            }

            // "10" kodunu, "17" kodunun sonundan itibaren aramaya başla
            int lotStartIndex = barcode.IndexOf("10", expiryDateEndIndex);
            if (lotStartIndex < 0)
                return null; // Lot alanı yoksa sorun değil; operatör elle girebilir.

            // "240" kodunun pozisyonunu bul
            int lotEndIndex = barcode.IndexOf("240", lotStartIndex);
            if (lotEndIndex < 0)
            {
                lotEndIndex = barcode.Length; // Eğer "240" kodu yoksa barkodun sonuna kadar al
            }

            if (lotEndIndex <= lotStartIndex + 2)
                return null;

            // "10" kodundan sonra gelen kısmı "240" koduna kadar kes
            string rawLotNumber = barcode.Substring(lotStartIndex + 2, lotEndIndex - (lotStartIndex + 2));

            // Alfanümerik karakterleri filtrele
            string lotNumber = new string(rawLotNumber.Where(char.IsLetterOrDigit).ToArray());

            // Geçerli bir lot numarası en az 3 karakter olmalı; değilse null.
            if (string.IsNullOrEmpty(lotNumber) || lotNumber.Length < 3)
                return null;

            return lotNumber;
        }
    }
    public class BarcodeData
    {
        public string ReferenceNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string LotNumber { get; set; }
        public int Quantity { get; set; }
    }
}