using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlightMobileServer.Models
{
    public class Client
    {
        /// <summary>The size</summary>
        private const short Size = 512;

        /// <summary>The client</summary>
        private TcpClient client;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public Client()
        {
            this.client = new TcpClient(AddressFamily.InterNetwork);
        }

        public bool IsConnected
        {
            get
            {
                try
                {
                    if (client != null && client.Client != null && client.Client.Connected)
                    {
                        /* pear to the documentation on Poll:
                         * When passing SelectMode.SelectRead as a parameter to the Poll method it will return 
                         * -either- true if Socket.Listen(Int32) has been called and a connection is pending;
                         * -or- true if data is available for reading; 
                         * -or- true if the connection has been closed, reset, or terminated; 
                         * otherwise, returns false
                         */

                        // Detect if client disconnected
                        if (client.Client.Poll(0, SelectMode.SelectRead))
                        {
                            byte[] buff = new byte[1];
                            if (client.Client.Receive(buff, SocketFlags.Peek) == 0)
                            {
                                // Client disconnected
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }


        public void Connect(string ip, int port)
        {
            this.client.Connect(IPAddress.Parse(ip), port);
        }

        public bool isConnected()
        {
            return this.client.Connected;
        }

        public void Disconnect()
        {
            this.client.Close();
        }

        public void Write(string data)
        {
            if (this.isConnected())
            {
                NetworkStream networkStream = this.client.GetStream();
                byte[] sendBytes = Encoding.ASCII.GetBytes(data);
                networkStream.Write(sendBytes, 0, sendBytes.Length);
            }
        }


        public void flush()
        {
            this.client.GetStream().Flush();
        }

        public string Read()
        {
            if (this.isConnected())
            {
                NetworkStream ns = this.client.GetStream();
                byte[] dataBytes = new byte[Size];
                int bytesRead = ns.Read(dataBytes, 0, Size);
                string dataToSend = Encoding.ASCII.GetString(dataBytes, 0, bytesRead);
                return dataToSend;
            }
            throw new Exception("Client disconnected, turn FlightGear on and press connect");
        }
    }
}

