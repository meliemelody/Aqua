using Aqua.Terminal;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Aqua.Miscellaneous
{
    /*
     * Project Renaissance
     * Source code by ipluxteamx.
     * Software for Aqua System 0.2.1
     * 
     * Version 0.1.1
     * Milestone 2
     */
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
            string path = null;

            if (!Directory.Exists("0:\\AquaSys\\Config"))
                Directory.CreateDirectory("0:\\AquaSys\\Config");

            if (!Directory.Exists("0:\\AquaSys\\Config\\TED"))
                Directory.CreateDirectory("0:\\AquaSys\\Config\\TED");

            for (int i = 0; i < args.Length; i++)
            {
                // Debugging only.
                // This is only used for debugging purposes.
                // Console.WriteLine(args[i]);

                if (args[i] == "-h") { }
                else if (path == null)
                {
                    path = args[i];

                    if (!path.Contains("\\"))
                    {
                        // Set the path to (for example) : "0:\AquaSys\" + "file.txt".
                        path = $"{Kernel.currentDirectory}{path}";

                        // Replace all the "\\" to "\".
                        path = path.Replace("\\\\", "\\");
                    }
                    if (!File.Exists(path))
                        File.WriteAllText(path, "This is the default TED Editor message.\nTo remove every line, simply press CTRL+K.");

                    Console.Clear();
                    Editor(path);

                    // Console.WriteLine($"Path : {path}");
                }
                else
                    Terminal.Terminal.DebugWrite("Unknown argument : " + args[i], 4);
            }
        }

        public static bool DrawUpperBar(int x, int y, string path, string oldC, string newC)
        {
            try
            {
                Console.SetCursorPosition(x, y);

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                for (int screenX = x; screenX <= Console.WindowWidth; screenX++)
                {
                    Console.Write(' ');
                }

                string programName = "Project Avalanche | version 0.1.1";
                Console.SetCursorPosition(x, y);
                Console.Write(programName);

                Console.SetCursorPosition(Console.WindowWidth - path.Length, y);
                Console.Write(path);

                if (newC != oldC)
                    Console.BackgroundColor = ConsoleColor.Red;
                else
                    Console.BackgroundColor = ConsoleColor.DarkGreen;

                Console.ForegroundColor = ConsoleColor.White;

                Console.SetCursorPosition(x, y + 1);
                for (int screenX = x; screenX <= Console.WindowWidth; screenX++)
                {
                    Console.Write(' ');
                }

                Console.SetCursorPosition(x, y + 1);
                if (newC.Length != 0)
                {
                    int chars = newC.Length;
                    chars--;
                    Console.Write("Characters : [" + chars + "]");
                }
                else
                    Console.Write("Characters : [No characters yet]");

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(x, y + 2);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Editor(string path)
        {
            string fileContents = File.ReadAllText(path), oldFC = File.ReadAllText(path);
            string toreturn = fileContents;
            int defaultYPos = 2;

            for (; ; )
            {
                // Draw the information and title bar.
                DrawUpperBar(0, 0, path, oldFC, fileContents);

                Console.SetCursorPosition(0, defaultYPos);
                Console.Write(fileContents);

                var currentCursorPos = Console.GetCursorPosition();
                Console.SetCursorPosition(currentCursorPos.Left, currentCursorPos.Top);

                var input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.Enter)
                    fileContents += "\n";
                else if (input.Key == ConsoleKey.Backspace)
                {
                    if (fileContents.Length != 0)
                    {
                        fileContents = fileContents.Remove(fileContents.Length - 1, 1);

                        if (Console.CursorLeft != 0)
                        {
                            Console.CursorLeft--;
                            Console.Write(' ');

                            Console.CursorLeft++;
                        }
                        else
                        {
                            Console.CursorTop--;
                        }
                    }
                }
                else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if (input.Key == ConsoleKey.S)
                    {
                        File.WriteAllText(path, fileContents);
                        oldFC = fileContents;
                    }
                    else if (input.Key == ConsoleKey.K)
                    {
                        fileContents = " ";
                        Console.Clear();

                        Console.SetCursorPosition(0, defaultYPos);
                    }
                    else if (input.Key == ConsoleKey.Q)
                    {
                        Console.Clear();
                        break;
                    }
                }
                else
                    fileContents += input.KeyChar;
            }
        }
    }
}