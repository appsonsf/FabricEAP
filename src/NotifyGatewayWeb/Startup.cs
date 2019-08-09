using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppsOnSF.Common.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification;
using NotifyGatewayWeb.Hubs;
using NotifyGatewayWeb.EventBus;
using Microsoft.AspNetCore.DataProtection;
using AppsOnSF.Common.BaseServices;
using Microsoft.IdentityModel.Tokens;
using Common.Extensions;

namespace NotifyGatewayWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            //基础服务
#if !DEBUG
            services.AddDataProtection()
                .SetApplicationName("EAP-NotifyGateway")
                .PersistKeysToServiceFabric();
#endif

            //业务依赖
            services.AddScoped<Func<Guid, INotifySessionActor>>(u => RemotingClient.CreateNotifySessionActor);

            //SignalR
            var redisHosts = Configuration.GetSection("Redis").GetValue<string>("Hosts").Split(' ');
            services.AddSignalR()
#if DEBUG
                ;
#else
                .AddStackExchangeRedis(redisHosts[0]);
#endif

            //Authentication
            var idSvrOption = Configuration.GetSection("IdSvr").Get<IdSvrOption>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = idSvrOption.IssuerUri;
                options.RequireHttpsMetadata = idSvrOption.RequireHttps;

                // Configure JWT Bearer Auth to expect our security key
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateActor = false,
                        ValidateLifetime = true,
                        LifetimeValidator = (before, expires, token, param) =>
                        {
                            return expires > DateTime.UtcNow;
                        },
                    };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddEventBus();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready") });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureExceptionHandler();

            app.UseAuthentication();
            app.UseSignalR(routes =>
            {
                routes.MapHub<NotifyHub>("/notifyHub");
            });
        }
    }
}
