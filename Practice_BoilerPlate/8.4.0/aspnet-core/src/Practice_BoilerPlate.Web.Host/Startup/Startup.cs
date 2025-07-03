    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Castle.Facilities.Logging;
    using Abp.AspNetCore;
    using Abp.AspNetCore.Mvc.Antiforgery;
    using Abp.Castle.Logging.Log4Net;
    using Abp.Extensions;
    using Practice_BoilerPlate.Configuration;
    using Practice_BoilerPlate.Identity;
    using Abp.AspNetCore.SignalR.Hubs;
    using Abp.Dependency;
    using Abp.Json;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json.Serialization;
    using System.IO;
    using Practice_BoilerPlate.SignalR;
    using Stripe;

    namespace Practice_BoilerPlate.Web.Host.Startup
    {
        public class Startup
        {
            private const string _defaultCorsPolicyName = "localhost";

            private const string _apiVersion = "v1";

            private readonly IConfigurationRoot _appConfiguration;
            private readonly IWebHostEnvironment _hostingEnvironment;

            public Startup(IWebHostEnvironment env)
            {
                _hostingEnvironment = env;
                _appConfiguration = env.GetAppConfiguration();
            }

            public void ConfigureServices(IServiceCollection services)
            {
                //MVC
                services.AddControllersWithViews(
                    options => { options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute()); }
                ).AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new AbpMvcContractResolver(IocManager.Instance)
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    };
                });

                IdentityRegistrar.Register(services);
                AuthConfigurer.Configure(services, _appConfiguration);

                services.AddSignalR();

                // Configure CORS for angular2 UI
                services.AddCors(
                    options => options.AddPolicy(
                        _defaultCorsPolicyName,
                        builder => builder
                            .WithOrigins(
                                // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                                _appConfiguration["App:CorsOrigins"]
                                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                    .Select(o => o.RemovePostFix("/"))
                                    .ToArray()
                            )
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                    )
                );

                // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
                ConfigureSwagger(services);

                // Configure Abp and Dependency Injection
                services.AddAbpWithoutCreatingServiceProvider<Practice_BoilerPlateWebHostModule>(
                    // Configure Log4Net logging
                    options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                        f => f.UseAbpLog4Net().WithConfig(_hostingEnvironment.IsDevelopment()
                            ? "log4net.config"
                            : "log4net.Production.config"
                        )
                    )
                );
            }

            public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
            {
                StripeConfiguration.ApiKey = _appConfiguration["Stripe:SecretKey"];


                app.UseAbp(options => { options.UseAbpRequestLocalization = false; }); // Initializes ABP framework.

                app.UseCors(_defaultCorsPolicyName); // Enable CORS!

                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();

                app.UseAbpRequestLocalization();
            
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<ChatHub>("/signalr/chatHub");

                    endpoints.MapHub<AbpCommonHub>("/signalr");
                    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
                });

                // Enable middleware to serve generated Swagger as a JSON endpoint
                app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });

                // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
                app.UseSwaggerUI(options =>
                {
                    // specifying the Swagger JSON endpoint.
                    options.SwaggerEndpoint($"/swagger/{_apiVersion}/swagger.json", $"Practice_BoilerPlate API {_apiVersion}");
                    options.IndexStream = () => Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("Practice_BoilerPlate.Web.Host.wwwroot.swagger.ui.index.html");
                    options.DisplayRequestDuration(); // Controls the display of the request duration (in milliseconds) for "Try it out" requests.  
                }); // URL: /swagger
            }
        
            private void ConfigureSwagger(IServiceCollection services)
            {
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc(_apiVersion, new OpenApiInfo
                    {
                        Version = _apiVersion,
                        Title = "Practice_BoilerPlate API",
                        Description = "Practice_BoilerPlate",
                        // uncomment if needed TermsOfService = new Uri("https://example.com/terms"),
                        Contact = new OpenApiContact
                        {
                            Name = "Practice_BoilerPlate",
                            Email = string.Empty,
                            Url = new Uri("https://twitter.com/aspboilerplate"),
                        },
                        License = new OpenApiLicense
                        {
                            Name = "MIT License",
                            Url = new Uri("https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/LICENSE"),
                        }
                    });
                    options.DocInclusionPredicate((docName, description) => true);

                    // Define the BearerAuth scheme that's in use
                    options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
                    {
                        Description =
                            "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });

                    //add summaries to swagger
                    bool canShowSummaries = _appConfiguration.GetValue<bool>("Swagger:ShowSummaries");
                    if (canShowSummaries)
                    {
                        var hostXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                        var hostXmlPath = Path.Combine(AppContext.BaseDirectory, hostXmlFile);
                        options.IncludeXmlComments(hostXmlPath);

                        var applicationXml = $"Practice_BoilerPlate.Application.xml";
                        var applicationXmlPath = Path.Combine(AppContext.BaseDirectory, applicationXml);
                        options.IncludeXmlComments(applicationXmlPath);

                        var webCoreXmlFile = $"Practice_BoilerPlate.Web.Core.xml";
                        var webCoreXmlPath = Path.Combine(AppContext.BaseDirectory, webCoreXmlFile);
                        options.IncludeXmlComments(webCoreXmlPath);
                    }
                });
            }
        }
    }
