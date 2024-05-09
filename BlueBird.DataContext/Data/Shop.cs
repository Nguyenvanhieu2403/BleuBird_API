using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.DataConext.Data
{
    public class Shop
    {
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }
        public string BrandName { get; set; }
        public string Avata { get ; set; }
    }
}
