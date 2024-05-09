using BlueBird.DataConext.Data;
using BlueBird.DataConext.Models;
using BlueBird.Reponsitory.Interface;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TimeTable.DataContext.Data;
using TimeTable.DataContext.Models;

namespace BlueBird.Reponsitory
{
    public class UserRepons : IUserRepons
    {
        private readonly ConnectToSql _connectToSql;
        private readonly IConfiguration _configuration;

        public UserRepons(ConnectToSql connectToSql, IConfiguration configuration)
        {
            _connectToSql = connectToSql;
            _configuration = configuration;
        }
        public async Task<string> SignInAsync(SignInModel signInModel)
        {
            try
            {
                var salt = PasswordManager.GenerateSalt(); // Tạo chuỗi salt mới cho mật khẩu
                var hashedPassword = PasswordManager.HashPassword(signInModel.Password, salt);
                string query = "select count(*) from dbo.Users where Email = @Email and PassWordHas = @PassWordHas";
                var parameter = new DynamicParameters();
                parameter.Add("Email", signInModel.Email, DbType.String);
                parameter.Add("PassWordHas", hashedPassword, DbType.String);

                using (var connect = _connectToSql.CreateConnection())
                {
                    var count = await connect.ExecuteScalarAsync<int>(query, parameter);
                    if (count != 0)
                    {
                        var userQuery = "SELECT * FROM dbo.Users WHERE Email = @Email";
                        var users = await connect.QueryFirstOrDefaultAsync<Users>(userQuery, parameter);
                        if (users.UsedState == 11 || users.UsedState == 12)
                        {
                            return "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ Admin để mở";
                        }
                        var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, signInModel.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim("UserName", users.FullName),
                            new Claim("Email", users.Email),
                            new Claim("UserId", users.Id.ToString()),
                            new Claim(ClaimTypes.Role, users.Roles),
                            new Claim ("PhoneNumber", users.Phone.ToString()),
                            new Claim ("Gender", users.Gender.ToString()),
                            new Claim ("DateOfBirth", users.DateOfBirth.ToString()),
                            new Claim("Avata", users.Avata),

                        };
                        var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:ISK"]));
                        var token = new JwtSecurityToken(
                                issuer: _configuration["JWT:ValidIssuer"],
                                audience: _configuration["JWT:ValidAudience"],
                                expires: DateTime.Now.AddMinutes(30),
                                claims: authClaims,
                                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                        );

                        return new JwtSecurityTokenHandler().WriteToken(token);
                    }
                    else
                    {
                        return "Tên đăng nhập hoặc mật khẩu không đúng";
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> SignUpAsync(SignUpModel signUpModel)
        {
            try
            {
                using var connect = _connectToSql.CreateConnection();
                var salt = PasswordManager.GenerateSalt(); // Tạo chuỗi salt mới cho mật khẩu
                var hashedPassword = PasswordManager.HashPassword(signUpModel.Password, salt); // Mã hóa mật khẩu

                var parameter = new DynamicParameters();
                parameter.Add("FirstName", signUpModel.First_Name, DbType.String);
                parameter.Add("LastName", signUpModel.Last_Name, DbType.String);
                parameter.Add("Email", signUpModel.Email, DbType.String);
                parameter.Add("Password", hashedPassword, DbType.String); // Lưu mật khẩu đã mã hóa
                parameter.Add("PhoneNumber", signUpModel.Phone, DbType.Int64);
                parameter.Add("Address", signUpModel.Address, DbType.String);
                parameter.Add("Roles", "User", DbType.String);
                parameter.Add("Gender", 1, DbType.Int32);
                parameter.Add("DateOfBirth", DateTime.Now, DbType.Date);
                parameter.Add("Avata", "", DbType.String);

                var result = await connect.QueryFirstOrDefaultAsync<string>("SignUpUser", parameter, commandType: CommandType.StoredProcedure);

                if (result == "SignUp Success")
                {
                    return "SignUp Success";
                }
                else
                {
                    return "SignUp Error";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Users> GetByIdAsync(string email)
        {
            using var connect = _connectToSql.CreateConnection();
            var searchEmail = await connect.QueryFirstOrDefaultAsync<Users>("GetByEmail", new { Email = email }, commandType: CommandType.StoredProcedure);
            return searchEmail;
        }

        public async Task<string> SignUpShopAsync(SignUpShopModel signUpShopModel)
        {
            try
            {
                using var connect = _connectToSql.CreateConnection();
                var salt = PasswordManager.GenerateSalt(); // Tạo chuỗi salt mới cho mật khẩu
                var hashedPassword = PasswordManager.HashPassword(signUpShopModel.Password, salt); // Mã hóa mật khẩu

                var parameter = new DynamicParameters();
                Guid Id = Guid.NewGuid();
                Guid IdShop = Guid.NewGuid();
                parameter.Add("IdUser", Id);
                parameter.Add("FirstName", signUpShopModel.First_Name, DbType.String);
                parameter.Add("LastName", signUpShopModel.Last_Name, DbType.String);
                parameter.Add("Email", signUpShopModel.Email, DbType.String);
                parameter.Add("Password", hashedPassword, DbType.String); // Lưu mật khẩu đã mã hóa
                parameter.Add("PhoneNumber", signUpShopModel.Phone, DbType.Int64);
                parameter.Add("Address", signUpShopModel.Address, DbType.String);
                parameter.Add("IdShop", IdShop);
                parameter.Add("BrandName", signUpShopModel.BrandName, DbType.String);
                parameter.Add("AvataShop", signUpShopModel.Avata, DbType.String);
                parameter.Add("Roles", "Shop", DbType.String);
                parameter.Add("Gender", 1, DbType.Int32);
                parameter.Add("DateOfBirth", DateTime.Now, DbType.Date);
                parameter.Add("Avata", "https://img.freepik.com/free-icon/user_318-159711.jpg", DbType.String);

                var result = await connect.QueryFirstOrDefaultAsync<string>("SignUpUserShop", parameter, commandType: CommandType.StoredProcedure);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserShow> GetUserAsync(string token)
        {
            try
            {
                using (var connect = _connectToSql.CreateConnection())
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var decodedToken = tokenHandler.ReadJwtToken(token);
                    var userIdClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "UserId");
                    if (userIdClaim == null)
                    {
                        return null;
                    }
                    Guid UserId = Guid.Parse(userIdClaim.Value);
                    var parameters = new DynamicParameters();
                    parameters.Add("@IdUser", UserId);

                    var result = await connect.QueryFirstOrDefaultAsync<UserShow>("GetInforUser",parameters,commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<List<ManagerAccount>> ManagerAccountAsync(string? search)
        {
            try
            {
                using var connect = _connectToSql.CreateConnection();
                var parameters = new DynamicParameters();
                if(search == null) search = "string";
                parameters.Add("@search", search);
                var result = connect.Query<ManagerAccount>("AccountManager", parameters, commandType: CommandType.StoredProcedure);
                return Task.FromResult(result.ToList());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<string> UpdateUserAsync(Guid Id, string token, int UsedState)
        {
            try
            {
                using var connect = _connectToSql.CreateConnection();
                connect.Open();
                var tokenHandler = new JwtSecurityTokenHandler();
                var decodedToken = tokenHandler.ReadJwtToken(token);
                var userIdClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null)
                {
                    return null;
                }
                Guid UserId = Guid.Parse(userIdClaim.Value);
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "UpdateUser";
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@IdUser", UserId);
                command.Parameters.AddWithValue("@UsedState", UsedState);
                command.Connection = (SqlConnection)connect;
                // Add the @Result parameter for the stored procedure (output parameter).
                SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.NVarChar, -1);
                resultParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(resultParam);
                // Open the connection before executing the command.
                command.ExecuteNonQuery();
                return Task.FromResult(resultParam.Value.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<string> UpdateRoleAsync(Guid Id, string token, string Role)
        {
            try
            {
                using var connect = _connectToSql.CreateConnection();
                connect.Open();
                var tokenHandler = new JwtSecurityTokenHandler();
                var decodedToken = tokenHandler.ReadJwtToken(token);
                var userIdClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null)
                {
                    return null;
                }
                Guid UserId = Guid.Parse(userIdClaim.Value);
                var command = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "UpdateRole"
                };
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@IdUser", UserId);
                command.Parameters.AddWithValue("@Role", Role);
                command.Connection = (SqlConnection)connect;
                // Add the @Result parameter for the stored procedure (output parameter).
                var resultParam = new SqlParameter("@Result", SqlDbType.NVarChar, -1)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(resultParam);
                // Open the connection before executing the command.
                command.ExecuteNonQuery();
                return Task.FromResult(resultParam.Value.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<string> DeleteUserAsync(Guid Id, string check)
        {
            try
            {
                using var connect = _connectToSql.CreateConnection();
                connect.Open();
                var command = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "DeleteUser"
                };
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Check", check);
                command.Connection = (SqlConnection)connect;
                // Add the @Result parameter for the stored procedure (output parameter).
                var resultParam = new SqlParameter("@Result", SqlDbType.NVarChar, -1)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(resultParam);
                // Open the connection before executing the command.
                command.ExecuteNonQuery();
                return Task.FromResult(resultParam.Value.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<string> UnLockAndLockAsync(string  check)
        {
            try
            {
                var resultParam = new SqlParameter();
                if (check == "LockAccount")
                {
                    using var connect = _connectToSql.CreateConnection();
                    connect.Open();
                    var command = new SqlCommand
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "UnlockAllAccount"
                    };
                    command.Parameters.AddWithValue("@UsedState", 11);
                    command.Connection = (SqlConnection)connect;
                    // Add the @Result parameter for the stored procedure (output parameter).
                    resultParam = new SqlParameter("@Result", SqlDbType.NVarChar, -1)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(resultParam);
                    // Open the connection before executing the command.
                    command.ExecuteNonQuery();
                }
                else if(check == "UnlockAccount")
                {
                    using var connect = _connectToSql.CreateConnection();
                    connect.Open();
                    var command = new SqlCommand
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "UnlockAllAccount"
                    };
                    command.Parameters.AddWithValue("@UsedState", 10);
                    command.Connection = (SqlConnection)connect;
                    // Add the @Result parameter for the stored procedure (output parameter).
                    resultParam = new SqlParameter("@Result", SqlDbType.NVarChar, -1)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(resultParam);
                    // Open the connection before executing the command.
                    command.ExecuteNonQuery();
                }
                else if(check =="LockShop")
                {
                    using var connect = _connectToSql.CreateConnection();
                    connect.Open();
                    var command = new SqlCommand
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "LockAndUnlockShop"
                    };
                    command.Parameters.AddWithValue("@Role", "User");
                    command.Connection = (SqlConnection)connect;
                    // Add the @Result parameter for the stored procedure (output parameter).
                    resultParam = new SqlParameter("@Result", SqlDbType.NVarChar, -1)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(resultParam);
                    // Open the connection before executing the command.
                    command.ExecuteNonQuery();
                }
                else if(check == "UnlockShop")
                {
                    using var connect = _connectToSql.CreateConnection();
                    connect.Open();
                    var command = new SqlCommand
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "LockAndUnlockShop"
                    };
                    command.Parameters.AddWithValue("@Role", "Shop");
                    command.Connection = (SqlConnection)connect;
                    // Add the @Result parameter for the stored procedure (output parameter).
                    resultParam = new SqlParameter("@Result", SqlDbType.NVarChar, -1)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(resultParam);
                    // Open the connection before executing the command.
                    command.ExecuteNonQuery();
                }
                return Task.FromResult(resultParam.Value.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
