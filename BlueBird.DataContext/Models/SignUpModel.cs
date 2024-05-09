using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.DataConext.Models
{
    public class SignUpModel
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set;}
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
    }
    public class SignUpShopModel : SignUpModel
    {
        [Required]
        public string BrandName { get; set; }
        public string Avata { get; set; }
    }
}
