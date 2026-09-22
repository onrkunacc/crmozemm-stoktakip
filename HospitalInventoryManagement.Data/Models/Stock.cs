using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalInventoryManagement.Data.Models
{
    public class Stock
    {
        [Key]
        public int StockID { get; set; }
        public int HospitalID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public string? LotNumber { get; set; } // Yeni LotNumber alanı
        public int FlaconCount { get; set; }

        // Temel (base) birim: teste normalize edilmiş gerçek stok. Tek doğruluk kaynağı budur.
        // Quantity (kutu) ve FlaconCount bu değerden türetilip senkron tutulur.
        public int TestCount { get; set; }

        // Ait olduğu sayım dönemi (aktif sayım = güncel stok)
        public int? SayimID { get; set; }
        public Sayim Sayim { get; set; }

        public Hospital Hospital { get; set; }
        public Product Product { get; set; }
        public ICollection<StockTransaction> StockTransactions { get; set; }
    }
}