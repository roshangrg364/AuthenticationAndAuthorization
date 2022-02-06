
using BaseModule.DbContextConfig;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserModule.Entity;

namespace InventorySystemMysql
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseLazyLoadingProxies().UseMySQL(Configuration.GetConnectionString("Default"), b => b.MigrationsAssembly("InventorySystemMysql"));
            });
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<MyDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            }
            );
            services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
            {
                ProgressBar = true,
                TimeOut = 1500,
                PositionClass = ToastPositions.TopRight 
            })
             .AddMvcOptions(options =>
             {
                 var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                 options.Filters.Add(new AuthorizeFilter(policy));
             })
            .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null) ;

            services.UseInventoryDiConfig();

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                }
            );

           services.Configure<CookieTempDataProviderOptions>(options => { options.Cookie.IsEssential = true; });
            services.ConfigureApplicationCookie(options =>
             {
                 // Cookie settings
                 options.Cookie.HttpOnly = true;
                 options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                 options.Cookie.SameSite = SameSiteMode.Strict;
                 options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                 options.Events.OnRedirectToLogin = (evnt) =>
                     {
                         var returnUrl = evnt.Request.Path;
                         if (string.IsNullOrWhiteSpace(returnUrl) || returnUrl == "/") returnUrl = "/Home/Index";
                         evnt.Response.Redirect("/Account/Login?ReturnUrl=" + returnUrl);
                         return Task.CompletedTask;
                     };
                 options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                 options.SlidingExpiration = true;
             });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization(); 
            app.UseNToastNotify();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
               name: "MyArea",
              pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}