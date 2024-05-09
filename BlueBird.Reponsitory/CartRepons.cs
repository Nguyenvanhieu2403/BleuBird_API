using BlueBird.DataConext.Data;
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
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using TimeTable.DataContext.Data;

namespace BlueBird.Reponsitory
{
    public class CartRepons : ICartRepons
    {
        private readonly ConnectToSql _connectToSql;

        public CartRepons(ConnectToSql connectToSql) 
        {
            _connectToSql = connectToSql;
        }
        public async Task<string> AddCartAsync(List<CartModel> models, string token)
        {
            try
            {
                string result = string.Empty;
                using (var connect = (SqlConnection)_connectToSql.CreateConnection())
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var decodedToken = tokenHandler.ReadJwtToken(token);
                    var userIdClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "UserId");
                    if (userIdClaim == null)
                    {
                        return "Token không hợp lệ";
                    }
                    Guid UserId = Guid.Parse(userIdClaim.Value);

                    connect.Open();

                    // Tạo DataTable từ danh sách CartModel
                    DataTable cartDataTable = new DataTable();
                    cartDataTable.Columns.Add("Id", typeof(Guid));
                    cartDataTable.Columns.Add("IdUser", typeof(Guid));
                    cartDataTable.Columns.Add("IdProduct", typeof(Guid));
                    cartDataTable.Columns.Add("Number", typeof(int));
                    foreach (var model in models)
                    {
                        cartDataTable.Rows.Add(Guid.NewGuid(), UserId, model.IdProduct, model.Number);
                    }

                    // Tạo và thiết lập tham số TVP
                    SqlCommand command = new SqlCommand("AddCart", connect);
                    command.CommandType = CommandType.StoredProcedure;

                    // Gọi stored procedure AddCart
                    SqlParameter tvpParam = command.Parameters.AddWithValue("@CartItems", cartDataTable);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "CartModelTVP";
                    SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.NVarChar, -1);
                    resultParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resultParam);
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
        public async Task<string> DeleteCartAsync(Guid Id)
        {
            string result = string.Empty;
            try
            {
                using(var connect = _connectToSql.CreateConnection())
                {
                    SqlCommand command = new SqlCommand();
                    var command1 = connect.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "DeleteCart";
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
            catch( Exception  ex )
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(List<CartDispay>, int)> GetAllCartAsync(int pageIndex, int pageSize, string token, string? search )
        {
            try
            {
                using(var connect = _connectToSql.CreateConnection())
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var decodedToken = tokenHandler.ReadJwtToken(token);
                    var userIdClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "UserId");
                    if (userIdClaim == null)
                    {
                        return (new List<CartDispay>(), 0);
                    }
                    Guid UserId = Guid.Parse(userIdClaim.Value);
                    var parameters = new DynamicParameters();
                    parameters.Add("@IdUser", UserId);
                    parameters.Add("@pageIndex", pageIndex);
                    parameters.Add("@pageSize", pageSize);
                    if(search == null)
                    {
                        search = "";
                    }
                    parameters.Add("@search", search);
                    parameters.Add("@totalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var result = await connect.QueryAsync<CartDispay>(
                        "GetAllCart",
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

        public async Task<string> UpdateCartAsync(Guid Id, CartModel model)
        {
            string result = string.Empty;
            try
            {
                using (var connect = _connectToSql.CreateConnection())
                {
                    connect.Open();
                    
                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "UpdateCart";
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@IDProduct", model.IdProduct);
                    command.Parameters.AddWithValue("@Number", model.Number);
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
            catch( Exception ex ) 
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
