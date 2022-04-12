using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SutekiTmp.Domain.Common.Authentication;
using SutekiTmp.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace SutekiTmp.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult CookieAuthIndex()
        {
            return View();
        }


        [Authorize(AuthenticationSchemes = CustomAuthenticationOptions.Scheme)]
        public IActionResult CustomAuthIndex()
        {
            var test = User.Claims.Where(c => c.Type == ClaimTypes.Email);
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