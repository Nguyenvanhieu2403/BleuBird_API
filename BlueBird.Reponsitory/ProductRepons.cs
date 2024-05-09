using BlueBird.DataConext.Data;
using BlueBird.DataConext.Models;
using BlueBird.DataContext.Models;
using BlueBird.Reponsitory.Interface;
using Dapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable.DataContext.Data;

namespace BlueBird.Reponsitory
{
    public class ProductRepons : IProductRepons
    {
        private readonly ConnectToSql _connectToSql;

        public ProductRepons(ConnectToSql connectToSql) 
        {
            _connectToSql = connectToSql;
        }
        public async Task<string> AddProductAsync(ProductModel productModel, string token)
        {
            string result = null;
            try
            {
                using (var connect = _connectToSql.CreateConnection())
                {
                    connect.Open();
                    string jsonImages = JsonConvert.SerializeObject(productModel.Images);
                    string jsonProductDetails = JsonConvert.SerializeObject(productModel.Product_Detail);
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
                    command.CommandText = "AddProduct";
                    Guid IdProduct = Guid.NewGuid();
                    command.Parameters.AddWithValue("@Id", IdProduct);
                    command.Parameters.AddWithValue("@Name", productModel.Name);
                    command.Parameters.AddWithValue("@IdUser", UserId);
                    command.Parameters.AddWithValue("@IdProducer", productModel.IdProducer);
                    command.Parameters.AddWithValue("@IdProductType", productModel.IdProductType);
                    command.Parameters.AddWithValue("@Material", productModel.Material);
                    command.Parameters.AddWithValue("@Price", productModel.Price);
                    command.Parameters.AddWithValue("@Description", productModel.Description);
                    command.Parameters.AddWithValue("@IdDiscount", productModel.IdDiscount);
                    command.Parameters.AddWithValue("@CreateBy", UserId);
                    command.Parameters.AddWithValue("@CreateDate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("@ModifiedDate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("@Images", jsonImages);
                    command.Parameters.AddWithValue("@ProductDetails", jsonProductDetails);
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> DeleteProductAsync(Guid Id)
        {
            string result = string.Empty;
            try
            {
                using (var connect = _connectToSql.CreateConnection())
                {
                    SqlCommand command = new SqlCommand();
                    var command1 = connect.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "DeleteProduct";
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

        public Task<string> DeleteProductCencorshipManagementAsync(Guid Id)
        {
            try
            {
                using (var connect = _connectToSql.CreateConnection())
                {
                    SqlCommand command = new SqlCommand();
                    var command1 = connect.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "DeleteProductCencorshipManagement";
                    command.Parameters.AddWithValue("@Id", Id);
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

        public async Task<(List<Product>,int)> FindProductAsync(string? search, int pageIndex, int pageSize, string ProductType)
        {
            try
            {
                int TotalRecord = 0;
                List<Product> products = new List<Product>();
                using (var connect = (SqlConnection)_connectToSql.CreateConnection())
                {
                    connect.Open();
                    var command = new SqlCommand("GetProducerBySearch", connect)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    // Thêm tham số phân trang
                    search ??= "";
                    command.Parameters.AddWithValue("@search", search);
                    command.Parameters.AddWithValue("@pageIndex", pageIndex);
                    command.Parameters.AddWithValue("@pageSize", pageSize);
                    command.Parameters.AddWithValue("@productType", ProductType);

                    using SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            Id = (Guid)reader["Id"],
                            Name = reader["Name"].ToString(),
                            IdProducer = (Guid)reader["IdProducer"],
                            IdProductType = (Guid)reader["IdProductType"],
                            Price = (Double)reader["Price"],
                            Material = reader["Material"].ToString(),
                            Description = reader["Description"].ToString(),
                            IdDiscount = (Guid)reader["IdDiscount"],
                        };
                        product.Images = JsonConvert.DeserializeObject<List<IMG>>((reader["img"]).ToString());

                        // Phân tích chuỗi JSON thành danh sách Product_Detail
                        var productDetailJson = reader["product_Detail"].ToString();
                        product.Products = JsonConvert.DeserializeObject<List<Product_Detail>>(productDetailJson);

                        products.Add(product);
                        TotalRecord = (int)reader["TotalRecord"];
                    }
                }
                return (products,TotalRecord);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<List<CencorshipManagementModel>> GetCencorshipManagementModelAsync(string? search)
        {
            try
            {
                using (var connect = _connectToSql.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    search ??= "string";
                    parameters.Add("@search", search);
                    var result = connect.Query<CencorshipManagementModel>("CencorshipManagement", parameters, commandType: CommandType.StoredProcedure);
                    return Task.FromResult(result.ToList());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<ProductDetail> GetProductByIdsAsync(Guid Id)
        {
            try
            {
                int TotalRecord = 0;
                ProductDetail product = new ProductDetail();
                using (var connect = (SqlConnection)_connectToSql.CreateConnection())
                {
                    connect.Open();
                    SqlCommand command = new SqlCommand("GetProductById", connect);
                    command.CommandType = CommandType.StoredProcedure;

                    // Thêm tham số phân trang
                    command.Parameters.AddWithValue("@Id", Id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            product = new ProductDetail
                            {
                                Id = (Guid)reader["Id"],
                                Name = reader["Name"].ToString(),
                                IdProducer = (Guid)reader["IdProducer"],
                                IdProductType = (Guid)reader["IdProductType"],
                                Price = (Double)reader["Price"],
                                Material = reader["Material"].ToString(),
                                Description = reader["Description"].ToString(),
                                IdDiscount = (Guid)reader["IdDiscount"],
                                IdShop = (Guid)reader["IdShop"],
                                NameDiscount = reader["NameDiscount"].ToString(),
                                BrandName = reader["BrandName"].ToString(),
                                avataShop = reader["avataShop"].ToString(),
                                TotalProduct = (int)reader["TotalProduct"],
                            };
                            product.Images = JsonConvert.DeserializeObject<List<IMG>>((reader["img"]).ToString());

                            // Phân tích chuỗi JSON thành danh sách Product_Detail
                            var productDetailJson = reader["product_Detail"].ToString();
                            product.Products = JsonConvert.DeserializeObject<List<Product_Detail>>(productDetailJson);

                        }
                    }
                }
                return Task.FromResult(product);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(List<Product>, int)> GetProductsAsync(int pageIndex, int pageSize, string ProductType)
        {
            try
            {
                int TotalRecord = 0;
                List<Product> products = new List<Product>();
                using (var connect = (SqlConnection)_connectToSql.CreateConnection())
                {
                    connect.Open();
                    SqlCommand command = new SqlCommand("GetAllProduct", connect);
                    command.CommandType = CommandType.StoredProcedure;

                     // Thêm tham số phân trang
                    command.Parameters.AddWithValue("@pageIndex", pageIndex);
                    command.Parameters.AddWithValue("@pageSize", pageSize);
                    command.Parameters.AddWithValue("@productType", ProductType);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                Id = (Guid)reader["Id"],
                                Name = reader["Name"].ToString(),
                                IdProducer = (Guid)reader["IdProducer"],
                                IdProductType = (Guid)reader["IdProductType"],
                                Price = (Double)reader["Price"],
                                Material = reader["Material"].ToString(),
                                Description = reader["Description"].ToString(),
                                IdDiscount = (Guid)reader["IdDiscount"],
                            };
                            product.Images = JsonConvert.DeserializeObject<List<IMG>>((reader["img"]).ToString());
                            
                            // Phân tích chuỗi JSON thành danh sách Product_Detail
                            var productDetailJson = reader["product_Detail"].ToString();
                            product.Products = JsonConvert.DeserializeObject<List<Product_Detail>>(productDetailJson);
                            
                            products.Add(product);
                            TotalRecord = (int)reader["TotalRecord"];
                        }
                    }
                }
                return (products, TotalRecord);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<string> UpdateCencorshipManagementModelAsync(Guid IdProduct, string token)
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
                    return Task.FromResult("Token không hợp lệ");
                }
                Guid UserId = Guid.Parse(userIdClaim.Value);
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "UpdateCencorshipManagementModel";
                command.Parameters.AddWithValue("@IdProduct", IdProduct);
                command.Parameters.AddWithValue("@IdUser", UserId);
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

        public async Task<string> UpdateProductAsync(Guid Id, ProductModel productModel, string token)
        {
            string result = null;
            try
            {
                using (var connect = _connectToSql.CreateConnection())
                {
                    connect.Open();
                    string jsonImages = JsonConvert.SerializeObject(productModel.Images.Select(img => new { Img = img.img }));
                    string jsonProductDetails = JsonConvert.SerializeObject(productModel.Product_Detail);
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
                    command.CommandText = "UpdateProduct";
                    Guid IdProduct = Guid.NewGuid();
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@Name", productModel.Name);
                    command.Parameters.AddWithValue("@IdUser", UserId);
                    command.Parameters.AddWithValue("@IdProducer", productModel.IdProducer);
                    command.Parameters.AddWithValue("@IdProductType", productModel.IdProductType);
                    command.Parameters.AddWithValue("@Material", productModel.Material);
                    command.Parameters.AddWithValue("@Price", productModel.Price);
                    command.Parameters.AddWithValue("@Description", productModel.Description);
                    command.Parameters.AddWithValue("@IdDiscount", productModel.IdDiscount);
                    command.Parameters.AddWithValue("@CreateBy", UserId);
                    command.Parameters.AddWithValue("@CreateDate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("@ModifiedDate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("@Images", jsonImages);
                    command.Parameters.AddWithValue("@ProductDetails", jsonProductDetails);
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
        private async void ImportImg(List<IFormFile> files, Guid IdUser, Guid IdProduct)
        {
            long size = files.Sum(f => f.Length);
            var tenfile = "";
            int i = 1;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    if (!System.IO.Directory.Exists("IMG"))
                        System.IO.Directory.CreateDirectory("IMG");
                    
                    // Tạo tên tệp ngẫu nhiên cho tệp ảnh
                    var fileName = formFile.FileName;
                    tenfile = "~/uploads/" + formFile.FileName; ;
                    if (!System.IO.Directory.Exists(Environment.CurrentDirectory + "\\IMG\\" + IdUser +"_" + IdProduct))
                        System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + "\\IMG\\" + IdUser + "_" + IdProduct);
                    // Lấy đường dẫn thư mục lưu trữ từ cấu hình
                    var storedFilesPath = Environment.CurrentDirectory + "\\IMG\\" + IdUser + "_" + IdProduct+ "\\";
                    // Kết hợp đường dẫn đầy đủ cho tệp ảnh
                    var filePath = Path.Combine(storedFilesPath, fileName);

                    // Lưu trữ tệp ảnh
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
                i++;
            }
        }

    }
}
