using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocialNinja.Contracts;
using SocialNinja.Contracts.Constants;
using SocialNinja.Contracts.Data;
using SocialNinja.Core.Data;
using SocialNinja.Migrations;
using SocialNinja.Web.Models.Providers;
using SocialNinja.Web.Services;

namespace SocialNinja.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var oidcProviders = new OidcProviders();
            Configuration.Bind("Oidc", oidcProviders);
            services.AddSingleton(oidcProviders);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserManager, UserManager>();

            services.AddTransient<IClaimsTransformation, MyClaimsTransformation>();

            var builder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = SocialAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = SocialAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(SocialAuthenticationDefaults.AuthenticationScheme);

            foreach (OidcProvider provider in oidcProviders.Providers)
            {
                switch (provider.Name)
                {
                    case OidcProviderType.Google:
                        builder.AddGoogle(options =>
                        {
                            options.SaveTokens = true;
                            options.ClientId = provider.ClientId;
                            options.ClientSecret = provider.ClientSecret;
                            options.Events.OnTicketReceived = (context) =>
                            {
                                Console.WriteLine(context.HttpContext.User);
                                return Task.CompletedTask;
                            };
                            options.Events.OnCreatingTicket = (context) =>
                            {
                                Console.WriteLine(context.Identity);
                                return Task.CompletedTask;
                            };
                        });
                        break;
                    case OidcProviderType.Facebook:
                        builder.AddFacebook(options =>
                        {
                            options.SaveTokens = true;
                            options.ClientId = provider.ClientId;
                            options.ClientSecret = provider.ClientSecret;
                            options.Events.OnTicketReceived = (context) =>
                            {
                                Console.WriteLine(context.HttpContext.User);
                                return Task.CompletedTask;
                            };
                            options.Events.OnCreatingTicket = (context) =>
                            {
                                Console.WriteLine(context.Identity);
                                return Task.CompletedTask;
                            };
                        });
                        break;
                }
            }

            services.AddSqlite<DatabaseContext>(Configuration.GetConnectionString("DefaultConnection"), (options) =>
            {
                options.MigrationsAssembly("SocialNinja.Migrations");
            });

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
