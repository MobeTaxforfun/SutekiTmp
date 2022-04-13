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
            var UserId = user.Claims.First(c=>c.Type == "UserId").Value;
            Context.Session.SetString("IsAuth", "true");

            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties? properties)
        {
            Context.Response.Cookies.Delete("myCookie");
            return Task.CompletedTask;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            var IsAuth = Context.Session.GetString("IsAuth");

            var claimIdentity = new ClaimsIdentity(CustomAuthenticationOptions.Scheme);
            claimIdentity.AddClaim(new Claim(ClaimTypes.Email, "mobe@gmail.com"));
            var principal = new ClaimsPrincipal(claimIdentity);
            var ticket = new AuthenticationTicket(principal, CustomAuthenticationOptions.Scheme);

            var endpoint = Context.Features.Get<IEndpointFeature>()?.Endpoint;

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
