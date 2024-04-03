using Microsoft.AspNetCore.Authentication.Cookies;
using StandDigital.Bootstrapping;

namespace StandDigital.Bootstrapping;

public static class DependencyInjectionAuthentication
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.
            AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
            AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = configuration["Authentication:Google:ClientId"] ?? throw new Exception("Fill Google ClientID");
                googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"] ?? throw new Exception("Fill Google ClientSecret");
            });

        return services;
    }
}
