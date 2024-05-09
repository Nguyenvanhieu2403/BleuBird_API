using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.DataConext.Data
{
    public class FeedBack
    {
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }
        public Guid IdProduct { get; set; }
        public string Description { get; set; } 
        public DateTime CreateDate { get; set; }
    }
}
