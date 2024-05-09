using BlueBird.DataConext.Data;
using BlueBird.DataConext.Models;
using BlueBird.DataContext.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.Reponsitory.Interface
{
    public interface IProductRepons
    {
        public Task<string> AddProductAsync(ProductModel productModel, string token );
        public Task<(List<Product>, int)> GetProductsAsync(int pageIndex, int pageSize, string productType);
        public Task<ProductDetail> GetProductByIdsAsync(Guid Id);
        public Task<(List<Product>, int)> FindProductAsync(string? search,  int pageIndex, int pageSize, string ProductType);
        public Task<string> UpdateProductAsync(Guid Id,ProductModel productModel, string token);
        public Task<string> DeleteProductAsync(Guid Id);
        public Task<List<CencorshipManagementModel>> GetCencorshipManagementModelAsync(string? search);
        public Task<string> UpdateCencorshipManagementModelAsync(Guid IdProduct, string token);
        public Task<string> DeleteProductCencorshipManagementAsync(Guid Id);
        
    }
}
