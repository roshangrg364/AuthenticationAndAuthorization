using System.Diagnostics;
using System.Threading.Tasks;
using AuthenticationAndAuthorization.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;

namespace AuthenticationAndAuthorization.Controllers
{
  
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IToastNotification _notify;
        public HomeController(ILogger<HomeController> logger,
            IToastNotification notify)
        {
            _logger = logger;
            _notify = notify;
        }

        public async Task<IActionResult> Index()
        {
            return View();
           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
