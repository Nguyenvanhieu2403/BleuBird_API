using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.DataContext.Models
{
    public class FeedBackModel
    {
        public string Description { get; set; }
    }

    public class FeedBackDisplayModel : FeedBackModel 
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Avata { get; set; }
        public DateTime DateCreate { get; set; }
    
    }
}
