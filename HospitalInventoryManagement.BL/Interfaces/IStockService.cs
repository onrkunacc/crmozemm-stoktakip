using HospitalInventoryManagement.Data.DTOs;
using HospitalInventoryManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalInventoryManagement.BL.Interfaces
{
    public interface IStockService
    {
        List<Stock> GetAllStocks();
        List<Stock> GetStocksByHospital(int hospitalId);
        Stock GetStockById(int id);
        void UpdateStock(Stock stock);
        void DeleteStock(int stockId);
        void AddStock(Stock stock);
        void AddStockTransaction(StockTransaction transaction);
        void UpdateStockQuantity(int stockId, int changeQuantity);
        Stock GetStockByReferenceNumber(string referenceNumber, int hospitalId);
        bool ProcessStockTransaction(string referenceNumber, int quantityChange, int hospitalId, string lotNumber = null, DateTime? expiryDate = null);
        void AddNewStock(string referenceNumber, string productName, int quantity, int hospitalId, string lotNumber, DateTime expiryDate);

        // Sayım: birden çok satırı (kutu/test) teste çevirip ilgili hastane stoğuna ekler,
        // her satır için kullanıcı bilgisiyle işlem kaydı yazar.
        StockCountResultDTO ApplyCount(List<StockCountLineDTO> lines, int hospitalId, string userId);

        // Hastaneler arası aktarım: kaynaktan düşer, hedefe ekler, iki taraflı açıklamalı işlem kaydı yazar.
        TransferResultDTO TransferBetweenHospitals(string referenceNumber, int quantity, string unit,
            int sourceHospitalId, int destinationHospitalId, string note, string userId);

        // Bir hastanedeki bir ürünün mevcut test miktarı (aktarım ekranında gösterim için).
        int GetTestCount(string referenceNumber, int hospitalId);
        Sayim GetActiveSayim(int hospitalId);
        Sayim StartNewSayim(int hospitalId, string userId);
        Sayim GetOrCreateActiveSayim(int hospitalId, string userId);

    }
}