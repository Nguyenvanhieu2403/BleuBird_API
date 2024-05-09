using BlueBird.Reponsitory;
using BlueBird.Reponsitory.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BlueBird.Respository.Configs
{
    public static class InjectionRepositoryExtension 
    {
        public static void DependencyInjectionRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepons, UserRepons>();
            services.AddScoped<IProducerRepons, ProducerRepons>();
            services.AddScoped<IProductRepons, ProductRepons>();
            services.AddScoped<IProductTypeRepons, ProductTypeRepons>();
            services.AddScoped<ICartRepons, CartRepons>();
            services.AddScoped<IOrdersRepons, OrdersRepons>();
            services.AddScoped<IChangePassRepons, ChangePassRepons>();
            services.AddScoped<IChangeAddressRepons, ChangeAddressRepons>();
            services.AddScoped<IGetProductMyShopRepons, GetProductMyShopRepons>();
            services.AddScoped<IFeedBackRepons,  FeedBackRepons>();
            services.AddScoped<IShopRepons, ShopRepons>();
        }
    }
}
