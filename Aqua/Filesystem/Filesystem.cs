using Aqua.Commands;
using System;
using io = System.IO;
using term = Aqua.Terminal.Screen;
using System.Linq;
using System.IO;
using Aqua.Interface;
using Aqua.Miscellaneous;

namespace Aqua.Filesystem
{
    public class File : Command
    {
        public File(String name, String description)
            : base(name, description) { }

        public override string Execute(string[] args)
        {
            var file = string.Join(" ", args, 1, args.Length - 1);

            switch (args[0])
            {
                case "make" or "mk":
                    try
                    {
                        io.File.Create(Kernel.currentDirectory + file);

                        // response = "The file \"" + args[1] + "\" has been successfully created.";
                        return term.DebugWrite(
                            "The file \"" + file + "\" has been successfully created.",
                            2
                        );
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "delete" or "del":
                    try
                    {
                        if (io.File.Exists(Kernel.currentDirectory + file))
                        {
                            io.File.Delete(Kernel.currentDirectory + file);

                            // response = "The file \"" + args[1] + "\" has been successfully deleted.";
                            return term.DebugWrite(
                                $"The file \"{args[0]}\" has been successfully moved over to \"{args[1]}\".",
                                2
                            );
                        }
                        else
                        {
                            return term.DebugWrite(
                                "The file you have specified does not exist.",
                                4
                            );
                        }
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "move" or "mv":
                    try
                    {
                        //io.File.Move(args[0], args[1]);
                        var oldFileContents = io.File.ReadAllLines(
                            Kernel.currentDirectory + args[0]
                        );
                        io.File.Delete(Kernel.currentDirectory + args[0]);

                        io.File.Create(Kernel.currentDirectory + args[1]);
                        io.File.WriteAllLines(Kernel.currentDirectory + args[1], oldFileContents);

                        return term.DebugWrite(
                            $"The file \"{args[0]}\" has been successfully copied over to \"{args[1]}\".",
                            2
                        );
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "copy" or "cp":
                    try
                    {
                        //io.File.Move(args[0], args[1]);
                        var fileContents = io.File.ReadAllLines(Kernel.currentDirectory + args[0]);

                        io.File.Create(Kernel.currentDirectory + args[1]);
                        io.File.WriteAllLines(Kernel.currentDirectory + args[1], fileContents);

                        return term.DebugWrite("This file has been successfully moved over.", 2);
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "write" or "wrt":
                    try
                    {
                        //io.File.WriteAllText(Kernel.currentDirectory + args[1], args[2]);
                        //FileUtilities.WriteLines(Kernel.currentDirectory + args[1], args[2]);
                        string path = Kernel.currentDirectory + file;

                        if (args[2] == "true")
                            Utilities.WriteLine(path, args[3], true);
                        else
                            Utilities.WriteLine(path, args[3], false);

                        return term.DebugWrite(
                            "The file \""
                                + file
                                + "\" is successfully storing the data : \""
                                + args[3]
                                + "\".",
                            2
                        );
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "read" or "rd":
                    try
                    {
                        var path = Kernel.currentDirectory + file;

                        //var fileOpen = io.File.OpenRead(path);
                        string[] lines = io.File.ReadAllLines(path);

                        Console.ForegroundColor = ConsoleColor.Gray;
                        //return "\"" + io.File.ReadAllText(Kernel.currentDirectory + args[1]) + "\"";

                        foreach (var line in lines)
                        {
                            Console.WriteLine("   " + line);
                        }

                        return null;
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "encrypt" or "ec":
                    try
                    {
                        if (Kernel.isRoot) {
                            var path = Kernel.currentDirectory + file;
                            var fileC = io.File.ReadAllText(path);

                            var encrypted = Decryption.Encrypt(fileC);
                            io.File.WriteAllText(path, encrypted);

                            return term.DebugWrite(
                                "The file \""
                                    + file
                                    + "\" is successfully encrypted.",
                                2
                            );
                        }
                        else
                        {
                            return term.DebugWrite(
                                "You are not a \"root\" user, please log in using root credentials.",
                                4
                            );
                        }
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "decrypt" or "dc":
                    try
                    {
                        if (Kernel.isRoot)
                        {
                            var path = Kernel.currentDirectory + file;
                            var fileC = io.File.ReadAllText(path);

                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write("Input your password : ");

                            Console.ForegroundColor = ConsoleColor.Black;
                            String input = Console.ReadLine();

                            var password = Decryption.Decrypt(Utilities.ReadLine(@"0:\AquaSys\Login\Password.acf", 0));
                            if (input != password) return term.DebugWrite(
                                "You did not enter the correct password, aborting the decryption.",
                                4
                            );

                            var decrypted = Decryption.Decrypt(fileC);
                            io.File.WriteAllText(path, decrypted);

                            return term.DebugWrite(
                                "The file \""
                                    + file
                                    + "\" is successfully decrypted.",
                                2
                            );
                        }
                        else
                        {
                            return term.DebugWrite(
                                "You are not a \"root\" user, please log in using root credentials.",
                                4
                            );
                        }
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case null:
                    return term.DebugWrite("Invalid argument.", 4);

                default:
                    return term.DebugWrite("Invalid argument.", 4);
            }
        }
    }

    public class Directory : Command
    {
        public Directory(String name, String description)
            : base(name, description) { }

        public override string Execute(string[] args)
        {
            // debugging
            /*Console.WriteLine(args[0]);

            Console.WriteLine(args[1]);
            Console.WriteLine(Kernel.currentDirectory);

            Console.WriteLine(Kernel.currentDirectory + args[1]);*/

            var directory = string.Join(" ", args, 1, args.Length - 1);

            // Console.WriteLine(directory);

            switch (args[0])
            {
                case "make" or "mk":
                    try
                    {
                        //Kernel.fs.CreateDirectory(Kernel.currentDirectory + string.Join(' ', args.Skip(1)));
                        System.IO.Directory.CreateDirectory(Kernel.currentDirectory + directory);

                        // response = "The file \"" + args[1] + "\" has been successfully created.";
                        return term.DebugWrite(
                            "The directory \"" + directory + "\" has been successfully created.",
                            2
                        );
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "delete" or "del":
                    try
                    {
                        if (System.IO.Directory.Exists(Kernel.currentDirectory + directory))
                        {
                            if (directory == "AquaSys" && Kernel.currentDirectory == "0:\\")
                            {
                                if (Kernel.isRoot != true)
                                    return term.DebugWrite(
                                        "You are not a \"root\" user, please log in using root credentials.",
                                        4
                                    );
                            }

                            System.IO.Directory.Delete(Kernel.currentDirectory + directory, true);
                            return term.DebugWrite(
                                "The directory \""
                                    + directory
                                    + "\" has been successfully deleted.",
                                2
                            );
                        }
                        else
                        {
                            return term.DebugWrite(
                                "The directory you have specified does not exist.",
                                4
                            );
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

    public class Filesystem : Command
    {
        public Filesystem(String name, String description)
            : base(name, description) { }

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
                        string[] directories = System.IO.Directory.GetDirectories(
                            Kernel.currentDirectory
                        );
                        int notTheFolder = 0;

                        foreach (var dir in directories)
                        {
                            if (dir == args[1])
                            {
                                term.DebugWrite(
                                    "Successfully changed the directory to " + dir + ".",
                                    2
                                );
                                System.IO.Directory.SetCurrentDirectory(dir);

                                Kernel.currentDirectory += dir + "\\";
                                return null;
                            }
                            else
                            {
                                notTheFolder++;
                            }
                        }

                        if (notTheFolder == directories.Length)
                        {
                            Console.Write("\n");
                            term.DebugWrite("Directory does not exist.", 4);
                        }

                        return null;
                    }
                    catch (Exception e)
                    {
                        return term.DebugWrite(e.ToString(), 4);
                    }

                case "format":
                    try
                    {
                        if (Kernel.isRoot == true)
                        {
                            /*for (int i = 0; i <= Kernel.fs.Disks[0].Partitions.Count; i++)
                                Kernel.fs.Disks[0].FormatPartition(i, "FAT32");*/
                            var size = Kernel.fs.Disks[0].Size;

                            if (args[1] == null)
                            {
                                Kernel.fs.Disks[0].DeletePartition(0);
                                Kernel.fs.Disks[0].CreatePartition(size);
                            }
                            else if (int.Parse(args[1]) <= size)
                            {
                                Kernel.fs.Disks[0].DeletePartition(0);
                                Kernel.fs.Disks[0].CreatePartition(int.Parse(args[1]));
                            }

                            Kernel.fs.Disks[0].FormatPartition(0, "FAT32");
                            term.DebugWrite("Formatted the drive, rebooting in 2 seconds...", 1);

                            Cosmos.HAL.Global.PIT.Wait(2000);
                            Cosmos.System.Power.Reboot();
                            return null;
                        }
                        else
                        {
                            return term.DebugWrite(
                                "You are not a \"root\" user, please log in using root credentials.",
                                4
                            );
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
