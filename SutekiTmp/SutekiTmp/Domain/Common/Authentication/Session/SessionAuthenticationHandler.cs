using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
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
        public SessionAuthenticationHandler(
            IOptionsMonitor<SessionAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {

        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Context.Session.Keys.Contains(Options.SessionKeyName))
            {
                return AuthenticateResult.Fail("無效的Session");
            }

            string UserName = Context.Session.GetString(Options.SessionKeyName) ?? string.Empty;
            ClaimsIdentity claimsIdentity = new(SessionAuthenticationOptions.Scheme);
            claimsIdentity.AddClaim(new(ClaimTypes.Name, UserName));
            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
            AuthenticationTicket ticket = new(claimsPrincipal, SessionAuthenticationOptions.Scheme);
            return AuthenticateResult.Success(ticket);
        }

        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
        {
            string uname = user.Identity?.Name ?? string.Empty;
            if (!string.IsNullOrEmpty(uname))
            {
                Context.Session.SetString(Options.SessionKeyName, uname);
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
