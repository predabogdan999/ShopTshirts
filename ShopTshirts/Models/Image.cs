using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopTshirts.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public int productId { get; set; }
        public Products Product { get; set; }
        public string Path { get; set; }

        

    }
}
