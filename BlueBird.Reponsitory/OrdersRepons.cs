using BlueBird.DataConext.Models;
using BlueBird.DataContext.Models;
using BlueBird.Reponsitory.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable.DataContext.Data;

namespace BlueBird.Reponsitory
{
    public class OrdersRepons : IOrdersRepons
    {
        private readonly ConnectToSql _connectToSql;

        public OrdersRepons(ConnectToSql connectToSql) 
        {
            _connectToSql = connectToSql;
        }
        public async Task<string> AddOrdersAsync(string token, OrdersModel orders, Guid? IdCart)
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
                    command.CommandText = "AddOrders";
                    Guid IdOrder = Guid.NewGuid();
                    command.Parameters.AddWithValue("@Id", IdOrder);
                    command.Parameters.AddWithValue("@IdUser",UserId );
                    
                    command.Parameters.AddWithValue("@IdCart",IdCart);
                    command.Parameters.AddWithValue("@IdProductDetail", orders.IdProduct);
                    command.Parameters.AddWithValue("@Description", orders.Description);
                    command.Parameters.AddWithValue("@Number", orders.Number);
                    Random random = new Random();
                    if(orders.Number >= 5)
                    {
                        orders.ShipPrice = random.NextDouble() * (1.0 - 50.0) + 1.0;
                    }
                    else
                    {
                        orders.ShipPrice = random.NextDouble() * (1.0 - 100.0) + 1.0;
                    }
                    if(orders.ShipPrice <= 0)
                    {
                        orders.ShipPrice = - orders.ShipPrice;
                    }
                    command.Parameters.AddWithValue("@PriceShip", orders.ShipPrice);
                    command.Parameters.AddWithValue("@UsedStatus", 3);
                    command.Parameters.AddWithValue("@CreateBy", UserId);
                    command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
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

        public async Task<string> AddOrdersListAsync(string token, OrdersModel orders, string ListIdCart)
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
                    command.CommandText = "AddOrdersList";
                    Guid IdOrder = Guid.NewGuid();
                    command.Parameters.AddWithValue("@Id", IdOrder);
                    command.Parameters.AddWithValue("@IdUser", UserId);

                    command.Parameters.AddWithValue("@CartIds", ListIdCart);
                    command.Parameters.AddWithValue("@Description", orders.Description);
                    Random random = new Random();
                    if (orders.Number >= 5)
                    {
                        orders.ShipPrice = random.NextDouble() * (1.0 - 50.0) + 1.0;
                    }
                    else
                    {
                        orders.ShipPrice = random.NextDouble() * (1.0 - 100.0) + 1.0;
                    }
                    if (orders.ShipPrice <= 0)
                    {
                        orders.ShipPrice = -orders.ShipPrice;
                    }
                    command.Parameters.AddWithValue("@PriceShip", orders.ShipPrice);
                    command.Parameters.AddWithValue("@UsedStatus", 3);
                    command.Parameters.AddWithValue("@CreateBy", UserId);
                    command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
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

        public Task<string> DeleteOrdersListAsync(string ListIdCart)
        {
            try
            {
                using (var connect = _connectToSql.CreateConnection())
                {
                    SqlCommand command = new SqlCommand();
                    var command1 = connect.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "DeleteOrdersList";
                    command.Parameters.AddWithValue("@CartIds", ListIdCart);
                    command.Connection = (SqlConnection)connect;

                    // Add the @Result parameter for the stored procedure (output parameter).
                    SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.NVarChar, -1);
                    resultParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultParam);
                    connect.Open(); // Open the connection before executing the command.
                    command.ExecuteNonQuery();
                    return Task.FromResult(resultParam.Value.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(List<OrdersModelDisplay>, int)> GetAllOrderAsync (int pageIndex, int pageSize, string? search, string token)
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
                        return (new List<OrdersModelDisplay> { }, 0);
                    }
                    Guid UserId = Guid.Parse(userIdClaim.Value);
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", UserId);
                    parameters.Add("@pageIndex", pageIndex);
                    parameters.Add("@pageSize", pageSize);
                    if (search == null)
                    {
                        search = "";
                    }
                    parameters.Add("@search", search);
                    parameters.Add("@totalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var result = await connect.QueryAsync<OrdersModelDisplay>(
                        "GetALLOrder",
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
    }
}
