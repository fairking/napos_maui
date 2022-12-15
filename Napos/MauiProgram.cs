using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Storage;
using Napos.Core.Exceptions;
using Napos.Data;
using Napos.Domain;
using Napos.Models;
using Napos.Services;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Napos
{
    public static class MauiProgram
    {
        public static IServiceProvider Services;

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddBlazorWebViewDeveloperTools();

            var connectionString = SetupDatabase();

            Bootstrap.SetupServices(builder.Services, connectionString, dataContextLifetime: ServiceLifetime.Singleton);

            builder.Services.AddSingleton((s) => new ServiceExecutor(new[] { typeof(Bootstrap).Assembly }));

            var app = builder.Build();

            Services = app.Services;

            return app;
        }

        private static string SetupDatabase()
        {
            var dbPath = Path.Combine(FileSystem.Current.AppDataDirectory, "napos.db3");

            var connectionString = $"Data Source={dbPath};";

            DataSetup.MigrateData(connectionString, false);

            return connectionString;
        }

        public static void OnExit()
        {
            _cancelTokenSrc.Dispose();
        }

        private static CancellationTokenSource _cancelTokenSrc = new CancellationTokenSource();

        [JSInvokable]
        public static async Task<object> CallMeFromJs(JsRequest request)
        {
            var executor = Services.GetService<ServiceExecutor>();

            object? result = null;
            using (var scope = Services.CreateScope())
            {
                try
                {
                    var serviceResult = await executor.ExecuteService(request, scope.ServiceProvider, _cancelTokenSrc.Token);
                    result = new { data = serviceResult };
                }
                catch (UserException exc)
                {
                    result = new { error = new { paths = exc.Properties, messages = exc.Messages } };
                }
                catch (Exception exc)
                {
                    // TODO: Add logger here
                    result = new { error = "Server error occured." };
                }
            }

            return result;
        }
    }
}