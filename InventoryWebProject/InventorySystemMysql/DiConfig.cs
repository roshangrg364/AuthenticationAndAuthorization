using BaseModule.Repository.Inventory;
using BaseModule.Repository.User;
using InventoryModule.Repository;
using InventoryModule.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserModule.Repository;
using UserModule.Service;

namespace InventorySystemMysql
{
    public static class DiConfig
    {
        public static void UseInventoryDiConfig(this IServiceCollection services)
        {
            UseInventoryRepo(services);
            UseInventoryServices(services);
            UserUserRepo(services);
            UserUserService(services);
        }
        private static void UseInventoryRepo(IServiceCollection services)
        {
            services.AddScoped<CategoryRepositoryInterface, CategoryRepository>();
           
        }
        private static void UseInventoryServices(IServiceCollection services)
        {
            services.AddScoped<CategoryServiceInterface, CategoryService>();
        }
        private static void UserUserRepo(IServiceCollection services)
        {
            services.AddScoped<UserRepositoryInterface, UserRepository>();

        }
        private static void UserUserService(IServiceCollection services)
        {
            services.AddScoped<UserServiceInterface, UserService>();
        }
    }
}
