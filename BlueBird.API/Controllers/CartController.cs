using BlueBird.DataContext.Models;
using BlueBird.Reponsitory.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTable.DataContext.Models;

namespace BlueBird.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepons _cartRepons;

        public CartController(ICartRepons cartRepons) 
        {
            _cartRepons = cartRepons;
        }

        [HttpPost] 
        public async Task<MethodResult> AddCart(string token, List<CartModel> model)
        {
            var result = await _cartRepons.AddCartAsync(model, token);
            if (result == null)
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpGet]
        public async Task<MethodResult> GetAllCart( int pageIndex, int pageSize, string token, string? search)
        {
            var result = await _cartRepons.GetAllCartAsync(pageIndex, pageSize, token, search);
            if(result.Item1 == null)
            {
                return MethodResult.ResultWithError(result.Item1, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result.Item1, 200, "Successfull", result.Item2);
        }
        [HttpDelete]
        public async Task<MethodResult> DeleteCart(Guid Id)
        {
            var result = await _cartRepons.DeleteCartAsync(Id);
            if (result == null)
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpPut]
        public async Task<MethodResult> UpdateCart(Guid Id, CartModel cartModel)
        {
            var result = await _cartRepons.UpdateCartAsync(Id, cartModel);
            if (result == null)
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
    }
}
