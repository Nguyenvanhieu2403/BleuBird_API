using BlueBird.DataConext.Models;
using BlueBird.DataContext.Models;
using BlueBird.Reponsitory.Interface;
using Dapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TimeTable.DataContext.Data;
using TimeTable.DataContext.Models;

namespace BlueBird.Reponsitory
{
    public class ChangePassRepons : IChangePassRepons
    {
        private readonly ConnectToSql _connectToSql;

        public ChangePassRepons(ConnectToSql connectToSql)
        {
            _connectToSql =  connectToSql;
        }
        public async Task<string> ChangePassWordAsync(ChangePassWordModel model, string token)
        {
            try
            {
                var salt = PasswordManager.GenerateSalt(); 
                var PassOld = PasswordManager.HashPassword(model.PassWordOld, salt); 
                var PassNew = PasswordManager.HashPassword(model.PassWordNew, salt);
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
                    command.CommandText = "ChangePassWord";
                    command.Parameters.AddWithValue("@Id", UserId);
                    command.Parameters.AddWithValue("@PassOld",PassOld );
                    command.Parameters.AddWithValue("@PassNew", PassNew);
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
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
