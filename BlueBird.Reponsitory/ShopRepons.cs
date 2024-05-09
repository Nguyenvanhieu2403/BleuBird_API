using BlueBird.DataContext.Models;
using BlueBird.Reponsitory.Interface;
using Dapper;
using Newtonsoft.Json.Linq;
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
    public class ShopRepons : IShopRepons
    {
        private readonly ConnectToSql _connectToSql;

        public ShopRepons(ConnectToSql connectToSql) 
        {
            _connectToSql = connectToSql;
        }

        public Task<List<ManagerShop>> GetManagerShopAsync(string? search)
        {
            try
            {
                using var connect = _connectToSql.CreateConnection();
                var parameters = new DynamicParameters();
                search ??= "string";
                parameters.Add("@search", search);
                var result = connect.Query<ManagerShop>("ManagerShop", parameters, commandType: CommandType.StoredProcedure);
                return Task.FromResult(result.ToList());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ShopModel>> GetProductByUserAsync(Guid IdProduct)
        {
            try
            {
                using (var connect = _connectToSql.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@IdProduct", IdProduct);
                    var result = await connect.QueryAsync<ShopModel>(
                        "GetShop",
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
    }
}
