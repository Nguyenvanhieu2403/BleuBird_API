using BlueBird.DataContext.Models;
using BlueBird.Reponsitory;
using BlueBird.Reponsitory.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTable.DataContext.Models;

namespace BlueBird.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeRepons _productTypeRepons;
        private readonly IConfiguration _config;

        public ProductTypeController(IProductTypeRepons productTypeRepons, IConfiguration config) 
        {
            _productTypeRepons = productTypeRepons;
            _config = config;
        }
        [HttpPost]
        public async Task<MethodResult> AddProductType(ProductTypeModel productTypeModel, string token)
        {
            var result = await _productTypeRepons.AddProductTypeAsync(productTypeModel, token);
            if(result == null || result == "Thêm loại hàng này thất bại" || result == "Loại hàng này đã tồn tại")
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }

        [HttpGet]
        public async Task<MethodResult> GetAllProductType(int pageIndex, int pageSize, string? search)
        {
            var result = await _productTypeRepons.GetAllProductType(pageIndex, pageSize, search);
            if (result.Item1 == null)
            {
                return MethodResult.ResultWithError(result.Item1, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result.Item1, 200, "Successfull", result.Item2);
        }

        [HttpPut]
        public async Task<MethodResult> UpdateProductType(Guid Id, ProductTypeModel productTypeModel, string token)
        {
            var result = await _productTypeRepons.UpdateProductTypeAsync(Id, productTypeModel, token);
            if(result == null || result == "Sửa loại sản phẩm không thành công")
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }

        [HttpPost("Test")]
        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            var tenfile = "";
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // Lấy đường dẫn thư mục lưu trữ từ cấu hình
                    var storedFilesPath = Environment.CurrentDirectory  +  "\\IMG\\";
                    // Tạo tên tệp ngẫu nhiên cho tệp ảnh
                    var fileName = formFile.FileName;
                    tenfile = "~/uploads/" + formFile.FileName; ;

                    // Kết hợp đường dẫn đầy đủ cho tệp ảnh
                    var filePath = Path.Combine(storedFilesPath, fileName);

                    // Lưu trữ tệp ảnh
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // Xử lý các tệp đã tải lên
            // Không nên tin tưởng hoặc sử dụng thuộc tính FileName mà không kiểm tra và xác thực nó.

            return Ok(new { count = files.Count, size, tenfile });
        }

    }
}
