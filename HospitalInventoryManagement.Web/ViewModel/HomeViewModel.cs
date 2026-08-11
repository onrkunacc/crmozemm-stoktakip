using HospitalInventoryManagement.Data.Models;

namespace HospitalInventoryManagement.Web.ViewModel
{
    public class HomeViewModel
    {
        public string HospitalName { get; set; }
        public List<Stock> LowStockItems { get; set; }
        public List<MonthlyStatisticsViewModel> StatisticsData { get; set; }
        public List<StockChartData> StockData { get; set; }

        // Özet kartlar (test bazlı)
        public int TotalProducts { get; set; }
        public int TotalTests { get; set; }
        public int LowStockCount { get; set; }
        public int ExpiringSoonCount { get; set; }
    }

    public class StockChartData
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }   // türetilmiş kutu
        public int TestCount { get; set; }  // asıl (test bazlı)
    }
}