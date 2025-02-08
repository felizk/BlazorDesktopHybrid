using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace BlazorDesktopHybrid.Web;

public static class AuthenticationWorkaroundExtensions
{
    public static void AddAuthenticationWorkaround(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "CustomScheme";
                options.DefaultChallengeScheme    = "CustomScheme";
            })
            .AddScheme<CustomAuthOptions, CustomAuthenticationHandler>("CustomScheme", options => { });
    }
    
    private class CustomAuthenticationHandler(IOptionsMonitor<CustomAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder)
        : AuthenticationHandler<CustomAuthOptions>(options, logger, encoder) 
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new GenericIdentity("CustomIdentity")), "CustomAuthenticationScheme")));
        }
    }

    private class CustomAuthOptions : AuthenticationSchemeOptions
    {
        // Add any custom options here if needed in the future
    }
}
