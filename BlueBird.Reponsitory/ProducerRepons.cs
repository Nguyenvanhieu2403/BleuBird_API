using BlueBird.DataConext.Data;
using BlueBird.DataConext.Models;
using BlueBird.DataContext.Models;
using BlueBird.Reponsitory.Interface;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TimeTable.DataContext.Data;

namespace BlueBird.Reponsitory
{
    public class ProducerRepons : IProducerRepons
    {
        private readonly ConnectToSql _connectToSql;

        public ProducerRepons(ConnectToSql connectToSql)
        {
            _connectToSql = connectToSql;
        }
        public async Task<string> AddProducerAsync(ProducerModel producerModel, string token)
        {
            string result = null;
            try
            {
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
                    command.CommandText = "AddProducer";
                    Guid IdClass = Guid.NewGuid();
                    command.Parameters.AddWithValue("@Id", IdClass);
                    command.Parameters.AddWithValue("@Name", producerModel.Name);
                    command.Parameters.AddWithValue("@Country", producerModel.Country);
                    command.Parameters.AddWithValue("@Address", producerModel.Address);
                    command.Parameters.AddWithValue("@Description", producerModel.Description);
                    command.Parameters.AddWithValue("@UsedStated", 0);
                    command.Parameters.AddWithValue("@CreateBy", UserId);
                    command.Parameters.AddWithValue("@CreateDate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("@ModifiedDate", DateTime.UtcNow);
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

        public async Task<string> DeleteProducerAsync(Guid Id)
        {
            string result = string.Empty;
            try
            {
                using (var connect = _connectToSql.CreateConnection())
                {
                    SqlCommand command = new SqlCommand();
                    var command1 = connect.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "DeleteProducer";
                    command.Parameters.AddWithValue("@Id", Id);
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

        public async Task<(List<Producer>, int)> GetAllProducerAsync(int pageIndex, int pageSize)
        {
            try
            
            {
                using(var connect = _connectToSql.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@pageIndex", pageIndex);
                    parameters.Add("@pageSize", pageSize);
                    parameters.Add("@totalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var result = await connect.QueryAsync<Producer>(
                        "GetAllProducer",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    int totalRecords = parameters.Get<int>("@totalRecords");
                    return (result.ToList(), totalRecords);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(List<Producer>, int)> GetProducerBySearchAsync(string search, int pageIndex, int pageSize)
        {
            try
            {
                using (var connect = _connectToSql.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@pageIndex", pageIndex);
                    parameters.Add("@search", search);
                    parameters.Add("@pageSize", pageSize);
                    parameters.Add("@totalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var result = await connect.QueryAsync<Producer>(
                        "GetProducerBySearch",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    int totalRecords = parameters.Get<int>("@totalRecords");
                    return (result.ToList(), totalRecords);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> UpdateProducerAsync(Guid Id, ProducerModel producerModel, string token)
        {
            string result = null;
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
                        result = "Token không hợp lệ";
                    }
                    Guid UserId = Guid.Parse(userIdClaim.Value);
                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "UpdateProducer";
                    Guid IdProduct = Guid.NewGuid();
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@Name", producerModel.Name);
                    command.Parameters.AddWithValue("@Country", producerModel.Country);
                    command.Parameters.AddWithValue("@Address", producerModel.Address);
                    command.Parameters.AddWithValue("@Description", producerModel.Description);   
                    command.Parameters.AddWithValue("@ModifiedBy", UserId);
                    command.Parameters.AddWithValue("@ModifiedDate", DateTime.UtcNow);
                    command.Connection = (SqlConnection)connect;
                    //ImportImg(files, UserId, IdProduct);
                    // Add the @Result parameter for the stored procedure (output parameter).
                    SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.NVarChar, -1);
                    resultParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultParam);
                    // Open the connection before executing the command.
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
