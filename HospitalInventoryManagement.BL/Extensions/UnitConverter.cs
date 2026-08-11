using System;
using HospitalInventoryManagement.Data.Models;

namespace HospitalInventoryManagement.BL.Extensions
{
    public enum StockUnit
    {
        Kutu,
        Flakon,
        Test
    }

    /// <summary>
    /// Tüm stok hesapları "Test" temel birimi üzerinden yürür.
    /// 1 Kutu = Product.BoxTextCount Test = Product.FlaconCountPerBox Flakon.
    /// </summary>
    public static class UnitConverter
    {
        public static StockUnit ParseUnit(string unit)
        {
            switch ((unit ?? string.Empty).Trim().ToLowerInvariant())
            {
                case "kutu":
                case "box":
                    return StockUnit.Kutu;
                case "flakon":
                case "flacon":
                    return StockUnit.Flakon;
                default:
                    return StockUnit.Test;
            }
        }

        /// <summary>Girilen miktarı teste çevirir (base birim).</summary>
        public static int ToTests(int quantity, string unit, Product product)
            => ToTests(quantity, ParseUnit(unit), product);

        public static int ToTests(int quantity, StockUnit unit, Product product)
        {
            if (product == null) return quantity;

            switch (unit)
            {
                case StockUnit.Kutu:
                    return quantity * product.BoxTextCount;

                case StockUnit.Flakon:
                    // 1 flakon = BoxTextCount / FlaconCountPerBox test
                    if (product.FlaconCountPerBox <= 0 || product.BoxTextCount <= 0) return 0;
                    return (int)Math.Round((double)quantity * product.BoxTextCount / product.FlaconCountPerBox);

                case StockUnit.Test:
                default:
                    return quantity;
            }
        }

        /// <summary>Test sayısından tam kutu (aşağı yuvarlanır).</summary>
        public static int ToBoxes(int testCount, Product product)
            => (product == null || product.BoxTextCount <= 0) ? 0 : testCount / product.BoxTextCount;

        /// <summary>Tam kutuya girmeyen artan test.</summary>
        public static int RemainderTests(int testCount, Product product)
            => (product == null || product.BoxTextCount <= 0) ? testCount : testCount % product.BoxTextCount;

        /// <summary>Test sayısının flakon karşılığı (yuvarlanır).</summary>
        public static int ToFlacons(int testCount, Product product)
        {
            if (product == null || product.BoxTextCount <= 0) return 0;
            return (int)Math.Round((double)testCount * product.FlaconCountPerBox / product.BoxTextCount);
        }

        /// <summary>"3 kutu + 12 test" gibi okunur özet.</summary>
        public static string Describe(int testCount, Product product)
        {
            if (product == null || product.BoxTextCount <= 0)
                return $"{testCount} test";

            int boxes = ToBoxes(testCount, product);
            int rem = RemainderTests(testCount, product);
            return rem > 0 ? $"{boxes} kutu + {rem} test ({testCount} test)"
                           : $"{boxes} kutu ({testCount} test)";
        }
    }
}