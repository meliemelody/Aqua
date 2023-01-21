using System;
using Sys = Cosmos.System;
using Aqua.Commands;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Aqua.Network;
using CosmosFtpServer;
using term = Aqua.Terminal.Terminal;
using Cosmos.System.Audio;
using Cosmos.System.Audio.IO;
using Cosmos.HAL.Drivers.PCI.Audio;

namespace Aqua 
{
    public class Kernel : Sys.Kernel 
    {
        private Manager _commandManager = new Manager();
        public static CosmosVFS fs = new CosmosVFS();

        public static bool isRoot;

        public static String currentDirectory = @"0:\";

        public static AudioMixer mixer = new AudioMixer();
        public static AC97 driver = AC97.Initialize(bufferSize: 4096);

        public AudioManager audioManager = new AudioManager()
        {
            Stream = mixer,
            Output = driver
        };

        public void FirstRun()
        {
            Console.WriteLine("Setting up things for you...\n");

            if (!System.IO.Directory.Exists(@"0:\Setup"))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(@"0:\Setup");

                    term.DebugWrite("Successfully made the Setup directory.", 1);

                    Cosmos.HAL.Global.PIT.Wait(500);
                }
                catch (Exception ex)
                {
                    term.DebugWrite(ex.ToString() + " | Rebooting...", 5);

                    Cosmos.HAL.Global.PIT.Wait(500);
                    Cosmos.System.Power.Reboot();
                }
            }

            try
            {
                System.IO.File.Create(@"0:\Setup\FirstRun.acf");
                System.IO.File.WriteAllText("0:\\Setup\\FirstRun.acf", "true");

                term.DebugWrite("Successfully made the FirstRun.acf file.", 1);
            }
            catch (Exception ex)
            {
                term.DebugWrite(ex.ToString(), 4);
            }

            Cosmos.HAL.Global.PIT.Wait(750);
            Console.Clear();
        }

        protected override void BeforeRun() 
        {
            try
            {
                VFSManager.RegisterVFS(fs);
                audioManager.Enable();
            }
            catch (Exception ex)
            {
                term.DebugWrite(ex.ToString() + " | Rebooting...", 5);

                Cosmos.HAL.Global.PIT.Wait(2500);
                Cosmos.System.Power.Reboot();
            }

            // Give time for the initialization to succeed
            Cosmos.HAL.Global.PIT.Wait(250);
            Console.Clear();

            if (System.IO.File.Exists("0:\\Setup\\FirstRun.acf") && System.IO.File.ReadAllText("0:\\Setup\\FirstRun.acf") == "true")
                FirstRun();

            Cosmos.HAL.Global.PIT.Wait(750);
            Aqua.Terminal.Login.LoginSystem.Start();
        }

        protected override void Run()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(Terminal.Login.LoginSystem.username + " | " + currentDirectory);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" $=> ");

            Console.ForegroundColor = ConsoleColor.White;

            String response;
            String input = Console.ReadLine();

            response = this._commandManager.ProcessInput(input);

            Console.WriteLine("  " + response + "\n");
        }

        public static void StartUp()
        {
            try
            {
                // Setup the network / Generate an IP address dynamically
                Network.Network.Setup();
            }
            catch (Exception e)
            {
                term.DebugWrite(e.ToString(), 4);
            }

            // Introduction
            String welcome = "Welcome to the world of Aqua.";

            // Center the cursor position
            Console.SetCursorPosition((Console.WindowWidth / 2) - (welcome.Length / 2) - 2, Console.CursorTop);

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(welcome + "\n");

            String versionInfo = @"Aqua | Version 0.1.1 | Codename 'Paris'";
            String[] devStatus =
            {
                "Current development status : ",
                "Alpha | Milestone 1\n"
            };

            Console.SetCursorPosition((Console.WindowWidth / 2) - (versionInfo.Length / 2) - 2, Console.CursorTop);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(versionInfo);

            Console.SetCursorPosition((Console.WindowWidth / 2) - ((devStatus[0].Length + devStatus[1].Length) / 2) - 2, Console.CursorTop);
            Console.Write(devStatus[0]);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(devStatus[1]);

            Sounds.Sounds.StartupSound();
        }
    }
}
