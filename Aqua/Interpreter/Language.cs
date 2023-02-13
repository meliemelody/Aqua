using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqua.Interpreter
{
    /*
     * AquaLang, (c) 2023, ipluxteamx
     * 
     * This is a custom interpretor
     * for the Aqua System.
     * 
     * Made in 0.3.0
     */

    public class Language
    {
        public Language() { }

        static void Main(string filePath)
        {
            string[] searchText = new string[] {
                "write "
            };

            Console.WriteLine(FindInFile(filePath, searchText[0]));
        }

        static string FindInFile(string filePath, string f)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    int startIndex = line.IndexOf(f);
                    if (startIndex != -1)
                    {
                        int endIndex = line.IndexOf(";", startIndex);
                        if (endIndex != -1)
                        {
                            string text = line.Substring(startIndex + f.Length, endIndex - startIndex - f.Length);
                            return text;
                        }
                    }
                }
            }

            return "Command not found";
        }
    }
}
