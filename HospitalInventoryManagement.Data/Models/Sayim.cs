using System;
using System.Collections.Generic;

namespace HospitalInventoryManagement.Data.Models
{

    public class Sayim
    {
        public int SayimID { get; set; }

        public int HospitalID { get; set; }

        // Hastane bazında sıralı numara: 1, 2, 3...
        public int SayimNo { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Yeni sayım başlatıldığında eski sayımın kapanış tarihi
        public DateTime? ClosedDate { get; set; }

        public string? CreatedByUserId { get; set; }

        // Hastanenin güncel (açık) sayımı mı?
        public bool IsActive { get; set; }

        public string? Note { get; set; }

        public Hospital Hospital { get; set; }
        public ICollection<Stock> Stocks { get; set; }
    }
}