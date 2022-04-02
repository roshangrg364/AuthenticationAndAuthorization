﻿using BaseModule.ActivityManagement.Repo;
using BaseModule.ActivityManagement.Service;
using BaseModule.AuditManagement.Repository;
using BaseModule.Repository.Email;
using BaseModule.Repository.Inventory;
using BaseModule.Repository.User;
using EmailModule.Repository;
using EmailModule.Service;
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
            UseBase(services);
            UserEmailConfig(services);
        }

        private static void UseBase(IServiceCollection services)
        {
            services.AddTransient<ActivityLogRepositoryInterface, ActivityLogRepository>();
            services.AddTransient<ActivityLogServiceInterface, ActivityLogService>();
            services.AddTransient<AuditLogRepositoryInterface, AuditLogRepository>();

        }
        private static void UserEmailConfig(IServiceCollection services)
        {
            services.AddScoped<EmailSenderServiceInterface, EmailSenderService>();
            services.AddScoped<EmailTemplateServiceInterface, EmailTemplateService>();
            services.AddScoped<EmailTemplateRepositoryInterface, EmailTemplateRepository>();
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
            services.AddScoped<RoleRepositoryInterface, RoleRepository>();

        }
        private static void UserUserService(IServiceCollection services)
        {
            services.AddScoped<UserServiceInterface, UserService>();
            services.AddScoped<RoleServiceInterface, RoleService>();
        }
    }
}
