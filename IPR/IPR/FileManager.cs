using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR
{
    class FileWriter
    {

        string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ClientData";

        
        public void WriteToFile(string message, string clientID)
        {
            if (!File.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string path = this.dir + @"\" + clientID;

            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);

                using (StreamWriter sw = File.CreateText(path + @"\Testdata2.txt"))
                {
                    sw.WriteLine(message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path + @"\Testdata2.txt"))
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
