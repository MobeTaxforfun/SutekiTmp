﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SutekiTmp.Domain.Common.Authentication;
using SutekiTmp.Domain.Common.Authentication.Session;
using SutekiTmp.Domain.Service.IService;
using SutekiTmp.Viewmodels.Login;
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

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginByCookieAuth(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var validUser = _LoginService.GetUser(new Viewmodels.Login.LoginViewModel
                {
                    UserName = model.UserName,
                    Password = model.Password
                });

                if (validUser != null)
                {

                    List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,model.UserName),
                    new Claim(ClaimTypes.Email,validUser.Email),
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
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginByCustomAuth(LoginViewModel model)
        {
            var validUser = _LoginService.GetUser(new Viewmodels.Login.LoginViewModel
            {
                UserName = model.UserName,
                Password = model.Password
            });

            if (validUser != null)
            {
                var claimIdentity = new ClaimsIdentity(CustomAuthenticationOptions.Scheme);
                claimIdentity.AddClaim(new Claim("UserId", validUser.UserId.ToString()));
                var properties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                await HttpContext.SignInAsync(CustomAuthenticationOptions.Scheme, new ClaimsPrincipal(claimIdentity));
                return View();
            }

            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> LoginBySession(LoginViewModel model)
        {
            var validUser = _LoginService.GetUser(new Viewmodels.Login.LoginViewModel
            {
                UserName = model.UserName,
                Password = model.Password
            });
            if (validUser != null)
            {
                //聲明
                Claim claimName = new(ClaimTypes.Name, validUser.UserName);
                Claim claimId = new("UserId", validUser.UserId.ToString());
                //標示
                ClaimsIdentity claimsIdentity = new(SessionAuthenticationOptions.Scheme);
                claimsIdentity.AddClaim(claimName);
                claimsIdentity.AddClaim(claimId);
                //驗證主體
                ClaimsPrincipal principal = new(claimsIdentity);
                await HttpContext.SignInAsync(SessionAuthenticationOptions.Scheme, principal);
                return View();
            }

            return Unauthorized();
        }
    }
}
