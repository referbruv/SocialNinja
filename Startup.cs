using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OidcApp.Models.Entities;
using OidcApp.Models.Providers;
using OidcApp.Models.Repositories;

namespace OidcApp
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
            var oidcProviders = new OidcProviders();
            Configuration.Bind("Oidc", oidcProviders);
            services.AddSingleton(oidcProviders);

            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var builder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = SocialAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = SocialAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(SocialAuthenticationDefaults.AuthenticationScheme);

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

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
