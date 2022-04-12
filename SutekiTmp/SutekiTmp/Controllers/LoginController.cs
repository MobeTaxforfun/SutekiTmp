using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SutekiTmp.Domain.Common.Authentication;
using SutekiTmp.Domain.Service.IService;
using System.Security.Claims;

namespace SutekiTmp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _LoginService;

        public LoginController(ILoginService loginService)
        {
            _LoginService = loginService;
        }

        public IActionResult LoginByJWT()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginByJWT(string UserName, string Password)
        {
            return View();
        }

        public IActionResult LoginByCookieAuth()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginByCookieAuth(string UserName, string Password)
        {
            var validUser = _LoginService.GetUser(new Viewmodels.Login.LoginViewModel
            {
                UserName = UserName,
                Password = Password
            });

            if (validUser != null)
            {

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,UserName),
                    new Claim("","")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var properties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                return View();
            }

            return Unauthorized();
        }

        public IActionResult LoginByCustomAuth()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginByCustomAuth(string UserName, string Password)
        {
            var validUser = _LoginService.GetUser(new Viewmodels.Login.LoginViewModel
            {
                UserName = UserName,
                Password = Password
            });

            if (validUser != null)
            {
                var claimIdentity = new ClaimsIdentity(CustomAuthenticationOptions.Scheme);
                claimIdentity.AddClaim(new Claim(ClaimTypes.Name, "jim"));
                var properties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                await HttpContext.SignInAsync(CustomAuthenticationOptions.Scheme, new ClaimsPrincipal(claimIdentity));
                return View();
            }

            return Unauthorized();
        }
    }
}
