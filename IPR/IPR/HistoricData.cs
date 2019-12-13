using IPR.AstrandTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace IPR
{
    class HistoricData: IAstrandData
    {

        string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);\
        List<int> hrList;
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

        public List<int> ReadFile()
        {

            string path = this.dir + @"\TestData.txt";
            string s;
            this.hrList = new List<int>();

            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (s.Contains("<HR>"))
                        {
                           this.hrList.Add(Int32.Parse(s.Substring(4)));
                        }                            
                    }
                    return hrList;
                }
            }
            else
            {
                Console.WriteLine("This file does not exist");
                return null;
            }


        }

        public int GetHeartFrequency()
        {
            if(hrList != null)
            {
                return 1;
            }
            return 0;
        }

        public int PushRotation()
        {
            throw new NotImplementedException();
        }
    }
}
