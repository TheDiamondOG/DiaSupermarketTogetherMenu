using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketTogetherKacker.tools
{
    internal class Filestuff
    {
        public void MakeFolder(string path)
        {
            if (File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

        }
        public void WriteToFile(string filename, string text)
        {
            string fileName = "diamenu/" + filename;
            if (!Directory.Exists("diamenu"))
            {
                Directory.CreateDirectory("diamenu");
            }
            File.WriteAllText(fileName, text);
        }

        public void AppendFile(string filename, string text)
        {
            string fileName = "diamenu/" + filename;
            if (!Directory.Exists("diamenu"))
            {
                Directory.CreateDirectory("diamenu");
            }
            if (File.Exists(fileName))
            {
                File.AppendAllText(fileName, "\n"+text);
            }
            else
            {
                WriteToFile(filename, text);
            }
            
        }
    }
}
