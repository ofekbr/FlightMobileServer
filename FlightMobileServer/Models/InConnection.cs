using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FlightMobileServer.Models
{
    public class InConnection
    {
        public void Connect(string url)
        {
            //TODO create http get request
            //client.Connect(ip, port);
        }
        public async Task<string> CreateRequestToServer(string request, string url)
        {
            string strUrl = String.Format(url);
            WebRequest requestObjGet = WebRequest.Create(strUrl);
            requestObjGet.Method = request;
            HttpWebResponse responseObjGet;
            responseObjGet = (HttpWebResponse)requestObjGet.GetResponse();

            string strResult = null;
            using (Stream stream = responseObjGet.GetResponseStream())
            {
                
                StreamReader sr = new StreamReader(stream);
                //strResult = sr.ReadToEnd();
                strResult = await sr.ReadToEndAsync();
                sr.Close();
            }

            //option if result is null or not right syntax
            if (strResult.Length < 5)
                return null;
            return strResult;
        }
    }
}
