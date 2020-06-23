﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlightMobileServer.Controllers
{
    [Route("/screenshot")]
    public class ImageController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {           
            //string url = "http://localhost:" + this.httpPort + "/screenshot";
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://localhost:5000/screenshot");
            myReq.Method = "GET";
            WebResponse myResponse = myReq.GetResponse();
            MemoryStream ms = new MemoryStream();
            myResponse.GetResponseStream().CopyTo(ms);
            byte[] data = ms.ToArray();
            return File(data, "image/png");
            //return HttpResponseMessage(data, "image/png");
            //byte[] flightGearImage = Encoding.ASCII.GetBytes(await _inConnection.CreateRequestToServer("GET", url));return new string[] { "value1", "value2" };
        }

    }
}