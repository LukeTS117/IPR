using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR.AstrandTest
{
    class HistoricData: IAstrandData
    {

        string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public int heartRate;
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

        public void ReadFile()
        {

            string path = this.dir + @"\TestData.txt";
            string s;
            

            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (s.Contains("<HR>"))
                        {
                            heartRate = Int32.Parse(s.Substring(4));
                            
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("This file does not exist");
            }


        }

        public int GetHeartFrequency()
        {
            return heartRate;
        }

        public int PushRotation()
        {
            throw new NotImplementedException();
        }
    }
}
