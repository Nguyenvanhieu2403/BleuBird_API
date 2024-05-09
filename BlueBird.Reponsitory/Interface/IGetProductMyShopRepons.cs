using BlueBird.DataConext.Models;
using BlueBird.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.Reponsitory.Interface
{
    public interface IGetProductMyShopRepons
    {
        public Task<List<ProductByUserModel>> GetProductByUserAsync(string token, string? search);
        public Task<List<RevenueModel>> GetProductRevenueAsync(string token);
    }
}
