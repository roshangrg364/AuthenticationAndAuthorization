using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthenticationAndAuthorization.CustomTokenProvider
{
    public class CustomEmailTokenProvider<T>: DataProtectorTokenProvider<T> where T:class
    {
        private readonly ILogger<CustomEmailTokenProvider<T>> _logger;
        public CustomEmailTokenProvider(IDataProtectionProvider provider, IOptions<CustomEmailTokenProviderOptions> options, ILogger<CustomEmailTokenProvider<T>>logger)
            :base(provider,options,logger)
        {
            _logger = logger;
        }
    }
}
