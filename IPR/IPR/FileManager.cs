using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR
{
    class FileManager
    {

        string dir { get; set; }

        public FileManager()
        {
            dir = @"C:\ClientData";
        }

        public string createDir(string clientID)
        {
            string localDir = dir + @"\" + clientID;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (!Directory.Exists(localDir))
            {
                Directory.CreateDirectory(localDir);
            }
            return localDir;
        }

        public List<string> selectDir()
        {
            string[] directories = Directory.GetDirectories(dir);
            List<string> nameDirs = new List<string>();

            foreach (string directorie in directories)
            {
                nameDirs.Add(directorie.Replace(dir + @"\", ""));
            }

            return nameDirs;
        }

        public List<string> selectFile(string chosenDir)
        {
            string[] files = Directory.GetFiles(dir + @"\" + chosenDir);
            List<string> nameFiles = new List<string>();

            foreach (string file in files)
            {
                nameFiles.Add(file);
            }

            return nameFiles;
        }


        public void WriteToFile(string message, string dir)
        {
            
            using (StreamWriter sr = File.AppendText(dir))
            {
                sr.WriteLine(message);
                sr.Close();
            }
        }

        public string creatFile(string chosenDir)
        {
            string filepath = chosenDir + @"\" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
            File.Create(filepath);
            return filepath;
        }


        public string ReadFile(string chosenDir, string chosenFile)
        {
            string dir = this.dir + @"\" + chosenDir + @"\" + chosenFile;
            string s;
            string packet = "";
            using (StreamReader sr = File.OpenText(dir))
            {
                s = sr.ReadLine();
                while (s != null)
                {
                    s = sr.ReadLine();
                    packet = packet + s + "-";
                }
                return packet;
            }

        }
    }
}
