﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClientServer;

namespace IPR
{
    class Client
    {
        private readonly TcpClient tcpClient;
        private NetworkStream networkStream;
        private byte[] buffer;
        public string message;

        public Client()
        {
            tcpClient = new TcpClient();
            this.buffer = new byte[1024];
        }

        public void Connect(string server, int port)
        {
            this.tcpClient.Connect(server, port);
            this.networkStream =  this.tcpClient.GetStream();
            this.networkStream.BeginRead(this.buffer, 0, this.buffer.Length, new AsyncCallback(this.OnRead), null);
        }

        public void OnRead(IAsyncResult ar)
        {
            int bytesRead = networkStream.EndRead(ar);
            string message = Encoding.ASCII.GetString(this.buffer);

        }

        public void NotifyNewTest()
        {

        }
    }
}
