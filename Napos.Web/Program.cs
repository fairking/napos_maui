using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Napos.Data;
using Napos.Domain;
using Napos.Web.Filters;
using Napos.Web.Services;
using System;
using System.IO;

namespace Napos.Web
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers((opt) =>
            {
                opt.ModelMetadataDetailsProviders.Add(new ModelBindingMetadataProvider());
            });

            var connectionString = SetupDatabase();

            Bootstrap.SetupServices(builder.Services, connectionString);

            // Service Executor
            builder.Services.AddScoped((s) =>
            {
                return new ServiceExecutor(new[] { typeof(Bootstrap).Assembly });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{service}/{operation}/{id?}",
                defaults: new { controller = "Default", action = "Index" });

            app.MapFallbackToFile("index.html");

            app.UseCors((cfg) =>
            {
                cfg.AllowAnyHeader();
                cfg.AllowAnyMethod();
                cfg.AllowAnyOrigin();
            });

            app.Run();
        }

        private static string SetupDatabase()
        {
            var dbFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Napos");

            if (!Directory.Exists(dbFolder))
                Directory.CreateDirectory(dbFolder);

            var dbPath = Path.Combine(dbFolder, "napos.db3");

            var connectionString = $"Data Source={dbPath};";

            DataSetup.MigrateData(connectionString, true);

            return connectionString;
        }
    }
}
