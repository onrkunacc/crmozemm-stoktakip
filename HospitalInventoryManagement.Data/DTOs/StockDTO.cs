namespace HospitalInventoryManagement.Data.DTOs
{
    public class StockDTO
    {
        public int StockID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int TestCount { get; set; }     // test bazlı gerçek stok
        public int BoxTextCount { get; set; }  // 1 kutu = kaç test (kırılım için)
        public DateTime ExpiryDate { get; set; }
        public string ProductName { get; set; }  // Sadece ProductName'i serileştirin
        public string HospitalName { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}