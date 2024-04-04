using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using StandDigital.Bootstrapping;

namespace StandDigital.Bootstrapping;

public static class DependencyInjectionAuthentication
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.
            AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).
            AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = configuration["Authentication:Google:ClientId"] ?? throw new Exception("Fill Google ClientID");
                googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"] ?? throw new Exception("Fill Google ClientSecret");
            }).
            AddIdentityCookies();

        Ver neste vídeo como fazer
            https://www.youtube.com/watch?v=r4shd_aC3T4

        return services;
    }
}
