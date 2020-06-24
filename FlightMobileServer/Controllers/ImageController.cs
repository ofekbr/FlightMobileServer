using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlightMobileServer.Controllers
{
    [Route("/screenshot")]
    public class ImageController : Controller
    {
        private IConfiguration configuration { get; }
        public ImageController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string httpPort = configuration.GetValue<string>("Logging:SimulatorInfo:HttpPort");
            string ip = configuration.GetValue<string>("Logging:SimulatorInfo:IP");
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://" + ip + ":" + httpPort + "/screenshot");
            myReq.Method = "GET";
            try
            {
                WebResponse myResponse = myReq.GetResponse();
                MemoryStream ms = new MemoryStream();
                myResponse.GetResponseStream().CopyTo(ms);
                byte[] data = ms.ToArray();
                return File(data, "image/png");
            } catch(Exception)
            {
                return BadRequest();
            }
        }

    }
}
