using BlueBird.Reponsitory;
using BlueBird.Reponsitory.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTable.DataContext.Models;

namespace BlueBird.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetProductMyShopController : ControllerBase
    {
        private readonly IGetProductMyShopRepons _getProductMyShopRepons;

        public GetProductMyShopController(IGetProductMyShopRepons getProductMyShopRepons) 
        {
            _getProductMyShopRepons = getProductMyShopRepons;
        }
        [HttpGet]
        public async Task<MethodResult> GetProductByUser(string token, string? search)
        {
            var result = await _getProductMyShopRepons.GetProductByUserAsync(token, search);
            if (result == null)
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpGet("Revenue")]
        public async Task<MethodResult> GetProductRevenue(string token)
        {
            var result = await _getProductMyShopRepons.GetProductRevenueAsync(token);
            if (result == null)
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
    }
}
