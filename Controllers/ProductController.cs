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
                    Id = x.ID,
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


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.ID == id);
            if (product != null)
            {
                List<ProductImage> images = _context
                    .ProductImages.Where(x => x.ProductId == product.ID).ToList();

                foreach (var image in images)
                {
                    string imageName = Path.Combine(Directory.GetCurrentDirectory(), "products", image.Name);
                    if (System.IO.File.Exists(imageName))
                    {
                        System.IO.File.Delete(imageName);
                    }

                    _context.ProductImages.Remove(image);
                }
                _context.SaveChanges();

                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (Id != 0)
            {
                var product = _context.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.ID == Id);
                ProductItemViewModel model = new ProductItemViewModel
                {
                    Id = product.ID,
                    Name = product.Name,
                    Price = product.Price,
                    PathImage = product.ProductImages.Select(x => new ProductImageItemVM
                    {
                        Path = x.Name
                    }).ToList()
                };
                return View(model);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Edit(ProductEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Дані введено не коректно!");
                return View(model);

            }

            if (model.Id == 0)
            {
                ModelState.AddModelError("", "Оберіть продукт видалення!");
                return View(model);
            }

            var product = _context.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.ID == model.Id);
            if (product != null)
            {
                string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "products");
                product.Name = model.Name;
                product.Price = model.Price;
                //видаляємо сторі фотки
                if (model.deletedImages != null)
                {
                    foreach (var delProduct in model.deletedImages)
                    {
                        var delProductImage = product.ProductImages.SingleOrDefault(x => delProduct.Contains(x.Name));
                        string imgPath = Path.Combine(dirPath, delProductImage.Name);
                        if (System.IO.File.Exists(imgPath))
                        {
                            System.IO.File.Delete(imgPath);
                        }
                        _context.ProductImages.Remove(delProductImage);
                    }
                }
                //Додати нові фотки
                if (model.Images != null)
                {
                    foreach (var newImages in model.Images)
                    {
                        string ext = Path.GetExtension(newImages.FileName);
                        string fileName = Path.GetRandomFileName() + ext;

                        string filePath = Path.Combine(dirPath, fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            newImages.CopyTo(stream);
                        }

                        _context.ProductImages.Add(new Data.Entities.ProductImage
                        {
                            Name = fileName,
                            ProductId = product.ID
                        });
                    }
                }

                _context.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("", "Помилка коду! Зверніться до сервісного центру!");
                return View(model);
            }

            return RedirectToAction("Index");
        }
    }
}
