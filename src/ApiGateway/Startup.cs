using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Extensions;
using Common.Services;
using MassTransit;
using AppsOnSF.Common.BaseServices;
using AppsOnSF.Common.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ApiGateway
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
            services.AddHealthChecks();

            //基础服务
#if !DEBUG
            services.AddDataProtection()
                .SetApplicationName("EAP-ApiGateway")
                .PersistKeysToServiceFabric();
#endif
            services.AddMemoryCache();

            services.AddHttpContextAccessor();

            services.Configure<FormOptions>(opt =>
            {
                opt.MultipartBodyLengthLimit = int.MaxValue;
            });

            services.AddHttpClient<IIdentityService, IdentityService>();

            // Register the Swagger services
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "EAP API";
                    document.Info.Description = "Enterprise Apps Platform API";
                    document.Info.TermsOfService = "None";
                    document.SecurityDefinitions.Add("Bearer", new NSwag.SwaggerSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                        Name = "Authorization",
                        Type = NSwag.SwaggerSecuritySchemeType.ApiKey,
                        In = NSwag.SwaggerSecurityApiKeyLocation.Header
                    });
                    document.Security.Add(new NSwag.SwaggerSecurityRequirement { { "Bearer", new string[] { } }, });
                };
            });

            //公共依赖
            services.AddSingleton(CreateMapper());
            services.AddSingleton(RemotingProxyFactory.CreateSimpleKeyValueService());
            var (bus, bus_option) = this.CreateBus("RabbitMQ");
            services.AddCommonServices(bus, bus_option.HostAddress);

            //业务依赖
            AppComponent.TheStartup.ConfigureServices(services);
            Attachment.TheStartup.ConfigureServices(services);
            ConfigMgmt.TheStartup.ConfigureServices(services);
            EnterpriseContact.TheStartup.ConfigureServices(services);
            GroupFile.TheStartup.ConfigureServices(services);
            InstantMessage.TheStartup.ConfigureServices(services);
            NotifyStartup.ConfigureServices(services);
            
            //Application Parts
            var assemblyEC = typeof(EnterpriseContact.TheStartup).GetTypeInfo().Assembly;
            var partEC = new AssemblyPart(assemblyEC);

            //TODO 没有加ConfigMgmtApi，也能用？

            //var assemblyCM=typeof()
            services.AddMvc()
                .ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(partEC))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Authentication
            var idSvrOption = Configuration.GetSection("IdSvr").Get<IdSvrOption>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.Authority = idSvrOption.IssuerUri;
                 options.RequireHttpsMetadata = idSvrOption.RequireHttps;
                 options.Audience = "eap.api";

                 options.TokenValidationParameters =
                   new TokenValidationParameters
                   {
                       NameClaimType = "preferred_username",
                       ValidateAudience = false,
                       ValidateIssuer = false,
                       ValidateActor = false,
                       ValidateLifetime = true,
                       LifetimeValidator = (before, expires, token, param) =>
                       {
                           return expires > DateTime.UtcNow;
                       },
                   };
             });

            //相关配置
            services.Configure<IdSvrOption>(Configuration.GetSection("IdSvr"));
            services.Configure<MinioOption>(Configuration.GetSection("Minio"));
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

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseSwagger();
            app.UseSwaggerUi3();

            app.UseAuthentication();
            app.UseMvc();
        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EnterpriseContact.VmMappingProfile>();
            });
            return config.CreateMapper();
        }

        private (IBusControl, RabbitMQOption) CreateBus(string sectionName)
        {
            var option = Configuration.GetSection(sectionName).Get<RabbitMQOption>();
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(option.HostAddress), h =>
                {
                    h.Username(option.Username);
                    h.Password(option.Password);
                });
            });
            return (bus, option);
        }
    }
}
