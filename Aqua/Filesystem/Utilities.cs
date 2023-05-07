using Aqua.Commands;
using System;
using System.IO;
using System.Text;
using io = System.IO;
using term = Aqua.Terminal.Screen;

namespace Aqua.Filesystem
{
    public class Utilities
    {
        public static string ReadLine(string file, int line)
        {
            string[] lines = io.File.ReadAllLines(file);
            return lines[line];
        }

        // USELESS
        public static bool WriteLine(string file, string content, bool append)
        {
            try
            {
                if (!io.File.Exists(file)) io.File.Create(file);
                TextWriter fileOpen = new StreamWriter(file, append);

                fileOpen.WriteLine(content);
                fileOpen.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
