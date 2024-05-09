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
    public class ChangeAddressController : ControllerBase
    {
        private readonly IChangeAddressRepons _changeAddressRepons;

        public ChangeAddressController(IChangeAddressRepons changeAddressRepons) 
        {
            _changeAddressRepons = changeAddressRepons;
        }
        [HttpGet]
        public async Task<MethodResult> GetAddress(string token)
        {
            var result = await _changeAddressRepons.GetAddressAsync(token);
            if (result == null)
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpPut]
        public async Task<MethodResult> UpdateAddress(ChangeAddressModel changeAddressModel, string token)
        {
            var result = await _changeAddressRepons.ChangeAddressAsync(changeAddressModel, token);
            if (result == null || result == "Sửa địa chỉ thất bại" || result == "Token không hợp lệ")
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
    }
}
