using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        
        static void Main(string[] args)
        {
            bool running = true;
            TcpListener listener = new TcpListener(IPAddress.Any, 1337);
            listener.Start();
            List<Server.Client> clients = new List<Client>();
            int clientID = 0;

            while(running)
            {
                TcpClient newClient = listener.AcceptTcpClient();
                clientID++;
                clients.Add(new Server.Client(newClient, clientID));
                Console.WriteLine("New Client Connected: " + clientID);
            }
        }
    }
}
