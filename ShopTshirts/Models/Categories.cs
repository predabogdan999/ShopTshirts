using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopTshirts.Models
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }
        public string categoryName { get; set; }

        public ICollection<Products> Products { get; set; }
    }
}
