using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.DataContext.Models
{
    public class ChangePassWordModel
    {
        public string PassWordOld { get; set; }
        public string PassWordNew { get; set;}
        public string ConfirmPassWord { get; set; }
    }
}
