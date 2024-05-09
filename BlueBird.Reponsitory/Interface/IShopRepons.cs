using BlueBird.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.Reponsitory.Interface
{
    public interface IShopRepons
    {
        public Task<List<ShopModel>> GetProductByUserAsync(Guid IdProduct);
        public Task<List<ManagerShop>> GetManagerShopAsync(string? search);
    }
}
