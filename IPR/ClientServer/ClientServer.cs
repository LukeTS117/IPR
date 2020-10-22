using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace ClientServer
{
    class ClientServer
    {
        public enum NetworkType
        {
            HR, //Heartrate 
            IC, //Instant Cadence
            ID, //Patient ID
            WH, //Weight
            SX, //Sex (Male or Female)
            AG, //Age
            EI, //Ergo ID
            
        }
        public void Write(NetworkStream nws, string message)
        {
            byte[] packet = Encoding.ASCII.GetBytes(message);
            nws.Write(packet, 0, packet.Length);
            nws.Flush();
        }

        public void EncodeMessage()
        {

        }

        public void DecodeMessage()
        {

        }
    }
}
