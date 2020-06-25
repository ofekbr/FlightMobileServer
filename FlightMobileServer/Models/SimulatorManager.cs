using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileServer.Models
{
    public class SimulatorManager
    {
        public readonly InConnection _inConnection;
        public readonly OutConnection _outConnection;
        public SimulatorManager(string ip, int port)
        {
            _inConnection = new InConnection();
            _outConnection = new OutConnection(ip, port);
            StartConnection();

        }

        public Boolean StartConnection()
        {
            try
            {
                _outConnection.Start();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
