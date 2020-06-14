using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FlightMobileServer.Models
{
    public class OutConnection
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
        public void SendInfo(int value, string strVal)
        {
            // TODO: check about the ip and port - how to recieve it?
            client.Connect("127.0.0.1", 1234);
            string send = "";
            switch (strVal)
            {
                case "throttle":
                    {
                        send = "set /controls/engines/current-engine/throttle" + value;
                        break;
                    }
                case "elevator":
                    {
                        send = "set /controls/flight/elevator" + value;
                        break;
                    }
                case "rudder":
                    {
                        send = "set /controls/flight/rudder" + value;
                        break;
                    }
                case "aileron":
                    {
                        send = "set /controls/flight/aileron" + value;
                        break;
                    }
            }
            client.Write(send);
            client.Disconnect();
        }
    }
}
