using BlueBird.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.Reponsitory.Interface
{
    public interface IChangePassRepons
    {
        public Task<string> ChangePassWordAsync(ChangePassWordModel model, string token);
    }
}
