using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopTshirts.Models
{
    public class ProductModel
    {
        public string productName { get; set; }
        public int categoryId { get; set; }
        public Categories Categories { get; set; }
        public string description { get; set; }
        public string productImg { get; set; }
        public int price { get; set; }
        public int rating { get; set; }
        public string color { get; set; }
        public int warranty { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime  startDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime endDate { get; set; }

        public Products ToEntity(Products product)
        {
            product.productName = productName;
            product.rating = rating;
            product.price = price;
            product.warranty = warranty;
            product.color = color;
            product.description = description;
            product.productImg = productImg;
            product.startDate = DateTime.Now;
            product.endDate = endDate;
            return product;
        }

       



    }
}
