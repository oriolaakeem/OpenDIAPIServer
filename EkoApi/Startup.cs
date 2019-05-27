using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EkoApiCore.Data;
using Microsoft.EntityFrameworkCore;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using System.IO;
using Newtonsoft.Json.Serialization;
using EkoApiCore.Filters;

namespace EkoApiCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
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

            
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile(Configuration.GetSection("Logging"));

            string repDirName = Configuration["CrystalReport:reportsFolder"];
            if (!Directory.Exists(repDirName))
            {
                Directory.CreateDirectory(repDirName);
            }

            //string idSrv = env.IsDevelopment() ? Configuration["IdentityServerUrl:Development"] : Configuration["IdentityServerUrl:Production"];
            //app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            //{
            //    Authority = idSrv,
            //    RequireHttpsMetadata = false,
            //    ApiName = "payslipEmailApi"
            //});


            app.UseMvc(config =>
            {
                config.MapRoute(
                  name: "Default",
                  template: "{controller}/{action}/{id?}"
                  );


            });
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            


            services.AddMvcCore(config =>
            {
                config.Filters.Add(typeof(CustomExceptionFilter));
            })
                 .AddAuthorization()
                 .AddJsonFormatters(opt =>
                      {
                          opt.ContractResolver = new CamelCasePropertyNamesContractResolver();
                          opt.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                      })
                     ;


            string conString = Configuration["Data:DefaultConnection:connString"];

            services.AddDbContext<SAPB1>(options => options.UseSqlServer(conString));

            var builder = new ContainerBuilder();


            var opt1 = new DbContextOptionsBuilder();
            opt1.UseSqlServer(conString);

            builder.RegisterType<SAPB1>()
           .As<DbContext>()
           .WithParameter("options", opt1)
           .InstancePerLifetimeScope();

            builder.Populate(services);

            this.ApplicationContainer = builder.Build();


            return this.ApplicationContainer.Resolve<IServiceProvider>();
        }
       
    }

}
