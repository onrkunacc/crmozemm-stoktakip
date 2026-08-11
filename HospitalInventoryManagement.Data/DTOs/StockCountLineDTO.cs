using System;

namespace HospitalInventoryManagement.Data.DTOs
{
    /// <summary>
    /// Sayım ekranından gelen tek satır. Birim "Kutu" veya "Test" olabilir;
    /// servis tarafında teste normalize edilip toplanır.
    /// </summary>
    public class StockCountLineDTO
    {
        public string ReferenceNumber { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }            // "Kutu" | "Test"
        public string? LotNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    /// <summary>Sayım sonucu özeti (kullanıcıya geri bildirim için).</summary>
    public class StockCountResultDTO
    {
        public int LineCount { get; set; }
        public int ProductCount { get; set; }
        public int TotalTests { get; set; }
        public System.Collections.Generic.List<string> Warnings { get; set; } = new();
    }

    /// <summary>Hastaneler arası aktarım sonucu.</summary>
    public class TransferResultDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TransferredTests { get; set; }
    }
}