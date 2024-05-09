using BlueBird.DataConext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.DataConext.Data
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? IdProducer { get; set; } = Guid.Empty;
        public Guid? IdProductType { get; set; } = Guid.Empty;
        public string Material { get; set; }
        public Double Price { get; set; }
        public string Description { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set;}
        public List<IMG> Images { get; set; }
        public List<Product_Detail> Products { get; set; }
        public int UsedStatus { get; set; }
        public Guid? IdDiscount { get; set; } = Guid.Empty;
        public Guid? IdShop { get; set;} = Guid.Empty;
        public Product() 
        {
            IdProducer = Guid.Empty;
            IdDiscount = Guid.Empty;
        }
    }
    public class ProductDetail : Product
    {
        public string NameDiscount { get; set; }
        public string BrandName { get; set; }
        public string avataShop { get; set; }
        public int TotalProduct { get; set; }
    }
}
