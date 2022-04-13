using Microsoft.AspNetCore.Authentication;

namespace SutekiTmp.Domain.Common.Authentication.Session
{
    public static class SessionAuthenticationExtensions
    {
        public static AuthenticationBuilder AddSessionAuthenticationnOptions(this AuthenticationBuilder builder, Action<SessionAuthenticationOptions> options)
        {
            return builder.AddScheme<SessionAuthenticationOptions,SessionAuthenticationHandler>(SessionAuthenticationOptions.Scheme, options);
        }
    }
}
