using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.DataConext.Data
{
    public class Discount
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Percents { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
