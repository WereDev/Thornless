using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;

namespace Thornless.UI.Web.AppStart
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.RegisterServices();
            services.AddControllers();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureExceptionHandler();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            });

            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            else
            {
                app.UseSpaStaticFiles(new StaticFileOptions()
                {
                    OnPrepareResponse = ctx =>
                    {
                        if (ctx.Context.Request.Path.Value.ToLower().EndsWith("index.html"))
                        {
                            // Do not cache explicit `index.html`. See also: `DefaultPageStaticFileOptions` below for implicit "/index.html"
                            SetNoCacheOnHeader(ctx.Context.Response.Headers, 0);
                        }
                        else
                        {
                            // Cache all static resources for 30 days (versioned filenames)
                            SetNoCacheOnHeader(ctx.Context.Response.Headers, 30);
                        }
                    },
                });
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
                else
                {
                    spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions()
                    {
                        OnPrepareResponse = ctx =>
                        {
                            // Do not cache implicit `/index.html`.  See also: `UseSpaStaticFiles` above
                            SetNoCacheOnHeader(ctx.Context.Response.Headers, 0);
                        },
                    };
                }
            });
        }

        private void SetNoCacheOnHeader(IHeaderDictionary headers, int expirationDays)
        {
            headers.Append("CacheControl", "no-cache, no-store, must-revalidate");
            headers.Append("Expires", expirationDays.ToString());
            headers.Append("Pragma", "no-cache");
        }
    }
}
