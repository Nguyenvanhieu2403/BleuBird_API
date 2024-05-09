using BlueBird.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.Reponsitory.Interface
{
    public interface ICartRepons
    {
        public Task<string> AddCartAsync(List<CartModel> model, string token);
        public Task<(List<CartDispay>, int)> GetAllCartAsync(int pageIndex, int pageSize, string token, string search);
        public Task<string> DeleteCartAsync(Guid Id);
        public Task<string> UpdateCartAsync(Guid Id, CartModel model);
    }
}
