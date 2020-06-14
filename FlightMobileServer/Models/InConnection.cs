using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FlightMobileServer.Models
{
    public class InConnection
    {
        private Client client;
        public void Connect(string ip, int port)
        {
            client.Connect(ip, port);
        }

        public void Disconnect()
        {
            client.Disconnect();
        }
        public string GetPhoto()
        {
            client.Write("get ?\r\n");
            string photo = client.Read();
            return photo;
        }
    }
}
