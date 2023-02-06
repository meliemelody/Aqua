using Cosmos.System.FileSystem.VFS;
using Cosmos.System.FileSystem;
using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using System.IO;

namespace Rescue
{
    // IN PROGRESS
    public class Kernel : Sys.Kernel
    {
        CosmosVFS fs;
        protected override void BeforeRun()
        {
            // Console.SetWindowSize(90, 60);
            Console.Clear();
            
            fs = new CosmosVFS();
            VFSManager.RegisterVFS(fs);
        }

        protected override void Run()
        {
            // Show the available options [format, delete the login info, etc...]
            ShowOptions();

            // Get the system status [installed or not installed].
            GetSystemStatus();

            // Set the foreground to gray, then shows the "What do you need to do today ?" message.
            // Unintentional reference to Windows.
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("What do you need to do today ? : [");

            // Save the cursor position for the "[value]" thing.
            // Then resume it when the "]" is written.
            var conPos = Console.GetCursorPosition();
            Console.SetCursorPosition(conPos.Left + 1, conPos.Top);
            Console.WriteLine("]");

            Console.SetCursorPosition(conPos.Left, conPos.Top);
            Console.ForegroundColor = ConsoleColor.White;
            var input = Console.ReadKey();

            // Replaces the ifs and elses.
            // Then check for the input.
            switch (input.Key)
            {
                case ConsoleKey.A:
                    Console.SetCursorPosition(0, conPos.Top + 1);

                    /*for (int i = 0; i <= fs.Disks[0].Partitions.Count; i++)
                    {
                        fs.Disks[0].FormatPartition(i, "FAT32");
                        Console.WriteLine($"[OK] | Successfully formated the partition {i}.");
                    }*/
                    try
                    {
                        fs.Disks[0].Clear();
                        fs.Disks[0].CreatePartition(fs.Disks[0].Size);

                        // In case the user wants to delete their current installation.
                        conPos = Console.GetCursorPosition();

                        Console.WriteLine("Do you want to delete your current installation ? [y/n] : [");

                        Console.SetCursorPosition(conPos.Left + 1, conPos.Top);
                        Console.WriteLine("]");

                        Console.SetCursorPosition(conPos.Left, conPos.Top);
                        var deleteInput = Console.ReadKey().Key;

                        Console.SetCursorPosition(conPos.Left, conPos.Top + 1);
                        switch (deleteInput)
                        {
                            case ConsoleKey.Y:
                                if (Directory.Exists(@"0:\AquaSys"))
                                {
                                    try
                                    {
                                        Directory.Delete(@"0:\AquaSys", true);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine(ex.ToString());
                                        break;
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Current installation not found, aborting this step.");
                                    break;
                                }
                                break;

                            case ConsoleKey.N:
                                break;

                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("You have entered an incorrect argument, aborting this step.");

                                break;
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Successfully formated the partitions.");
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.ToString());
                    }

                    break;

                case ConsoleKey.B:
                    Console.SetCursorPosition(0, conPos.Top + 1);

                    if (Directory.Exists(@"0:\AquaSys\Login"))
                    {
                        try
                        {
                            Directory.Delete(@"0:\AquaSys\Login", true);
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.ToString());
                            break;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Login information is not present on your system.");
                        break;
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully deleted the login information.");
                    break;

                case ConsoleKey.C:
                    // This utility is in major development.
                    // It might not be useable for now.
                    Console.SetCursorPosition(0, conPos.Top + 1);

                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Which disk do you want to store your backup in ? [ex : 1:\\] : ");

                    Console.ForegroundColor = ConsoleColor.White;
                    var saveInput = Console.ReadLine();

                    var directories = Directory.GetDirectories("0:\\");
                    foreach (var directory in directories)
                    {
                        Console.WriteLine(directory);
                    }

                    break;

                default:
                    Console.SetCursorPosition(0, conPos.Top + 1);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You have entered an incorrect command.");

                    break;
            }

            Cosmos.HAL.Global.PIT.Wait(1000);
            Console.Clear();
        }

        private void GetSystemStatus()
        {
            var exists = System.IO.File.Exists(@"0:\AquaSys\Setup\FirstRun.acf");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("System Status : ");

            if (exists)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Installed");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not installed");
            }
        }

        private static void ShowOptions()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Aqua System | Rescue Disk | v0.1.1");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(
                @"
| A | Disk Format Utility
| B | Credential Wiper
| C | Backup and Restore
");
        }
    }
}
