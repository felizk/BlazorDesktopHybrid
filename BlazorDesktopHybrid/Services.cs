using BlazorDesktopHybrid.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorDesktopHybrid;

public static class Services
{
    public static void AddSharedServices(this IServiceCollection services)
    {
        services.AddScoped<ExampleUserService>();
        services.AddScoped<ExampleAuthenticationStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<ExampleAuthenticationStateProvider>());
        services.AddAuthorizationCore();
        services.AddCascadingAuthenticationState();
        services.AddHttpClient();
        services.AddFluentUIComponents();
    }
}