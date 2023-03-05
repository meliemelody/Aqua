using Cosmos.Core;
using System;
using System.IO;
using System.Linq;

namespace Aqua.Miscellaneous
{
    /*
     * Project Avalanche
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
                    {
                        File.Create(path);
                        File.WriteAllText(path, "This is the default TED Editor message.\nTo remove every line, simply press CTRL+K.");
                    }

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
            int defaultYPos = 2;

            // Draw the information and title bar.
            DrawUpperBar(0, 0, path, oldFC, fileContents);

            Console.SetCursorPosition(0, defaultYPos);
            Console.Write(fileContents);

            int cursorX = Console.CursorLeft, cursorY = Console.CursorTop;
            Console.SetCursorPosition(cursorX, cursorY);

            for (; ; )
            {
                // Draw the information and title bar.
                DrawUpperBar(0, 0, path, oldFC, fileContents);

                Console.SetCursorPosition(0, defaultYPos);
                Console.Write(fileContents);

                Console.SetCursorPosition(cursorX, cursorY);

                var input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.Enter)
                {
                    fileContents += "\n";
                    cursorX = 0;
                    cursorY++;
                }
                else if (input.Key == ConsoleKey.Backspace)
                {
                    if (fileContents.Length != 0 || cursorX > 1)
                    {
                        // Split the input string into lines
                        string[] lines = fileContents.Split('\n');

                        // Determine the line where the new text should be inserted
                        string lineToInsert = lines[cursorY - 2];
                        string newLine = lineToInsert.Substring(0, cursorX-1) + lineToInsert.Substring(cursorX);

                        lines[cursorY - 2] = newLine;
                        fileContents = string.Join('\n', lines);

                        if (Console.CursorLeft != 1)
                        {
                            cursorX--;
                            Console.Write("  ");

                            cursorX--;
                            Console.Write(' ');

                            cursorX++;
                            Console.Write(' ');
                        }
                        else
                        {
                            cursorX = Console.WindowWidth - 1;
                            cursorY--;
                        }
                    }
                }
                else if (input.Key == ConsoleKey.LeftArrow)
                {
                    if (cursorX > 0)
                    {
                        cursorX--;
                        Console.CursorLeft--;
                    }
                }
                else if (input.Key == ConsoleKey.RightArrow)
                {
                    if (cursorX < fileContents.Split('\n')[cursorY-2].Length)
                    {
                        cursorX++;
                        Console.CursorLeft++;
                    }
                }
                else if (input.Key == ConsoleKey.UpArrow)
                {
                    if (cursorY > defaultYPos)
                    {
                        cursorY--;
                        Console.CursorTop--;

                        if (cursorX > fileContents.Split('\n')[cursorY-2].Length)
                            cursorX = fileContents.Split('\n')[cursorY-2].Length;
                    }
                }
                else if (input.Key == ConsoleKey.DownArrow)
                {
                    if (cursorY < fileContents.Split('\n').Length+1)
                    {
                        cursorY++;

                        if (cursorX > fileContents.Split('\n')[cursorY-2].Length)
                            cursorX = fileContents.Split('\n')[cursorY-2].Length;
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
                        fileContents = "";
                        cursorX = 0;
                        cursorY = 0;
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
                {
                    // Split the input string into lines
                    string[] lines = fileContents.Split('\n');

                    // Determine the line where the new text should be inserted
                    string lineToInsert = lines[cursorY-2];
                    string newLine = lineToInsert.Substring(0, cursorX) + input.KeyChar.ToString() + lineToInsert.Substring(cursorX);

                    lines[cursorY-2] = newLine;
                    fileContents = string.Join('\n', lines);

                    cursorX++;
                }
            }
        }
    }
}