using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileServer.Models
{
    public class SimulatorManager
    {
        public InConnection _inConnection;
        public OutConnection _outConnection;
        public SimulatorManager()
        {
            _inConnection = new InConnection();
            _outConnection = new OutConnection();
            _outConnection.Start();
        }
    }
}
