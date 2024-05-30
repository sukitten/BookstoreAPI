using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace YourNamespace
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
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

                    // Potentially use the environment variable here 
                    // or pass it to services as needed
                });
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Here, you will add services to the container.
            // For example, to add support for controllers:
            // services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // This is where you'd configure the HTTP request pipeline.
            
            // Example middleware usage:
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            // 
            // app.UseRouting();
            //
            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapControllers();
            // });
        }
    }
}