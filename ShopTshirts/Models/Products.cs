using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopTshirts.Models
{
    public class Products
    {
        [Key]
        public int productId { get; set; }
        public string productName { get; set; }
        public Categories Categories { get; set; }
        public string description { get; set; }
        public byte[] productImg { get; set; }
        public int price { get; set; }
        public int rating { get; set; }
        public string color { get; set; }
        public int warranty { get; set; }
        public int categoryId { get; set; }

        

    }
}
