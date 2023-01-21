using Aqua.Commands;
using Sys = Cosmos.System;
using System;
using io = System.IO;
using Aqua.Terminal;
using term = Aqua.Terminal.Terminal;
using System.Text;
using System.IO;

namespace Aqua.Filesystem
{
    public class File : Command
    {
        public File (String name) : base (name) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
                case "make":
                    try
                    {
                        io.File.Create(@"0:\" + Kernel.currentDirectory + args[1]);

                        // response = "The file \"" + args[1] + "\" has been successfully created.";
                        return term.DebugWrite("The file \"" + args[1] + "\" has been successfully created.", 2);
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "del":
                    try
                    {
                        if (io.File.Exists(@"0:\" + Kernel.currentDirectory + args[1]))
                        {
                            io.File.Delete(@"0:\" + Kernel.currentDirectory + args[1]);

                            // response = "The file \"" + args[1] + "\" has been successfully deleted.";
                            return term.DebugWrite("The file \"" + args[1] + "\" has been successfully deleted.", 2);
                        }
                        else
                        {
                            return term.DebugWrite("The file you have specified does not exist.", 4);
                        }
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "write":
                    try
                    {
                        io.File.WriteAllText(@"0:\" + Kernel.currentDirectory + args[1], args[2]);

                        return term.DebugWrite("The file \"" + args[1] + "\" is successfully storing the data : \"" + args[2] + "\".", 2);
                    } 
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "read":
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        return "\"" + io.File.ReadAllText(@"0:\" + Kernel.currentDirectory + args[1]) + "\"";
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
