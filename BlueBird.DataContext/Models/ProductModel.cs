using BlueBird.DataContext.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.DataConext.Models
{
    public class ProductModel
    {
        public string Name { get; set; }
        public Guid? IdProducer { get; set; } = Guid.Empty;
        public Guid? IdProductType { get; set; } = Guid.Empty;
        public string Material { get; set; }
        public Double Price { get; set; }
        public string Description { get; set; }
        public Guid? IdDiscount { get; set; } = Guid.Empty;
        public List<IMG> Images { get; set; }
        public List<Product_DetailModel> Product_Detail { get; set; }
        public ProductModel() 
        {
            IdProducer = Guid.Empty;
            IdDiscount = Guid.Empty;
        }

    }

    public class IMG
    {
        public string img { get; set; }
        public int UsedStatus { get; set; }
    }

    public class RevenueModel
    {
        public string Img { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; }
        public int Remaining { get; set; }
        public Double Price { get; set; }
    }

    public class CencorshipManagementModel
    {
        public Guid Id { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public int UsedStatus { get; set; }
    }

}
