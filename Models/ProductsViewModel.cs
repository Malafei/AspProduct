using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppProductImages.Models
{
    public class ProductItemViewModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<ProductImageItemVM> PathImage { get; set; }
    }

    public class ProductImageItemVM
    {
        public string Path { get; set; }
    }


    public class ProductAddViewModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<IFormFile> PathImage { get; set; }
    }
}
