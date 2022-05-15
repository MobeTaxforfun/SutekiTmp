using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SutekiTmp.Domain.Common.Attributes;
using SutekiTmp.Domain.Common.Authentication;
using SutekiTmp.Domain.Common.Authentication.Session;
using SutekiTmp.Domain.Service.IService;
using SutekiTmp.Viewmodels.Login;
using System.Security.Claims;

namespace SutekiTmp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService loginService;

        public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateModel]
        public IActionResult LoginByCookieAuth(LoginViewModel model)
        {

            var CurrentUser = loginService.GetUserByUserNameAndPassWord(model.UserName, model.Password);
            if (CurrentUser != null)
            {

                List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name,CurrentUser.Name),
                        new Claim(ClaimTypes.Email,CurrentUser.Email),
                        new Claim(ClaimTypes.MobilePhone,CurrentUser.Phone),
                        new Claim("UserName",CurrentUser.UserName),
                        new Claim("UserId",CurrentUser.UserId.ToString())
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var properties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                return View();
            }
            else
            {
                //return Unauthorized();
                this.ModelState.AddModelError("", "帳號密碼錯誤");
                return View("Index", model);
            }
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> LoginByCustomAuth(LoginViewModel model)
        {
            var CurrentUser = loginService.GetUserByUserNameAndPassWord(model.UserName, model.Password);

            if (CurrentUser != null)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,CurrentUser.Name),
                    new Claim(ClaimTypes.Email,CurrentUser.Email),
                    new Claim(ClaimTypes.MobilePhone,CurrentUser.Phone),
                    new Claim("UserName",CurrentUser.UserName),
                    new Claim("UserId",CurrentUser.UserId.ToString())
                };

                var claimIdentity = new ClaimsIdentity(claims, CustomAuthenticationOptions.Scheme);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                await HttpContext.SignInAsync(CustomAuthenticationOptions.Scheme, new ClaimsPrincipal(claimIdentity), properties);
                return RedirectToAction("Index", "Home");
            }

            return Unauthorized();
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> LoginBySession(LoginViewModel model)
        {
            var CurrentUser = loginService.GetUserByUserNameAndPassWord(model.UserName, model.Password);
            if (CurrentUser != null)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,CurrentUser.Name),
                    new Claim(ClaimTypes.Email,CurrentUser.Email),
                    new Claim(ClaimTypes.MobilePhone,CurrentUser.Phone),
                    new Claim("UserName",CurrentUser.UserName),
                    new Claim("UserId",CurrentUser.UserId.ToString(),ClaimValueTypes.Integer32)
                };

                var claimIdentity = new ClaimsIdentity(claims, SessionAuthenticationOptions.Scheme);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                await HttpContext.SignInAsync(SessionAuthenticationOptions.Scheme, new ClaimsPrincipal(claimIdentity), properties);
                return RedirectToAction("Index", "Home");
            }

            return Unauthorized();
        }
    }
}
