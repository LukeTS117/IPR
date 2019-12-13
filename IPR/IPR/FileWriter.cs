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

        string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
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
