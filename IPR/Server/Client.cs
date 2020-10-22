using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Client
    {
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        private byte[] buffer;
        private int clientID{ get; set; }
        
        public Client(TcpClient newClient, int clientID)
        {
            this.clientID = clientID;
            this.startClient(newClient);
        }

        private void startClient(TcpClient newClient)
        {
            this.tcpClient = newClient;
            this.networkStream = this.tcpClient.GetStream();
            this.buffer = new byte[1024];

            this.networkStream.BeginRead(this.buffer, 0, this.buffer.Length, new AsyncCallback(this.OnRead), null);
        }

        public void OnRead(IAsyncResult ar)
        {
            int bytesRead = networkStream.EndRead(ar);
            string message = Encoding.ASCII.GetString(this.buffer);
            Console.WriteLine(message);
            
            buffer = new byte[1024];
            this.networkStream.BeginRead(this.buffer, 0, this.buffer.Length, new AsyncCallback(this.OnRead), null);
        }
    }
}
