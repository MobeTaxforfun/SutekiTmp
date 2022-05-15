using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using SutekiTmp.Domain.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SutekiTmp.Domain.Common.Authentication.Session
{
    public class SessionAuthenticationHandler : AuthenticationHandler<SessionAuthenticationOptions>, IAuthenticationSignInHandler, IAuthenticationHandler
    {
        private readonly IUserRepository userRepository;

        public SessionAuthenticationHandler(
            IOptionsMonitor<SessionAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserRepository userRepository
            ) : base(options, logger, encoder, clock)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// 驗證邏輯
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Context.Session.Keys.Contains(Options.SessionKeyName))
            {
                return AuthenticateResult.Fail("無效的Session");
            }

            string UserName = Context.Session.GetString(Options.SessionKeyName) ?? string.Empty;
            string UserSessionId = Context.Session.GetString(Options.SessionKeyId) ?? string.Empty;

            if (!Int32.TryParse(UserSessionId, out int UserId))
            {
                return AuthenticateResult.Fail("尚未初始化的UserSessionId");
            }


            var CurrentUser = userRepository.GetUserById(UserId);
            if (CurrentUser == null)
            {
                return AuthenticateResult.Fail("找不到這個使用者");
            }


            List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name,CurrentUser.Name),
                        new Claim(ClaimTypes.Email,CurrentUser.Email),
                        new Claim(ClaimTypes.MobilePhone,CurrentUser.Phone),
                        new Claim("UserName",CurrentUser.UserName),
                        new Claim("UserId",CurrentUser.UserId.ToString(),ClaimValueTypes.Integer32),
                        new Claim(ClaimTypes.Role,CurrentUser.Roles.FirstOrDefault().RoleId.ToString(),ClaimValueTypes.Integer32)
                    };
            ClaimsIdentity claimsIdentity = new(claims, SessionAuthenticationOptions.Scheme);


            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
            AuthenticationTicket ticket = new(claimsPrincipal, SessionAuthenticationOptions.Scheme);
            return AuthenticateResult.Success(ticket);
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="user"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
        {
            string UserName = user.Identity?.Name ?? string.Empty;
            string UserId = user.Claims.First(c => c.Type == "UserId").Value;
            if (!string.IsNullOrEmpty(UserName))
            {
                Context.Session.SetString(Options.SessionKeyName, UserName);
            }
            if (!string.IsNullOrEmpty(UserId))
            {
                Context.Session.SetString(Options.SessionKeyId, UserId);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task SignOutAsync(AuthenticationProperties? properties)
        {
            if (Context.Session.Keys.Contains(Options.SessionKeyName))
            {
                Context.Session.Remove(Options.SessionKeyName);
            }
            return Task.CompletedTask;
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            Context.Response.Redirect("/login");
            return Task.CompletedTask;
        }

    }
}
