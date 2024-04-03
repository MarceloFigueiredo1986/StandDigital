using Blazored.Modal;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using StandDigital.Bootstrapping;
using StandDigital.Components;
using StandDigital.Core;

namespace StandDigital;


public class Program
{

    public static void Main(string[] args)
    {
        Log.Logger = StandDigital_Shared.Bootstrapping.Installer.InstallSerilogSinksConfiguration();

        try
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services
                .AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.InstallCoreDependencies();
            builder.Services.AddBlazoredModal();
            builder.Configuration.InitialValidations();
                     
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Login";
            });


            builder.Services.InitializeSettings(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseWebAssemblyDebugging();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAntiforgery();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal($"{ex}");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
