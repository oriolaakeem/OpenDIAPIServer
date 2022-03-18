using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using OpenDIAPIServer.Filters;
using OpenDIAPIServer.models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenDIAPIServer.Services;
using Microsoft.AspNetCore.Mvc.Formatters;
using Swashbuckle.AspNetCore.Swagger;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace OpenDIAPIServer
{
    public class Startup
    {

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment Environment { get; }
        public IContainer ApplicationContainer { get; private set; }

        //// This method gets called by the runtime. Use this method to add services to the container.
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    // Add framework services.
        //    services.AddMvc();
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI();

            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            //loggerFactory.AddFile(Configuration.GetSection("Logging"));


            //string idSrv = env.IsDevelopment() ? Configuration["IdentityServerUrl:Development"] : Configuration["IdentityServerUrl:Production"];
            //app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            //{
            //    Authority = idSrv,
            //    RequireHttpsMetadata = false,
            //    ApiName = "payslipEmailApi"
            //});z


            app.UseMvc(config =>
            {
                config.MapRoute(
                  name: "Default",
                  template: "api/{controller}/{action}/{id?}"
                  );


            });
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddControllersAsServices();

            //services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Add framework services.
            services.AddCors();
            services.AddMvc(options => { // allow xml format for input 
                options.InputFormatters.Add(new XmlSerializerInputFormatter(options)); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "DI API Server",
                    Version = "v1",
                    Description = "Open DI API Server",
                    TermsOfService = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html"),
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Oriola Enterprises ",
                        Email = "oriolaakeem@outlook.com"
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense
                    {
                        Name = "Apache 2.0",
                        Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html")
                    }
                });
                c.CustomSchemaIds(x => x.FullName);
            });

            string apiUrl =  Configuration["SAP:APIUrl"];
            services.AddHttpClient<ApplicationClient>(client =>
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            });

            services.AddMvcCore(config =>
            {
                config.Filters.Add(typeof(CustomExceptionFilter));
                config.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml").ToString());
                config.FormatterMappings.SetMediaTypeMappingForFormat("config", MediaTypeHeaderValue.Parse("application/xml").ToString());
                config.FormatterMappings.SetMediaTypeMappingForFormat("js", MediaTypeHeaderValue.Parse("application/json").ToString());
            })
                 .AddAuthorization()
                 .AddXmlSerializerFormatters()
                 .AddJsonFormatters(opt =>
                      {
                          opt.ContractResolver = new CamelCasePropertyNamesContractResolver();
                          opt.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                      })
                     ;
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IServerTokenComponent, ServerTokenComponent>();
            var sapOpts = new SAP
            {
                LicenseServer = Configuration["SAP:LicenseServer"],
                ClientValidationEnabled = Configuration["SAP:ClientValidationEnabled"],
                CompanyDB = Configuration["SAP:CompanyDB"],
                DBPassword = Configuration["SAP:DBPassword"],
                DBUserName = Configuration["SAP:DBUserName"],
                DBServer = Configuration["SAP:DBServer"],
                SAPPassword = Configuration["SAP:SAPPassword"],
                SAPUserName = Configuration["SAP:SAPUserName"],
                UnobtrusiveJavaScriptEnabled = Configuration["SAP:UnobtrusiveJavaScriptEnabled"]
            };


            var builder = new ContainerBuilder();

            builder.RegisterInstance(sapOpts);

            builder.Populate(services);

            this.ApplicationContainer = builder.Build();


            return this.ApplicationContainer.Resolve<IServiceProvider>();
        }
       
    }

}
