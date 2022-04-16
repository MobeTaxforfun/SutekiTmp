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
        private readonly IUserRoleRepository _userRoleRepository;
        public SessionAuthenticationHandler(
            IOptionsMonitor<SessionAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserRoleRepository userRoleRepository
            ) : base(options, logger, encoder, clock)
        {
            _userRoleRepository = userRoleRepository;
        }

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

            var RoleId = _userRoleRepository.GetRoleIdByUserId(UserId);

            ClaimsIdentity claimsIdentity = new(SessionAuthenticationOptions.Scheme);
            claimsIdentity.AddClaim(new(ClaimTypes.Name, UserName));
            claimsIdentity.AddClaim(new("UserId", UserSessionId));
            claimsIdentity.AddClaim(new("RoleId", RoleId.ToString()));
            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
            AuthenticationTicket ticket = new(claimsPrincipal, SessionAuthenticationOptions.Scheme);
            return AuthenticateResult.Success(ticket);
        }

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

        public Task SignOutAsync(AuthenticationProperties? properties)
        {
            if (Context.Session.Keys.Contains(Options.SessionKeyName))
            {
                Context.Session.Remove(Options.SessionKeyName);
            }
            return Task.CompletedTask;
        }
    }
}
