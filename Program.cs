using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace YourNamespace
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error starting the application: {ex.Message}");
                // Here we might log to a file or other logging infrastructure
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => 
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((context, services) => 
                {
                    // Read environment variable
                    var myEnvVariable = Environment.GetEnvironmentVariable("MY_ENV_VARIABLE");

                    // Configuring a service to use the environment variable or other configuration based on it
                })
                .ConfigureLogging(logging => 
                {
                    logging.ClearProviders(); // Clears all the default logging providers.
                    logging.AddConsole(); // Adds a console logger.
                });
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500; 
                        await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
                        logger.LogError("An unexpected error occurred.");
                    });
                });
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}