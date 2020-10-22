using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClientServer;
using IPR.BLEHandling;

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

        public void NotifyNewTest(int age, int weight, bool male)
        {
            string sex = "male";
            if (!male)
            {
                sex = "female";
            }

            ClientServer.ClientServer.Write(this.networkStream, ClientServer.ClientServer.EncodeMessage(ClientServer.ClientServer.NetworkDataType.AG, age.ToString()));
            ClientServer.ClientServer.Write(this.networkStream, ClientServer.ClientServer.EncodeMessage(ClientServer.ClientServer.NetworkDataType.WH, weight.ToString()));
            ClientServer.ClientServer.Write(this.networkStream, ClientServer.ClientServer.EncodeMessage(ClientServer.ClientServer.NetworkDataType.SX, sex));
        }

        public void SendDataPoint(string dataPoint)
        {
            ClientServer.ClientServer.Write(this.networkStream, ClientServer.ClientServer.EncodeMessage(ClientServer.ClientServer.NetworkDataType.DP, dataPoint));
        }

        public void SendResult(double result)
        {
            ClientServer.ClientServer.Write(this.networkStream, ClientServer.ClientServer.EncodeMessage(ClientServer.ClientServer.NetworkDataType.DP, result.ToString()));
        }
    }
}
