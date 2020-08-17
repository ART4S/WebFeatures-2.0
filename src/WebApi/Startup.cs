using Application;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using VueCliMiddleware;
using WebApi.Configuration;
using WebApi.ModelBinders;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterApplication();

            services.RegisterBackgroundJobs(Configuration);
            services.RegisterCommonServices();
            services.RegisterDataAccess(Configuration);
            services.RegisterLogging();
            services.RegisterSecurity();

            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new FileModelBinderProvider());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "WebFeatures", Version = "v1" });
            });

            services.RegisterScheduledTasks();
            services.RegisterJwtAuthentication(Configuration);

            services.AddSpaStaticFiles(opt => opt.RootPath = "ClientApp/dist");
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandling();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapToVueCliProxy(new SpaOptions { SourcePath = "ClientApp" }, npmScript: "serve", port: 8080);
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
        }
    }
}