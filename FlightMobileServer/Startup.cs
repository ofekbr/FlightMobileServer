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
            services.AddSingleton<SimulatorManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var rewrite = new RewriteOptions()
               .AddRewrite("command", "api/command", true)
               .AddRewrite("disconnect", "api/command", true);

            rewrite.Rules.Add(new CustomRule());
            app.UseRewriter(rewrite);
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }



    public class CustomRule : Microsoft.AspNetCore.Rewrite.IRule
    {
        private InConnection _inConnection;
        
        public CustomRule()
        {
            _inConnection = new InConnection();
            //TODO try and catch
            try
            {
                _inConnection.Connect("http://localhost:5000/screenshot");
            }
            catch (Exception e)
            {

                int f = 5;    
            }
        }
        
        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;
            var host = request.Host;
            if (request.Path.Value.Contains("screenshot"))
            {
                var result = ContentAction(context);
                return;
            } else if (request.Path.Value.Contains("command"))
            {
                return;
            }


            var response = context.HttpContext.Response;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = RuleResult.EndResponse;


        }
        
        public async Task<HttpResponse> ContentAction(RewriteContext context)
        {
            var response = context.HttpContext.Response;
            byte[] flightGearImage = Encoding.ASCII.GetBytes( await _inConnection.CreateRequestToServer("GET", "http://localhost:5000/screenshot"));
            response.ContentType = "screenshot";
            await response.Body.WriteAsync(flightGearImage, 0, flightGearImage.Length);
            return response;
        }
    }
}
