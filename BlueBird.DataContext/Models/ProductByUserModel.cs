using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.DataContext.Models
{
    public class ProductByUserModel
    {
        public string BrandName { get; set; }
        public string Avata { get; set; }
        public Guid Id { get; set; }
        public string NameProduct { get; set; }
        public string Img { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; }
    }
}
