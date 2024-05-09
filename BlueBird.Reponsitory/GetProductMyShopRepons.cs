using BlueBird.DataConext.Models;
using BlueBird.DataContext.Models;
using BlueBird.Reponsitory.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable.DataContext.Data;

namespace BlueBird.Reponsitory
{
    public class GetProductMyShopRepons : IGetProductMyShopRepons
    {
        private readonly ConnectToSql _connectToSql;

        public GetProductMyShopRepons(ConnectToSql connectToSql) 
        {
            _connectToSql = connectToSql;
        }
        public async Task<List<ProductByUserModel>> GetProductByUserAsync(string token, string? search)
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
                        return new List<ProductByUserModel> { };
                    }
                    Guid UserId = Guid.Parse(userIdClaim.Value);
                    var parameters = new DynamicParameters();
                    parameters.Add("@IdUser", UserId);
                    search ??= "";
                    parameters.Add("@search", search);

                    var result = await connect.QueryAsync<ProductByUserModel>(
                        "GetAllProductMyShop",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<List<RevenueModel>> GetProductRevenueAsync(string token)
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
                        return Task.FromResult(new List<RevenueModel> { });
                    }
                    Guid UserId = Guid.Parse(userIdClaim.Value);
                    var parameters = new DynamicParameters();
                    parameters.Add("@IdUser", UserId);

                    var result = connect.Query<RevenueModel>("GetProductRevenue", parameters, commandType: CommandType.StoredProcedure);
                    return Task.FromResult(result.ToList());
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
