using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace ClientServer
{
    class ClientServer
    {
        public enum NetworkDataType
        {
            DP, //Datapoint
            AG, //Age
            WH, //Wheight
            PI, //Patient ID
            SX, //Sex: Male/Female
            RS, //Result
            
        }
        public static void Write(NetworkStream nws, string message)
        {
            byte[] packet = Encoding.ASCII.GetBytes(message);
            nws.Write(packet, 0, packet.Length);
            nws.Flush();
        }

        public static string EncodeMessage(NetworkDataType type, string message)
        {
            return "<" + type.ToString() + ">" + message;
        }

        public static void DecodeMessage()
        {

        }
    }
}
