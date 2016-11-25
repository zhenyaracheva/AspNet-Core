namespace CityInfo.Core
{
    using Entities.DbContexts;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Serialization;
    using NLog.Extensions.Logging;
    using Services.RepositoryServices;

    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSettings.json", optional:false, reloadOnChange:true)
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(o=>o.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter()));
            // make json props as they are (not lowercase)
            //.AddJsonOptions(o =>
            //{
            //    if (o.SerializerSettings != null)
            //    {
            //        var casterResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
            //        casterResolver.NamingStrategy = null;
            //    }
            //});
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
             services.AddTransient<IMailService, CloudMailService>();
#endif
            //var connectionString = "Server=.;Database=CityInfoDb;Trusted_Connection=True;";
            var connectionString = Startup.Configuration["connectionStrings:cityInfoDbConnectionString"];
            services.AddDbContext<CityInfoDbContext>(o=> o.UseSqlServer(connectionString));
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CityInfoDbContext context)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            //loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider());
            loggerFactory.AddNLog();

            if (env.IsDevelopment())

            {
                app.UseDeveloperExceptionPage();
            }

            context.EnsureSeedDataForContext();

            //show status code instead of blank page (browser)
            app.UseStatusCodePages();
            app.UseMvc();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
