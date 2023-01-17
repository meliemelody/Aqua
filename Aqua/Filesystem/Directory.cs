using Aqua.Commands;
using Sys = Cosmos.System;
using System;
using io = System.IO;
using Aqua.Terminal;
using term = Aqua.Terminal.Terminal;
using System.Linq;

namespace Aqua.Filesystem
{
    public class Directory : Command
    {
        public Directory (String name) : base (name) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
                case "make":
                    try
                    {
                        io.Directory.CreateDirectory(Kernel.currentDirectory + string.Join(' ', args.Skip(1)));

                        // response = "The file \"" + args[1] + "\" has been successfully created.";
                        return term.DebugWrite("The directory \"" + string.Join(' ', args.Skip(1)) + "\" has been successfully created.", 2);
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "del":
                    try
                    {
                        if (io.Directory.Exists(Kernel.currentDirectory + string.Join(' ', args.Skip(1))))
                        {
                            io.Directory.Delete(Kernel.currentDirectory + string.Join(' ', args.Skip(1)), true);

                            // response = "The file \"" + args[1] + "\" has been successfully deleted.";
                            return term.DebugWrite("The directory \"" + string.Join(' ', args.Skip(1)) + "\" has been successfully deleted.", 2);
                        }
                        else
                        {
                            return term.DebugWrite("The directory you have specified does not exist.", 4);
                        }
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
