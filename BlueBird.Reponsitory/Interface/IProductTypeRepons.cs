using BlueBird.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.Reponsitory.Interface
{
    public interface IProductTypeRepons
    {
        public Task<string> AddProductTypeAsync(ProductTypeModel productType, string token);
        public Task<string> UpdateProductTypeAsync(Guid Id, ProductTypeModel productType, string token);
        public Task<string> DeleteProductTypeAsync(Guid Id);
        public Task<(List<ProductTypeModel>, int)> GetAllProductType(int pageIndex, int pageSize, string search);
    }
}
