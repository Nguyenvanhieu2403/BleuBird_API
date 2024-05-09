using BlueBird.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.Reponsitory.Interface
{
    public interface IOrdersRepons
    {
        public Task<string> AddOrdersAsync(string token, OrdersModel orders, Guid? IdCart);
        public Task<string> AddOrdersListAsync(string token, OrdersModel orders, string ListIdCart);
        public Task<string> DeleteOrdersListAsync(string ListIdCart);
        public Task<(List<OrdersModelDisplay>, int)> GetAllOrderAsync(int pageIndex, int pageSize, string? search, string token);
    }
}
