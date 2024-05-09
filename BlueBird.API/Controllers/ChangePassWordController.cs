using BlueBird.DataConext.Models;
using BlueBird.DataContext.Models;
using BlueBird.Reponsitory.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using TimeTable.DataContext.Models;

namespace BlueBird.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangePassWordController : ControllerBase
    {
        private readonly IChangePassRepons _changePassRepons;

        public ChangePassWordController(IChangePassRepons changePassRepons)
        {
            _changePassRepons = changePassRepons;
        }
        [HttpPut]
        public async Task<MethodResult> ChangePassWord(ChangePassWordModel model, string token)
        {
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$";
            if (!Regex.IsMatch(model.PassWordNew, passwordPattern) || !Regex.IsMatch(model.ConfirmPassWord, passwordPattern))
            {
                string result1 = "Mật khẩu phải có ít nhất 8 ký tự, một chữ cái viết hoa, một chữ cái viết thường và một ký tự đặc biệt";
                return MethodResult.ResultWithError(result1, 400, "Error", 0);
            }
            if (model.PassWordNew != model.ConfirmPassWord)
            {
                return MethodResult.ResultWithError("Vui lòng điền hai mật khẩu giống nhau", 400, "ERROR", 0);
            }
            var result = await _changePassRepons.ChangePassWordAsync(model, token);
            if(result == null || result == "Mật khẩu không đúng" || result == "Đổi mật khẩu thất bại")
            {
                return MethodResult.ResultWithError(result, 400, "ERROR", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
    }
}
