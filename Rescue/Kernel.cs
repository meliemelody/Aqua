using Cosmos.System.FileSystem.VFS;
using Cosmos.System.FileSystem;
using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;

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
            Console.ForegroundColor = ConsoleColor.Gray;
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
                    fs.Disks[0].FormatPartition(0, "FAT32");
                    Console.WriteLine($"[OK] | Successfully formated the partition 0.");

                    break;

                case ConsoleKey.B:
                    if (System.IO.Directory.Exists(@"0:\System\Login"))
                        System.IO.Directory.Delete(@"0:\System\Login");
                    else
                        break;

                    Console.SetCursorPosition(0, conPos.Top + 1);
                    Console.WriteLine("[OK] | Successfully deleted the login information.");
                    break;

                default:
                    Console.SetCursorPosition(0, conPos.Top + 1);
                    Console.WriteLine("[ERROR] | You have entered an incorrect command.");
                    break;
            }

            Cosmos.HAL.Global.PIT.Wait(1000);
            Console.Clear();
        }

        private void GetSystemStatus()
        {
            var exists = System.IO.File.Exists(@"0:\System\Setup\FirstRun.acf");

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
            Console.WriteLine("Aqua System | Rescue Disk | v0.1.0");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(
                @"
| A | Format the hard drive.
| B | Delete your login information.
");
        }
    }
}
