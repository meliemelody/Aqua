using Aqua.Commands;
using Aqua.Terminal.Login;

using Cosmos.HAL.Drivers.PCI.Audio;
using Cosmos.System;
using Cosmos.System.Audio;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;

using System;
using Console = System.Console;
using Sys = Cosmos.System;

using term = Aqua.Terminal.Terminal;
using Aqua.ErrorHandler;
using Cosmos.System.Graphics;
using System.Drawing;

namespace Aqua
{
    public class Kernel : Sys.Kernel
    {
        private Manager _commandManager = new Manager();
        public static CosmosVFS fs;

        public static bool isRoot, guiStarted;

        public static String currentDirectory = @"0:\";
        public static String currentAccount;

        public static AudioMixer mixer;
        public static AC97 driver;

        public static AudioManager audioManager;

        public static Canvas canvas;

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
                    CrashHandler.CrashHandle(ex);
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
            guiStarted = false;
            try
            {
                fs = new CosmosVFS();
                VFSManager.RegisterVFS(fs);
            }
            catch (Exception ex)
            {
                CrashHandler.CrashHandle(ex);
            }

            // Give time for the initialization to succeed
            Cosmos.HAL.Global.PIT.Wait(250);
            Console.Clear();

            if (System.IO.File.Exists("0:\\Setup\\FirstRun.acf") && System.IO.File.ReadAllText("0:\\Setup\\FirstRun.acf") == "true")
                FirstRun();

            Cosmos.HAL.Global.PIT.Wait(750);
            LoginSystem.Start();
        }

        public static void TitleBar()
        {
            int xCord = Console.CursorLeft;
            int yCord = Console.CursorTop;

            Console.SetCursorPosition(0, 0);

            Console.BackgroundColor = ConsoleColor.DarkCyan;
        }

        protected override void Run()
        {
            if (guiStarted == true)
            {
                canvas.Display();
            }
            else
            {
                // TitleBar();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(currentDirectory);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("$=> ");

                Console.ForegroundColor = ConsoleColor.White;

                String response;
                String input = Console.ReadLine();

                response = this._commandManager.ProcessInput(input);

                Console.WriteLine("  " + response + "\n");
            }
        }

        public static void StartUp()
        {
            try
            {
                term.DebugWrite("Setting up the network...", 6);
                // Setup the network / Generate an IP address dynamically
                Network.Network.Setup();

                if (!VMTools.IsVMWare)
                {
                    term.DebugWrite("Setting up the audio drivers...", 6);

                    driver = AC97.Initialize(bufferSize: 4096);
                    mixer = new AudioMixer();

                    audioManager = new AudioManager()
                    {
                        Stream = mixer,
                        Output = driver
                    };

                    audioManager.Enable();
                }

                Cosmos.HAL.Global.PIT.Wait(750);
                Console.Clear();
            }
            catch (Exception e)
            {
                CrashHandler.CrashHandle(e);
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

            if (!VMTools.IsVMWare)
                Sounds.Sounds.StartupSound();
        }
    }
}
