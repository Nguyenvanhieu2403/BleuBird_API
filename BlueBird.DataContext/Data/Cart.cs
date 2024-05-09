using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.DataConext.Data
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }
        public Guid IdProduct { get; set; }
        public int Number { get ; set; }
        public float TotalPrice { get; set; }
    }
}
