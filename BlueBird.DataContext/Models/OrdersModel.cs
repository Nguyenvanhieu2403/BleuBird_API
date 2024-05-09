using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.DataContext.Models
{
    public class OrdersModel
    {
        public Guid Id { get; set; }
        public Guid IdProduct { get; set; }
        public Guid? IdDiscount { get; set; } = Guid.Empty;
        public int Number { get; set; }
        public double ShipPrice { get; set; }
        public string Description { get; set; }
    }
    public class OrdersModelDisplay
    {
        public Guid Id { get; set; }
        public Guid IdProduct { get; set; }
        public string BrandName { get; set; }
        public string Name { get; set; }
        public float Price { get; set; } 
        public string Color { get; set; }
        public string Size { get; set; }
        public float ShipPrice { get; set; }
        public int Number { get; set; }
        public float TotalPrice { get; set; }
        public int UsedStatus { get; set; }
        public string Img { get; set; }
    }
}
