using Microsoft.AspNetCore.Mvc;
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
            // TODO: check about the ip and port - how to recieve it?
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
                    SendInfo(command.Command.Elevator, "elevator");
                    SendInfo(command.Command.Rudder, "rudder");
                    SendInfo(command.Command.Throttle, "throttle");

                }catch(Exception)
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