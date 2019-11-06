using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR.BLEHandling
{

     
    class BLEHandler
    {
        public BLEConnect bleConnection;
        string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public BLEHandler(BLEConnect bleConnection)
        {
            this.bleConnection = bleConnection;
        }

        public void handleData(byte[] rawData)
        {
            if(rawData[0] == 22)
            {
                int heartRate = rawData[1];
                Console.WriteLine("Heartrat: " + heartRate);
                this.WriteToFile("<HR>" + heartRate);

            }
            else if(rawData[0] == 164)
            {
                int messageLength = rawData[1];
                byte[] message = rawData.Skip(4).Take(messageLength).ToArray();
                int pageNumber = message[0];

                if(pageNumber == 25)
                {
                    int instanteousCadence = message[2];
                    Console.WriteLine("instantaneos cadence:" + instanteousCadence);
                    this.WriteToFile("<IC>" + instanteousCadence);
                }
            }
        }

        public void WriteToFile(string message)
        {
            
            string path = this.dir + @"\TestData.txt";

            if (!File.Exists(path))
            {
                Directory.CreateDirectory(dir);
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(message);
                }
            }
        }

        public string ReadFile()
        {

            string path = this.dir + @"\TestData.txt";
            string packet = "";

            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    while (sr.ReadLine() != null)
                    {
                        packet += sr.ReadLine();
                    }
                    return packet;
                }
            }
            else
            {
                Console.WriteLine("This file does not exist");
                return "";
            }


        }
    }

}

