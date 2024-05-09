using BlueBird.DataConext.Models;
using BlueBird.Reponsitory;
using BlueBird.Reponsitory.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTable.DataContext.Models;

namespace BlueBird.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepons _productRepons;

        public ProductController(IProductRepons productRepons) 
        {
            _productRepons = productRepons;
        }
        [HttpPost]
        public async Task<MethodResult> AddProduct(ProductModel model, string token )
        {
            var result = await _productRepons.AddProductAsync(model, token);
            if(result == null || result == "Tên sản phẩm đã tồn tại" || result == "Lỗi khi thêm sản phẩm" || result == "Lỗi khi thêm ảnh hoặc chi tiết sản phẩm")
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpGet]
        public async Task<MethodResult> GetAllProduct(int pageIndex, int pageSize, string ProductType)
        {
            var result = await _productRepons.GetProductsAsync(pageIndex, pageSize, ProductType);
            if(result.Item1 == null )
            {
                return MethodResult.ResultWithError(result.Item1, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result.Item1, 200, "Successfull", result.Item2);
        }
        [HttpGet("Search")]
        public async Task<MethodResult> FindProductBySearch(string? search, int pageIndex, int pageSize, string ProductType)
        {
            var result = await _productRepons.FindProductAsync(search, pageIndex, pageSize, ProductType);
            if (result.Item1 == null)
            {
                return MethodResult.ResultWithError(result.Item1, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result.Item1, 200, "Successfull", result.Item2);
        }

        [HttpGet("Id")]
        public async Task<MethodResult> GetProductById(Guid Id)
        {
            var result = await _productRepons.GetProductByIdsAsync(Id);
            if (result == null)
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpDelete] 
        public async Task<MethodResult> DeleteProduct(Guid Id)
        {
            var result = await _productRepons.DeleteProductAsync(Id);
            if (result == null)
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpPut]
        public async Task<MethodResult> UpdateProduct(Guid Id, ProductModel productModel, string token)
        {
            var result  = await _productRepons.UpdateProductAsync(Id, productModel, token);
            if (result == null)
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpGet("CencorshipManagement")]
        public async Task<MethodResult> GetCencorshipManagement(string? search)
        {
            var result = await _productRepons.GetCencorshipManagementModelAsync(search);
            if (result == null)
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpPut("CencorshipManagement")]
        public async Task<MethodResult> UpdateCencorshipManagement(Guid IdProduct, string token)
        {
            var result = await _productRepons.UpdateCencorshipManagementModelAsync(IdProduct, token);
            if (result == null || result == "Kiểm duyệt thất bại")
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpDelete("DeleteProductCencorshipManagement")]
        public async Task<MethodResult> DeleteCencorshipManagement(Guid Id)
        {
            var result = await _productRepons.DeleteProductCencorshipManagementAsync(Id);
            if (result == null || result == "Xóa kiểm duyệt thất bại")
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        
    }
}                               
