using Aqua.Terminal;
using System;
using System.IO;

namespace Aqua.Miscellaneous
{
    public class TEDEditor
    {
        public static string Run(string[] args)
        {
            try
            {
                Load(args);
            }
            catch (Exception ex)
            {
                Terminal.Terminal.DebugWrite(ex.ToString(), 4);
            }
            return null;
        }

        public static void Load(string[] args)
        {
            string path = "";
            for (int i = 0; i < args.Length; i++)
            {
                // Debugging only.
                // This is only used for debugging purposes.
                Console.WriteLine(args[i]);

                if (args[i] == "-h") { }
                else if (path == "")
                {
                    path = args[i];

                    if (!path.Contains("\\"))
                    {
                        // Set the path to (for example) : "0:\System\" + "file.txt".
                        path = $"{Kernel.currentDirectory}{path}";

                        // Replace all the "\\" to "\".
                        path = path.Replace("\\\\", "\\");
                    }

                    Console.WriteLine($"Path : {path}");
                }
                else
                    Terminal.Terminal.DebugWrite("Unknown argument : " + args[i], 4);
            }
        }
    }
}