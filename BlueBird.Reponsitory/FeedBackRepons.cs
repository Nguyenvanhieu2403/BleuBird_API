using BlueBird.DataConext.Data;
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TimeTable.DataContext.Data;

namespace BlueBird.Reponsitory
{
    public class FeedBackRepons : IFeedBackRepons
    {
        private readonly ConnectToSql _connectToSql;

        public FeedBackRepons(ConnectToSql connectToSql) 
        {
            _connectToSql = connectToSql;
        }
        public async Task<string> DeleteFeedBackAsync(Guid IdProduct, string token)
        {
            string result = string.Empty;
            try
            {
                using (var connect = _connectToSql.CreateConnection())
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var decodedToken = tokenHandler.ReadJwtToken(token);
                    var userIdClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "UserId");
                    if (userIdClaim == null)
                    {
                        return "Token không hợp lệ";
                    }
                    Guid UserId = Guid.Parse(userIdClaim.Value);
                    SqlCommand command = new SqlCommand();
                    var command1 = connect.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "DeleteFeedBack";
                    command.Parameters.AddWithValue("@IdUser", UserId);
                    command.Parameters.AddWithValue("@IdPrduct", IdProduct);
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

        public async Task<(List<FeedBackDisplayModel>, int)> GetAllFeedBack(Guid IdProduct, int pageIndex, int pageSize)
        {
            try
            {
                using (var connect = _connectToSql.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@IdProduct", IdProduct);
                    parameters.Add("@pageIndex", pageIndex);
                    parameters.Add("@pageSize", pageSize);
                    parameters.Add("@totalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var result = await connect.QueryAsync<FeedBackDisplayModel>(
                        "GetALLFeedBack",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    int totalRecords = parameters.Get<int>("@totalRecord");
                    return (result.ToList(), totalRecords);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> PostFeedBackAsync(string token, Guid IdProduct, FeedBackModel feedBackModel)
        {
            string result = "";
            try
            {
                using(var connect = _connectToSql.CreateConnection())
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var decodedToken = tokenHandler.ReadJwtToken(token);
                    var userIdClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "UserId");
                    if (userIdClaim == null)
                    {
                        return "Token không hợp lệ";
                    }
                    Guid UserId = Guid.Parse(userIdClaim.Value);
                    SqlCommand command = new SqlCommand();
                    var command1 = connect.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "AddFeedBack";
                    Guid IdFeedBack = Guid.NewGuid();
                    command.Parameters.AddWithValue("@Id", IdFeedBack);
                    command.Parameters.AddWithValue("@IdUser", UserId);
                    command.Parameters.AddWithValue("@IdProduct", IdProduct); 
                    command.Parameters.AddWithValue("@Description", feedBackModel.Description); 
                    command.Parameters.AddWithValue("@DateCreate", DateTime.Now);
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

        public async Task<string> UpdateFeedBackAsync(string token, Guid Id, FeedBackModel feedBackModel)
        {
            string result = "";
            try
            {
                using (var connect = _connectToSql.CreateConnection())
                {
                    connect.Open();
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var decodedToken = tokenHandler.ReadJwtToken(token);
                    var userIdClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "UserId");
                    if (userIdClaim == null)
                    {
                        return "Token không hợp lệ";
                    }
                    Guid UserId = Guid.Parse(userIdClaim.Value);
                    SqlCommand command = new SqlCommand();
                    var command1 = connect.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "UpdateFeedBack";
                    Guid IdFeedBack = Guid.NewGuid();
                    command.Parameters.AddWithValue("@Id", IdFeedBack);
                    command.Parameters.AddWithValue("@IdUser", UserId);
                    command.Parameters.AddWithValue("@Description", feedBackModel.Description);
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
    }
}
