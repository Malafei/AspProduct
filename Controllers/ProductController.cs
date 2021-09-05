using AppProductImages.Data.Entities;
using AppProductImages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AppProductImages.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppEFContext _context;

        public ProductController(AppEFContext context)
        {
            _context = context;
        }

        public IActionResult Index ()
        {
            var model = _context.Products
                .Include(i => i.ProductImages)
                .Select(x => new ProductItemViewModel
                {
                    Name = x.Name,
                    Price = x.Price,
                    PathImage = x.ProductImages.Select(t => new ProductImageItemVM
                    {
                        Path = "/images/" + t.Name
                    }).ToList()

                });
                
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductAddViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            List<string> fileNames = new List<string>();
            foreach (var item in model.PathImage)
            {
                string fileName = "";
                if (item != null)
                {
                    var ext = Path.GetExtension(item.FileName);
                    fileName = Path.GetRandomFileName() + ext;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "products", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        item.CopyTo(stream);
                    }
                    fileNames.Add(fileName);
                }
            }
            Product product = new Product()
            {
                Name = model.Name,
                Price = model.Price
            };

            _context.Products.Add(product);
            _context.SaveChanges();
            int counter = 1;

            foreach (var img in fileNames)
            {
                ProductImage productImage = new ProductImage()
                {
                    Name = img,
                    Priority = counter++,
                    ProductId = product.ID
                };
                _context.ProductImages.Add(productImage);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
