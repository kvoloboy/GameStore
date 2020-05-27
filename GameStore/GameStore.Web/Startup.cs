using System.Globalization;
using Autofac;
using AutoMapper;
using GameStore.BusinessLayer;
using GameStore.BusinessLayer.Mappings;
using GameStore.Common;
using GameStore.Common.Mappings;
using GameStore.Common.Models;
using GameStore.DataAccess.Mongo.Mappings;
using GameStore.DataAccess.Sql.Context;
using GameStore.Identity;
using GameStore.Identity.CookieValidation;
using GameStore.Infrastructure;
using GameStore.Web.Filters;
using GameStore.Web.Mapping;
using GameStore.Web.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GameStore.Web
{
    public class Startup
    {
        private IConfigurationRoot Configuration { get; set; }

        public Startup(IHostEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/sign-in");
                    options.AccessDeniedPath = new PathString("/sign-in");
                    options.EventsType = typeof(CustomCookieAuthenticationEvents);
                });
            services.AddAuthorization();

            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });

            services.AddMvc(options =>
                {
                    options.CacheProfiles.Add("Default60", new CacheProfile
                    {
                        Duration = 60,
                        Location = ResponseCacheLocation.Any
                    });

                    options.Filters.Add<RequestIpFilter>();
                    options.Filters.Add<BenchmarkServicesFilter>();
                    options.Filters.Add<GuestConfigurationFilter>();
                    // options.Filters.Add<ExceptionFilter>();
                    
                    options.EnableEndpointRouting = false;
                    options.SuppressAsyncSuffixInActionNames = false;
                }).AddRazorRuntimeCompilation()
                .AddMvcLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo(Culture.En),
                    new CultureInfo(Culture.Ru)
                };

                options.DefaultRequestCulture = new RequestCulture(Culture.En);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddAutoMapper(
                typeof(DtoToEntity),
                typeof(EntityToDto),
                typeof(ViewModelToDto),
                typeof(DtoToViewModel),
                typeof(EntityToViewModel),
                typeof(SelfMappings),
                typeof(DtoToDto),
                typeof(CrossDatabaseMappings));

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("GameStoreContext")));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new SqlPersistenceModule());
            builder.RegisterModule(new BusinessLayerModule());
            builder.RegisterModule(new WebModule());
            builder.RegisterModule(new MongoPersistenceModule());
            builder.RegisterModule(new InfrastructureModule());
            builder.RegisterModule(new IdentityModule());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseStatusCodePagesWithRedirects("/error/{0}");
            }

            app.UseMiddleware<ImageLoaderMiddleware>();
            app.UseMiddleware<ImageWriterMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();

            app.UseRequestLocalization();

            app.UseStaticFiles();

            app.UseMvc();
            EntityMappings.Configure();
        }
    }
}