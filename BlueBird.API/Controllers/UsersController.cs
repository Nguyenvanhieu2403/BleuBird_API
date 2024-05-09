using BlueBird.DataConext.Models;
using BlueBird.Reponsitory.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using TimeTable.DataContext.Models;

namespace BlueBird.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepons accRepo;

        public UsersController(IUserRepons repo)
        {
            accRepo = repo;
        }
        [HttpPost("SignUp")]
        public async Task<MethodResult> SignUp(SignUpModel signUpModel)
        {
            string result1 = "";
            string validatePhone = @"^0\d{9}$";
            string validateEmail = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$";
            if (!Regex.IsMatch(signUpModel.Phone, validatePhone) || !Regex.IsMatch(signUpModel.Email, validateEmail))
            {
                result1 = "Số điện thoại hoặc email không hợp lệ";
                return MethodResult.ResultWithError(result1, 400, "Error", 0);
            }
            if (!Regex.IsMatch(signUpModel.Password, passwordPattern))
            {
                result1 = "Mật khẩu phải có ít nhất 8 ký tự, một chữ cái viết hoa, một chữ cái viết thường và một ký tự đặc biệt";
                return MethodResult.ResultWithError(result1, 400, "Error", 0);
            }
            if (signUpModel.Password != signUpModel.ConfirmPassword)
            {
                result1 = "Vui lòng điền hai mật khẩu giống nhau";
                return MethodResult.ResultWithError(result1, 400, "Error", 0);
            }
            var result = await accRepo.SignUpAsync(signUpModel);
            if (result == null)
            {
                result1 = "Lỗi";
                return MethodResult.ResultWithError(result1, 400, "Error", 0);
            }
            result1 = "Đăng ký thành công";
            return MethodResult.ResultWithSuccess(result1, 200, "Successfull", 0);
        }

        [HttpPost("SignUpShop")]
        public async Task<MethodResult> SignUpShop(SignUpShopModel signUpShopModel)
        {
            string result1 = "";
            string validatePhone = @"^0\d{9}$";
            string validateEmail = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$";
            if (!Regex.IsMatch(signUpShopModel.Phone, validatePhone) || !Regex.IsMatch(signUpShopModel.Email, validateEmail))
            {
                result1 = "Số điện thoại hoặc email không hợp lệ";
                return MethodResult.ResultWithError(result1,400,"Error",0);
            }
            if (!Regex.IsMatch(signUpShopModel.Password, passwordPattern))
            {
                result1 = "Mật khẩu phải có ít nhất 8 ký tự, một chữ cái viết hoa, một chữ cái viết thường và một ký tự đặc biệt";
                return MethodResult.ResultWithError(result1, 400, "Error", 0);
            }
            if (signUpShopModel.Password != signUpShopModel.ConfirmPassword)
            {
                result1 = "Vui lòng điền hai mật khẩu giống nhau";
                return MethodResult.ResultWithError(result1, 400, "Error", 0);
            }
            var result = await accRepo.SignUpShopAsync(signUpShopModel);
            if (result == null || result == "Email name or brandname already exists")
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }

        [HttpPost("SignIn")]
        public async Task<Custommessage> SignIn(SignInModel signInModel)
        {
            var result = await accRepo.SignInAsync(signInModel);
            var result1 = new Custommessage();
            var user = await accRepo.GetByIdAsync(signInModel.Email);
            if (user == null) return new Custommessage();
            if (string.IsNullOrEmpty(result))
            {
                throw new Exception();
            }
            if (result == "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ Admin để mở" || result == "Tên đăng nhập hoặc mật khẩu không đúng")
            {
                result1.status = "Error";
            }
            else
            {
                result1.status = "Thành công";
            }
            result1.token = result;
            result1.email = user.Email;
            result1.Id = user.Id;
            result1.name = user.FullName;
            result1.avata = user.Avata;
            result1.first_name = user.First_Name;
            result1.last_name = user.Last_Name;
            result1.password = user.PassWordHas;
            result1.Role = user.Roles;
            return result1;
        }
        [HttpGet]
        public async Task<MethodResult> GetUser(string token)
        {
            var result = await accRepo.GetUserAsync(token);
            if (result == null)
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpGet("ManagerAccount")]
        public async Task<MethodResult> ManagerAccount(string? search)
        {
            var result = await accRepo.ManagerAccountAsync(search);
            if (result == null)
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpPut("UsedState")]
        public async Task<MethodResult> UpdateUser(Guid Id, string token, int UsedState)
        {
            var result = await accRepo.UpdateUserAsync(Id, token, UsedState);
            if (result == null || result == "Thất bại")
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpPut("UpdateRole")]
        public async Task<MethodResult> UpdateRole(Guid Id, string token, string Role)
        {
            var result = await accRepo.UpdateRoleAsync(Id, token, Role);
            if (result == null || result == "Thất bại")
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpDelete]
        public async Task<MethodResult> DeleteUser(Guid Id, string check)
        {
            var result = await accRepo.DeleteUserAsync(Id, check);
            if (result == null || result == "Thất bại")
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
        [HttpPut("UnLockAndLockAsync")]
        public async Task<MethodResult> UnLockAndLockAsync(string check)
        {
            var result = await accRepo.UnLockAndLockAsync(check);
            if (result == null || result == "Thất bại")
            {
                return MethodResult.ResultWithError(result, 400, "Error", 0);
            }
            return MethodResult.ResultWithSuccess(result, 200, "Successfull", 0);
        }
    }
}
