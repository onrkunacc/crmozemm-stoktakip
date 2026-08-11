using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HospitalInventoryManagement.Web.ViewModel
{
    public class ProductEditViewModel
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Referans numarası zorunludur.")]
        public string ReferenceNumber { get; set; }

        // Edit formunda yok; zorunlu olmamalı (nullable => implicit required kalkar)
        public string? LotNumber { get; set; }

        public int BoxTextCount { get; set; }
        public int FlaconCountPerBox { get; set; }
        public int CategoryID { get; set; }
        public int TypeID { get; set; }
        public int PlatformID { get; set; }

        // SelectList değerleri için (bind edilmez, doğrulanmaz)
        [BindNever, ValidateNever]
        public SelectList Categories { get; set; }
        [BindNever, ValidateNever]
        public SelectList ProductTypes { get; set; }
        [BindNever, ValidateNever]
        public SelectList Platforms { get; set; }
    }
}