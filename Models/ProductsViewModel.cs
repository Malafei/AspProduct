using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppProductImages.Models
{
    public class ProductItemViewModel
    {
        public int Id { get; set; }
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

    public class ProductEditViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Назва"), Required(ErrorMessage = "Поле 'Назва' не може бути пустим!")]
        public string Name { get; set; }
        [Display(Name = "Ціна"), Required(ErrorMessage = "Поле 'Ціна' не може бути пустим!")]
        public decimal Price { get; set; }
        [Display(Name = "Фотографії")]
        public List<IFormFile> Images { get; set; }
        public List<string> deletedImages { get; set; }
    }
}
