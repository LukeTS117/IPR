using System;
using System.Collections.Generic;
using System.IO;
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
        private FileManager fileManager;
        private string localFile = null;
        private bool fileCreated = false;
        private int clientID{ get; set; }

        
        
        public Client(TcpClient newClient, int clientID)
        {
            this.clientID = clientID;
            this.fileManager = new FileManager();
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
            this.DecodeMessage(message);
            buffer = new byte[1024];
            this.networkStream.BeginRead(this.buffer, 0, this.buffer.Length, new AsyncCallback(this.OnRead), null);
        }

        private void DecodeMessage(string message)
        {
            string sType = message.Substring(1, 2);
            if (Enum.TryParse(sType, out ClientServer.ClientServer.NetworkDataType type))
            {
               HandleMessage(type, message.Substring(4).Trim('\0'));
            };
        }

        private void HandleMessage(ClientServer.ClientServer.NetworkDataType type, string value)
        {
            if(type == ClientServer.ClientServer.NetworkDataType.PI)
            {
               // filemanger startup
               string localDir = fileManager.createDir(value);
               localFile = fileManager.creatFile(localDir);
                if(localFile != null)
                {
                    fileCreated = true;
                    fileManager.WriteToFile("<" + type.ToString() + ">" + value, localFile);
                }
            }
            else if(fileCreated && (type == ClientServer.ClientServer.NetworkDataType.AG || 
                                    type == ClientServer.ClientServer.NetworkDataType.WH ||
                                    type == ClientServer.ClientServer.NetworkDataType.SX ||
                                    type == ClientServer.ClientServer.NetworkDataType.RS ||
                                    type == ClientServer.ClientServer.NetworkDataType.DP))
            {
                fileManager.WriteToFile("<"+type.ToString() + ">" + value, localFile);
            }
        }
    }
}
