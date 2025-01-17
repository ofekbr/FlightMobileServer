using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FlightMobileServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FlightMobileServer
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
            services.AddControllers();
            string ip = Configuration.GetValue<string>("Logging:SimulatorInfo:IP");
            int port = Configuration.GetValue<int>("Logging:SimulatorInfo:TelnetPort");
            SimulatorManager sm = new SimulatorManager(ip, port);
            services.AddSingleton(sm);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            int port = Configuration.GetValue<int>("Logging:SimulatorInfo:HttpPort");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           /* var rewrite = new RewriteOptions()
               .AddRewrite("command", "api/command", true)
               .AddRewrite("disconnect", "api/command", true);*/

            //app.UseHttpsRedirection();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
