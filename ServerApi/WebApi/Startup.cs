using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Server_Data;
using Entity.Database;
using Microsoft.EntityFrameworkCore;
using Server_Model.Base;
using WebApi.Middleware;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using WebApi.Logging;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private static TraceListener Listener;
        static void ConfigureTraceSourceLogging()
        {
            var fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            if (File.Exists(fileName))
                File.Create(fileName).Close();

            var writer = File.CreateText(fileName);
            Listener = new TextWriterTraceListener(writer.BaseStream);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //ConfigureTraceSourceLogging();
            services.AddControllers();
            services.AddDbContext<Server_Data.Context.ApplicationDatabaseContext>(opt => opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), options =>
             {
                options.SetPostgresVersion(new Version("9.3"));
                options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), new string[] { "57P01" });
                options.MigrationsAssembly("Server-Api");
            }));

            services.AddLogging();

            services.AddTransient<EntitysModel>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });


            services.AddLogging(c => c.AddConfiguration(Configuration.GetSection("Logging"))
                //.AddTraceSource(new SourceSwitch("TraceSourceLogging", SourceLevels.Verbose.ToString()), Listener)
                .AddConsole());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<AuthenticationMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSwagger();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                //c.RoutePrefix = string.Empty;
            });

            loggerFactory.AddFile(Directory.GetCurrentDirectory());
        }
    }
}
