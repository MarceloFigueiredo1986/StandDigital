using StandDigital.Components;
using StandDigital.Repositories.Car;
using StandDigital.Repositories.CarMetadata;
using StandDigital.Repositories.CarPhotos;
using StandDigital.UnitsOfWork.Car;
using StandDigital_Shared.Persistence;

namespace StandDigital
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddScoped<IDatabase, Database>();
            builder.Services.AddScoped<ICarRepository, CarRepository>();
            builder.Services.AddScoped<ICarMetadataRepository, CarMetadataRepository>();
            builder.Services.AddScoped<IPhotosRepository, PhotosRepository>();
            builder.Services.AddScoped<ICarUoW, CarUoW>();

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

            app.Run();
        }
    }
}
