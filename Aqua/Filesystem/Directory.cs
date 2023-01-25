using Aqua.Commands;
using System;
using System.Linq;
using io = System.IO;
using term = Aqua.Terminal.Terminal;

namespace Aqua.Filesystem
{
    public class Directory : Command
    {
        public Directory(String name) : base(name) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
                case "make":
                    try
                    {
                        io.Directory.CreateDirectory(@"0:\" + Kernel.currentDirectory + string.Join(' ', args.Skip(1)));

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
                        if (io.Directory.Exists(@"0:\" + Kernel.currentDirectory + string.Join(' ', args.Skip(1))))
                        {
                            io.Directory.Delete(@"0:\" + Kernel.currentDirectory + string.Join(' ', args.Skip(1)), true);

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
