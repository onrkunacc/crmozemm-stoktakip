using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalInventoryManagement.Data.Models
{
    public class StockTransaction
    {
        [Key]
        public int TransactionID { get; set; }
        public int StockID { get; set; }
        public int ChangeQuantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public string LotNumber { get; set; }

        // Kim girdi takibi (AspNetUsers.Id)
        public string? UserId { get; set; }
        // İşlemin geçtiği hastane (aktarımda iki taraf, raporlamada filtre için)
        public int? HospitalID { get; set; }
        // Operatörün girdiği birim: "Kutu" | "Test" | "Flakon"
        public string? Unit { get; set; }
        // ChangeQuantity'nin teste normalize edilmiş hâli (işareti ChangeQuantity ile aynı)
        public int QuantityInTests { get; set; }
        // Aktarımda karşı işlemi (çıkış<->giriş) eşlemek için
        public int? RelatedTransactionID { get; set; }

        public Stock Stock { get; set; }
    }
}