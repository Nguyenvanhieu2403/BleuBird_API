using BlueBird.Reponsitory.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTable.DataContext.Models;

namespace BlueBird.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopRepons _shopRepons;

        public ShopController(IShopRepons shopRepons) 
        {
            _shopRepons = shopRepons;
        }
        [HttpGet]
        public async Task<MethodResult> GetShop(Guid IdProduct)
        {
            var result = await _shopRepons.GetProductByUserAsync(IdProduct);
            if (result == null)
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpGet("ManagerShop")]
        public async Task<MethodResult> GetManagerShop(string? search)
        {
            var result = await _shopRepons.GetManagerShopAsync(search);
            if (result == null)
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
    }
}
