using HospitalInventoryManagement.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalInventoryManagement.Web.Controllers
{
    [ApiController]
    [Route("api/stock")]
    [AllowAnonymous]
    public class StockApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public StockApiController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> List(
            [FromQuery] int? hospitalId,
            [FromQuery] int? sayimNo,
            [FromQuery] string? apiKey,
            [FromHeader(Name = "X-Api-Key")] string? headerKey,
            CancellationToken ct)
        {
            // ─── 1) API KEY DOĞRULAMA ───
            var expected = _config["StockApi:ApiKey"];
            var provided = !string.IsNullOrWhiteSpace(headerKey) ? headerKey : apiKey;

            if (string.IsNullOrWhiteSpace(expected))
                return StatusCode(500, new { success = false, message = "Server misconfiguration (missing api key)." });

            if (string.IsNullOrWhiteSpace(provided) || provided != expected)
                return Unauthorized(new { success = false, message = "Invalid or missing API key." });

            // ─── 2) QUERY ───
            var q = _context.Stocks
                .AsNoTracking()
                .Include(s => s.Product).ThenInclude(p => p.ProductType)
                .Include(s => s.Product).ThenInclude(p => p.Platform)
                .Include(s => s.Hospital)
                .Include(s => s.Sayim)
                .AsQueryable();

            if (hospitalId.HasValue && hospitalId.Value > 0)
                q = q.Where(s => s.HospitalID == hospitalId.Value);

            // Sayım filtresi: belirtilen sayım, yoksa aktif
            if (sayimNo.HasValue && sayimNo.Value > 0)
                q = q.Where(s => s.Sayim != null && s.Sayim.SayimNo == sayimNo.Value);
            else
                q = q.Where(s => s.Sayim != null && s.Sayim.IsActive);

            // ─── 3) PROJECTION ───
            var items = await q
                .OrderBy(s => s.Hospital.HospitalName)
                .ThenBy(s => s.Product.ProductName)
                .Select(s => new StockApiItemDto
                {
                    StockId = s.StockID,
                    HospitalId = s.HospitalID,
                    HospitalName = s.Hospital.HospitalName,
                    ProductId = s.ProductID,
                    ProductName = s.Product.ProductName,
                    ReferenceNumber = s.Product.ReferenceNumber ?? "",
                    ProductType = s.Product.ProductType != null ? s.Product.ProductType.TypeName : null,
                    Platform = s.Product.Platform != null ? s.Product.Platform.PlatformName : null,
                    BoxCount = s.Quantity,
                    FlaconCount = s.FlaconCount,
                    TestCount = s.TestCount,
                    BoxTextCount = s.Product.BoxTextCount,
                    FlaconCountPerBox = s.Product.FlaconCountPerBox,
                    LotNumber = s.LotNumber,
                    ExpiryDate = s.ExpiryDate,
                    LastUpdated = s.LastUpdated,
                    SayimNo = s.Sayim != null ? s.Sayim.SayimNo : 0,
                    SayimAktifMi = s.Sayim != null && s.Sayim.IsActive,
                    SayimTarihi = s.Sayim != null ? s.Sayim.CreatedDate : DateTime.MinValue
                })
                .ToListAsync(ct);

            return Ok(new StockApiResponseDto
            {
                Success = true,
                Count = items.Count,
                GeneratedAt = DateTime.Now,
                Items = items
            });
        }

        [HttpGet("ping")]
        public IActionResult Ping(
            [FromQuery] string? apiKey,
            [FromHeader(Name = "X-Api-Key")] string? headerKey)
        {
            var expected = _config["StockApi:ApiKey"];
            var provided = !string.IsNullOrWhiteSpace(headerKey) ? headerKey : apiKey;
            if (string.IsNullOrWhiteSpace(expected) || string.IsNullOrWhiteSpace(provided) || provided != expected)
                return Unauthorized(new { success = false });
            return Ok(new { success = true, serverTime = DateTime.Now, version = "1.0" });
        }
    }

    // ─────────────────────────────────────────────────────────────────────
    //  DTO'lar (controller ile aynı dosyada — pratik)
    //  CRMOzem tarafında aynı şekilde tanımlanacak (StockApiDtos.cs)
    // ─────────────────────────────────────────────────────────────────────
    public class StockApiResponseDto
    {
        public bool Success { get; set; }
        public int Count { get; set; }
        public DateTime GeneratedAt { get; set; }
        public List<StockApiItemDto> Items { get; set; } = new();
    }

    public class StockApiItemDto
    {
        public int StockId { get; set; }
        public int HospitalId { get; set; }
        public string HospitalName { get; set; } = "";

        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public string ReferenceNumber { get; set; } = "";
        public string? ProductType { get; set; }
        public string? Platform { get; set; }

        public int BoxCount { get; set; }
        public int FlaconCount { get; set; }
        public int TestCount { get; set; }
        public int BoxTextCount { get; set; }
        public int FlaconCountPerBox { get; set; }

        public string? LotNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime LastUpdated { get; set; }

        public int SayimNo { get; set; }
        public bool SayimAktifMi { get; set; }
        public DateTime SayimTarihi { get; set; }
    }
}
