using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace SutekiTmp.Domain.Common.Authentication
{
    public class CustomAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string Scheme = "MobeAuth";
        public PathString LoginPath { get; set; }
        public PathString LogoutPath { get; set; }
        public PathString AccessDeniedPath { get; set; }
    }

    public class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthenticationOptions>, IAuthenticationHandler,IAuthenticationSignInHandler
    {
        public CustomAuthenticationHandler(IOptionsMonitor<CustomAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
        {

        }

        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
        {
            var ticket = new AuthenticationTicket(user, properties, Scheme.Name);

            //登入的邏輯

            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties? properties)
        {
            //登出的邏輯

            return Task.CompletedTask;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            var claimIdentity = new ClaimsIdentity(CustomAuthenticationOptions.Scheme);
            var principal = new ClaimsPrincipal(claimIdentity);
            var ticket = new AuthenticationTicket(principal, CustomAuthenticationOptions.Scheme);

            //驗證的邏輯

            return AuthenticateResult.Success(ticket);
        }
    }

    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddCustomAuthenticationOptions(this AuthenticationBuilder authenticationBuilder, Action<CustomAuthenticationOptions> options)
        {
            return authenticationBuilder.AddScheme<CustomAuthenticationOptions, CustomAuthenticationHandler>(CustomAuthenticationOptions.Scheme, options);
        }
    }

}
