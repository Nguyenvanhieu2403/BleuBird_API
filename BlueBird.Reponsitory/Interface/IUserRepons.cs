using BlueBird.DataConext.Data;
using BlueBird.DataConext.Models;

namespace BlueBird.Reponsitory.Interface
{
    public interface IUserRepons
    {
        public Task<string> SignUpAsync(SignUpModel signUpModel);
        public Task<string> SignUpShopAsync(SignUpShopModel signUpShopModel);
        public Task<string> SignInAsync(SignInModel signInModel);
        public Task<Users> GetByIdAsync(string email);
        public Task<UserShow> GetUserAsync(string token);
        public Task<List<ManagerAccount>> ManagerAccountAsync(string? search);
        public Task<string> UpdateUserAsync(Guid Id, string token, int UsedState);
        public Task<string> UpdateRoleAsync(Guid Id, string token, string Role);
        public Task<string> DeleteUserAsync(Guid Id, string check);
        public Task<string> UnLockAndLockAsync(string check);
    }
}
