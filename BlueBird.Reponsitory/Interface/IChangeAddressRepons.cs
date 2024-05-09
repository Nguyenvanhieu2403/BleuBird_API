using BlueBird.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.Reponsitory.Interface
{
    public interface IChangeAddressRepons
    {
        public Task<ChangeAddressModel> GetAddressAsync(string token);
        public Task<string> ChangeAddressAsync(ChangeAddressModel changeAddressModel, string token);
    }
}
