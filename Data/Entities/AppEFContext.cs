using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppProductImages.Data.Entities
{
    public class AppEFContext : DbContext
    {
        public AppEFContext (DbContextOptions<AppEFContext> options):
            base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

    }
}
