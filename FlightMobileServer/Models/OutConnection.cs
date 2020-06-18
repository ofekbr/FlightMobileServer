using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FlightMobileServer.Models
{
    public class OutConnection
    {
        private readonly BlockingCollection<AsyncCommand> _queue;
        private Client client;

        public OutConnection()
        {
            _queue = new BlockingCollection<AsyncCommand>();
            client = new Client();
        }
        public void Connect(string ip, int port)
        {
            client.Connect(ip, port);
        }

        public void Disconnect()
        {
            client.Disconnect();
        }

        public void SendInfo(double value, string strVal)
        {
            string send = "";
            switch (strVal)
            {
                case "throttle":
                    {
                        send = "set /controls/engines/current-engine/throttle " + value;
                        break;
                    }
                case "elevator":
                    {
                        send = "set /controls/flight/elevator " + value;
                        break;
                    }
                case "rudder":
                    {
                        send = "set /controls/flight/rudder " + value;
                        break;
                    }
                case "aileron":
                    {
                        send = "set /controls/flight/aileron " + value;
                        break;
                    }
            }
            send += "\r\n";
            client.Write(send);
        }

        public double GetInfo(string strVal)
        {
            string send = "";
            switch (strVal)
            {
                case "throttle":
                    {
                        send = "get /controls/engines/current-engine/throttle";
                        break;
                    }
                case "elevator":
                    {
                        send = "get /controls/flight/elevator";
                        break;
                    }
                case "rudder":
                    {
                        send = "get /controls/flight/rudder";
                        break;
                    }
                case "aileron":
                    {
                        send = "get /controls/flight/aileron";
                        break;
                    }
            }
            send += "\r\n";
            client.Write(send);
            string value = client.Read();
            try
            {
                double retValue = Double.Parse(value);
                return retValue;

            }
            catch (Exception)
            {
                throw new Exception();
            }
            
        }

        public Task<ActionResult> Execute(Command cmd)
        {
            var asyncCommand = new AsyncCommand(cmd);
            _queue.Add(asyncCommand);
            return asyncCommand.Task;
        }
        public void Start()
        {
            Task.Factory.StartNew(ProcessCommands);
        }

        public void ProcessCommands()
        {
            client.Connect("127.0.0.1", 5402);
            ActionResult res;
            foreach (AsyncCommand command in _queue.GetConsumingEnumerable())
            {
                //send command
                try
                {
                    SendInfo(command.Command.Aileron, "aileron");
                    if (GetInfo("aileron")  != command.Command.Aileron)
                        throw new Exception();
                    SendInfo(command.Command.Elevator, "elevator");
                    if (GetInfo("elevator") != command.Command.Elevator)
                        throw new Exception();
                    SendInfo(command.Command.Rudder, "rudder");
                    if (GetInfo("rudder") != command.Command.Rudder)
                        throw new Exception();
                    SendInfo(command.Command.Throttle, "throttle");
                    if (GetInfo("throttle") != command.Command.Throttle)
                        throw new Exception();

                }
                catch (Exception)
                {
                    //TODO read and check 
                    res = new BadRequestResult();
                }

                 res = new OkResult();
                
                command.Completion.SetResult(res);
            }
        }
    }
}