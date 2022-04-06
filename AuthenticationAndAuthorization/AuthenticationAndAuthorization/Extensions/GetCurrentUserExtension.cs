using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using UserModule.Exceptions;

namespace AuthenticationAndAuthorization.Extensions
{
    public static class GetCurrentUserExtension
    {
       
            public static string GetCurrentUserId(this ControllerBase controller)
            {
                return controller.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            public static async Task<UserModule.Entity.User> GetCurrentUser(this ControllerBase controller)
            {
                var userId = controller.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                using var serviceScope = ServiceActivator.GetScope();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<UserModule.Entity.User>>();
                return await userManager.FindByIdAsync(userId).ConfigureAwait(true) ?? throw new UserNotFoundException();
            }
        

    }
}
