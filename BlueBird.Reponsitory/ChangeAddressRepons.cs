using BlueBird.DataContext.Models;
using BlueBird.Reponsitory.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TimeTable.DataContext.Data;

namespace BlueBird.Reponsitory
{
    public class ChangeAddressRepons : IChangeAddressRepons
    {
        private readonly ConnectToSql _connectToSql;

        public ChangeAddressRepons(ConnectToSql connectToSql) 
        {
            _connectToSql = connectToSql;
        }
        public async Task<string> ChangeAddressAsync(ChangeAddressModel changeAddressModel, string token)
        {
            try
            {
                string result = string.Empty;
                using (var connect = _connectToSql.CreateConnection())
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var decodedToken = tokenHandler.ReadJwtToken(token);
                    var userIdClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "UserId");
                    if (userIdClaim == null)
                    {
                        result = "Token không hợp lệ";
                    }
                    Guid UserId = Guid.Parse(userIdClaim.Value);
                    SqlCommand command = new SqlCommand();
                    var command1 = connect.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "UpdateAddress";
                    command.Parameters.AddWithValue("@Id", UserId);
                    command.Parameters.AddWithValue("@FirstName", changeAddressModel.FirstName);
                    command.Parameters.AddWithValue("@LastName", changeAddressModel.LastName);
                    command.Parameters.AddWithValue("@Phone", changeAddressModel.PhoneNumber);
                    command.Parameters.AddWithValue("@Address", changeAddressModel.Address);
                    command.Connection = (SqlConnection)connect;

                    // Add the @Result parameter for the stored procedure (output parameter).
                    SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.NVarChar, -1);
                    resultParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultParam);
                    connect.Open(); // Open the connection before executing the command.
                    await command.ExecuteNonQueryAsync();
                    result = resultParam.Value.ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ChangeAddressModel> GetAddressAsync(string token)
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
                        return new ChangeAddressModel { };
                    }
                    Guid UserId = Guid.Parse(userIdClaim.Value);
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", UserId);

                    var result = await connect.QueryFirstOrDefaultAsync<ChangeAddressModel>(
                        "GetAddress",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
