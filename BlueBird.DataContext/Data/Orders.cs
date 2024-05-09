using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.DataConext.Data
{
    public class Orders
    {
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }
        public string Reveice_Name { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        public string Description { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set;}
        public int UsedStatus { get; set; }
        public int Number { get; set; }
        public double ShipPrice { get; set; }
        public float TotalPrice { get; set; }
        public Guid IdCart { get; set; }
        public Guid IdDiscount { get; set; }
    }
}
