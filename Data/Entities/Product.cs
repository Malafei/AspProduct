using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppProductImages.Data.Entities
{
    [Table("tblProduct")]
    public class Product
    {
        [Key]
        public int ID { get; set; }
        [Required, StringLength(255)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}
