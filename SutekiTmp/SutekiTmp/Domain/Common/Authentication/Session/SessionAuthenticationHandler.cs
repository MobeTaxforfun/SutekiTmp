using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SutekiTmp.Domain.Common.Authentication.Session
{
    public class SessionAuthenticationHandler : IAuthenticationSignInHandler
    {
        public const string TEST_SCHEM_NAME = "some_authen";
        public SessionAuthenticationOptions Options { get; private set; }

        public SessionAuthenticationHandler(IOptions<SessionAuthenticationOptions> opt)
        {
            Options = opt.Value;
        }

        public HttpContext HttpContext { get; private set; }
        public AuthenticationScheme Scheme { get; private set; }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            if (Scheme.Name != TEST_SCHEM_NAME)
            {
                return Task.FromResult(AuthenticateResult.Fail("不是這個驗證實體"));
            }
            // 再看Session
            if (!HttpContext.Session.Keys.Contains(Options.SessionKeyName))
            {
                return Task.FromResult(AuthenticateResult.Fail("無效的Session"));
            }
            // 通過驗證
            string un = HttpContext.Session.GetString(Options.SessionKeyName) ?? string.Empty;
            ClaimsIdentity id = new(TEST_SCHEM_NAME);
            id.AddClaim(new(ClaimTypes.Name, un));
            ClaimsPrincipal prcp = new(id);
            AuthenticationTicket ticket = new(prcp, TEST_SCHEM_NAME);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        public Task ChallengeAsync(AuthenticationProperties? properties)
        {
            HttpContext.Response.Redirect($"{Options.LoginPath}?{Options.ReturnUrlKey}={HttpContext.Request.Path}");
            return Task.CompletedTask;
        }

        public async Task ForbidAsync(AuthenticationProperties? properties)
        {
            await HttpContext.ForbidAsync(Scheme.Name);
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            HttpContext = context;
            Scheme = scheme;
            return Task.CompletedTask;
        }

        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
        {
            string uname = user.Identity?.Name ?? string.Empty;
            if (!string.IsNullOrEmpty(uname))
            {
                HttpContext.Session.SetString(Options.SessionKeyName, uname);
            }
            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties? properties)
        {
            if (HttpContext.Session.Keys.Contains(Options.SessionKeyName))
            {
                HttpContext.Session.Remove(Options.SessionKeyName);
            }
            return Task.CompletedTask;
        }
    }
}
