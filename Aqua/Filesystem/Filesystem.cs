using Aqua.Commands;
using System;
using term = Aqua.Terminal.Terminal;

namespace Aqua.Filesystem
{
    public class Filesystem : Command
    {
        public Filesystem(String name) : base(name) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
                case "free":
                    try
                    {
                        var availableSpace = Kernel.fs.GetAvailableFreeSpace(@"0:\");

                        Console.ForegroundColor = ConsoleColor.Gray;
                        return "Available free space : " + availableSpace;
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "type":
                    try
                    {
                        var fs_type = Kernel.fs.GetFileSystemType(@"0:\");

                        Console.ForegroundColor = ConsoleColor.Gray;
                        return "File System type : " + fs_type;
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "list":
                    try
                    {
                        var fList = System.IO.Directory.GetFiles(Kernel.currentDirectory);
                        var dList = System.IO.Directory.GetDirectories(Kernel.currentDirectory);

                        if (args[1] == "d")
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            foreach (var directory in dList)
                            {
                                Console.WriteLine("  - " + directory);
                            }
                        }
                        else if (args[1] == "f")
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            foreach (var file in fList)
                            {
                                Console.WriteLine("  - " + file);
                            }
                        }
                        else if (args[1] == "a" || args[1] == null || args[1] == "")
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            foreach (var directory in dList)
                            {
                                Console.WriteLine("  - " + directory);
                            }

                            Console.ForegroundColor = ConsoleColor.White;
                            foreach (var file in fList)
                            {
                                Console.WriteLine("  - " + file);
                            }
                        }
                        else
                        {
                            return term.DebugWrite("Invalid argument.", 4);
                        }

                        return null;
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "cd":
                    try
                    {
                        string[] directories = System.IO.Directory.GetDirectories(Kernel.currentDirectory);

                        foreach (var dir in directories)
                        {
                            if (dir == args[1])
                            {
                                term.DebugWrite("Successfully changed the directory to : " + dir + ".\n", 2);
                                System.IO.Directory.SetCurrentDirectory(dir);

                                Kernel.currentDirectory += (dir + "\\");
                            }
                        }

                        return null;
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                default:
                    return term.DebugWrite("Invalid argument.", 4);
            }
        }
    }
}
