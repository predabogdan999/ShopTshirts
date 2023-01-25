using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
      //  [Column(TypeName = "NVARCHAR(256)")]
        public string productImg { get; set; }
        public int price { get; set; }
        public int rating { get; set; }
        public string color { get; set; }
        public int warranty { get; set; }
        public int categoryId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime startDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime endDate { get; set; }



    }
}
