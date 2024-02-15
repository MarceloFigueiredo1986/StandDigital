using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using StandDigital.Components;
using StandDigital.HostedServices;
using StandDigital.Services.Repositories.Car;
using StandDigital.Services.Repositories.CarExtras;
using StandDigital.Services.Repositories.CarMetadata;
using StandDigital.Services.Repositories.CarPhotos;
using StandDigital.Services.UnitsOfWork.Car;
using StandDigital_HttpClient;
using StandDigital_Shared.Bootstrapping;
using StandDigital_Shared.Persistence;

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
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddScoped<IDatabase, Database>();
            builder.Services.AddScoped<ICarRepository, CarRepository>();
            builder.Services.AddScoped<ICarMetadataRepository, CarMetadataRepository>();
            builder.Services.AddScoped<IPhotosRepository, PhotosRepository>();
            builder.Services.AddScoped<ICarExtrasRepository, CarExtrasRepository>();
            builder.Services.AddScoped<ICarUoW, CarUoW>();
            builder.Services.InstallHttpClient();
            builder.Services.InstallSharedProject();
            builder.Services.AddHostedService<DatabaseUpdate>();

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

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            Log.Logger.Information("--------------------App started");

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
