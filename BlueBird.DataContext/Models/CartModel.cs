using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.DataContext.Models
{
    public  class CartModel
    {
        public Guid IdProduct { get; set; }
        public int Number { get; set; } 
    }
    public class CartDispay
    {
        public Guid Id { get; set; }
        public string BrandName { get; set; }
        public string NameProduct { get; set; }
        public int Number { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public double TotalPrice { get; set; }
        public string Img { get; set; }
    }
}
