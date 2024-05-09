using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.DataConext.Data
{
    public class Product_Detail
    {
        public Guid Id { get; set; }    
        public Guid IdProduct { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set;}
        public string Color { get; set; }
        public string Size { get; set; }
    }
}
