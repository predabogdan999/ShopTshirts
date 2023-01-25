using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ShopTshirts.Models
{
    public class RegisterModel :IdentityUser
    {
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
