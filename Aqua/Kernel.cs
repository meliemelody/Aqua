using Aqua.Commands;
using Aqua.Terminal.Accounts;

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
using Cosmos.System.Graphics.Fonts;

using Cosmos.Core.Memory;
using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System.Threading;
using System.Diagnostics;
using Aqua.Filesystem;
using Aqua.Miscellaneous;
using Cosmos.System.ScanMaps;
using Aqua.Commands.Executables;

namespace Aqua
{
    public class Kernel : Sys.Kernel
    {
        private Manager _commandManager = new Manager();
        public static CosmosVFS fs;

        public static bool isRoot, guiStarted;
        public static string currentDirectory = "0:\\", currentAccount;

        public static AudioMixer mixer;
        public static AC97 audioDriver;
        public static AudioManager audioManager;

        public static ConsoleColor bgColor = ConsoleColor.Black;
        public static ConsoleColor fgColor = ConsoleColor.White;

        public static Canvas canvas;

        public static string time;

        [ManifestResourceStream(ResourceName = "Aqua.Fonts.zap-ext-vga09.psf")] public static byte[] font;

        //public static PCScreenFont Font = PCScreenFont.Default;
        //public static byte[] FontVga;

        public static string version = "0.3.0";

        static (int Left, int Top) cursorPos;

        // This is the "Setup" function, executed if the run is the first run ever.
        public void FirstRun()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Setting up things for you...");

            // Detect if the emulator is NOT VirtualBox or QEMU.
            // These emulators/virtualizers cannot support the filesystem completely for now. 
            if (!VMTools.IsVirtualBox || !VMTools.IsQEMU)
            {
                // Create the AquaSys directory.
                // THIS IS OBLIGATORY FOR MOST SYSTEM APPS.
                if (!System.IO.Directory.Exists(@"0:\AquaSys"))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(@"0:\AquaSys");

                        term.DebugWrite("Successfully made the AquaSys directory.\n", 1);

                        Cosmos.HAL.Global.PIT.Wait(500);
                    }
                    catch (Exception ex)
                    {
                        CrashHandler.Handle(ex);
                    }
                }

                // Create the one-time Setup folder.
                // This folder plays a role in First Time-detecting.
                if (!System.IO.Directory.Exists(@"0:\AquaSys\Setup"))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(@"0:\AquaSys\Setup");

                        term.DebugWrite("Successfully made the Setup directory.\n", 1);

                        Cosmos.HAL.Global.PIT.Wait(500);
                    }
                    catch (Exception ex)
                    {
                        CrashHandler.Handle(ex);
                    }
                }

                // Make the FirstRun.acf file.
                try
                {
                    System.IO.File.Create(@"0:\AquaSys\Setup\FirstRun.acf");
                    System.IO.File.WriteAllText("0:\\AquaSys\\Setup\\FirstRun.acf", "true");

                    term.DebugWrite("Successfully made the FirstRun.acf file.\n", 1);
                }
                catch (Exception ex)
                {
                    term.DebugWrite(ex.ToString(), 4);
                }

                Miscellaneous.Readme.WriteReadme();
            }
            else
            {
                term.DebugWrite("Your emulator/virtualizer does not support the VFS.", 4);
            }

            Cosmos.HAL.Global.PIT.Wait(750);
            Console.Clear();
        }

        public void CheckKeyboardLayout()
        {
            if (System.IO.File.Exists(@"0:\AquaSys\Config\KeyMap.acf"))
            {
                var content = System.IO.File.ReadAllText(@"0:\AquaSys\Config\KeyMap.acf");

                switch (content)
                {
                    case "us":
                        KeyboardManager.SetKeyLayout(new US_Standard());
                        break;

                    case "fr":
                        KeyboardManager.SetKeyLayout(new FR_Standard());
                        break;

                    case "de":
                        KeyboardManager.SetKeyLayout(new DE_Standard());
                        break;
                }
            }
            else
            {
                KeyboardManager.SetKeyLayout(new US_Standard());
            }
        }

        // This function runs before the main loop, which is Run().
        // It is used to initiate drivers, the filesystem, check if this run is the first run, and set settings.
        protected override void BeforeRun()
        {
            Console.SetWindowSize(90, 60);

            var f = PCScreenFont.LoadFont(font);
            VGAScreen.SetFont(f.CreateVGAFont(), f.Height);

            try
            {
                fs = new CosmosVFS();
                VFSManager.RegisterVFS(fs);
                System.IO.Directory.SetCurrentDirectory(currentDirectory);

                // fs.Disks[0].FormatPartition(0, "FAT32");

                // Check for format input.
                // Note : Replaced by the Disk Utility/Recovery Disk
                // FormatCheck();
            }
            catch (Exception ex)
            {
                CrashHandler.Handle(ex);
            }

            // Give time for the initialization to succeed
            Cosmos.HAL.Global.PIT.Wait(250);
            Console.Clear();

            CheckKeyboardLayout();

            if (!System.IO.File.Exists("0:\\AquaSys\\Setup\\FirstRun.acf") || System.IO.File.ReadAllText("0:\\AquaSys\\Setup\\FirstRun.acf") != "true" || fs.Disks[0].Partitions == null)
                FirstRun();

            Cosmos.HAL.Global.PIT.Wait(750);
            LoginSystem.Start();
        }

        protected override void Run()
        {
            //Console.WriteLine($"Collected : {Heap.Collect()} objects.");
            Heap.Collect();

            if (guiStarted)
            {
                Graphics.Graphics.Draw(canvas);
            }
            else
            {
                // Draw the upper bar, with the time, name of the OS and version.
                // I need to find a way to update it while still being able to use the shell, and vice-versa.
                DrawBar();

                // The main shell, source of almost all my problems.
                AquaShell();

                // Set the current colors to the specified colors.
                // Can set them using the "set" command.
                Console.BackgroundColor = bgColor;
                Console.ForegroundColor = fgColor;
            }
        }

        private void AquaShell()
        {
            // Change the text color to Dark Green.
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(currentDirectory);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("$=> ");

            // Set the current colors to the specified colors, again.
            // Can set them using the "set" command.
            Console.BackgroundColor = bgColor;
            Console.ForegroundColor = fgColor;

            // Listen for input, then respond to that input accordingly using the command manager.
            string response;
            string input = Console.ReadLine();

            response = _commandManager.ProcessInput(input);

            Console.WriteLine("  " + response + "\n");
        }

        public static void DrawBar()
        {
            //var time = DateTime.Now.ToString("HH:mm");
            var developmentStatus = "Alpha | Milestone 2";

            time = $"{Cosmos.HAL.RTC.Hour.ToString("D2")}:{Cosmos.HAL.RTC.Minute.ToString("D2")}";
            //time = DateTime.Now.ToString("t");
            cursorPos = Console.GetCursorPosition();

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < Console.WindowWidth; i++) Console.Write(' ');

            Console.SetCursorPosition(0, 0);
            Console.Write("Aqua | Version " + version);

            DrawTime(time);

            Console.SetCursorPosition(Console.WindowWidth - developmentStatus.Length, 0);
            Console.Write(developmentStatus);

            Console.SetCursorPosition(cursorPos.Left, cursorPos.Top);
            Console.BackgroundColor = bgColor;
            Console.ForegroundColor = fgColor;
        }

        public static void DrawTime(string time)
        {
            var oldCurPos = Console.GetCursorPosition();

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition((Console.WindowWidth / 2 + 1) - (time.Length / 2) - 2, 0);
            Console.Write(time);

            Console.SetCursorPosition(oldCurPos.Left, oldCurPos.Top);
        }

        public static void StartUp()
        {
            try
            {
                term.DebugWrite("Setting up the network...", 1);
                // Setup the network / Generate an IP address dynamically
                Network.Network.Setup();

                // Network.Network.DownloadFile("http://info.cern.ch/hypertext/WWW/TheProject.html", @"0:\File.txt");

                if (!VMTools.IsVMWare)
                {
                    term.DebugWrite("Setting up the audio drivers...", 0);

                    audioDriver = AC97.Initialize(bufferSize: 4096);
                    mixer = new AudioMixer();

                    audioManager = new AudioManager()
                    {
                        Stream = mixer,
                        Output = audioDriver
                    };

                    audioManager.Enable();
                }

                Cosmos.HAL.Global.PIT.Wait(750);
                Console.Clear();
            }
            catch (Exception e)
            {
                CrashHandler.Handle(e);
            }

            DrawBar();
            Console.SetCursorPosition(0, 1);

            // Introduction
            string welcome = "Welcome to the world of Aqua.";

            Console.WriteLine();
            // Center the cursor position
            Console.SetCursorPosition((Console.WindowWidth / 2) - (welcome.Length / 2) - 1, Console.CursorTop);

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(welcome + "\n");

            string versionInfo = @"Aqua | Version " + version + " | Codename 'Paris'";
            string[] devStatus =
            {
                "Current development status : ",
                "Alpha | Milestone 2\n"
            };

            Console.SetCursorPosition((Console.WindowWidth / 2) - (versionInfo.Length / 2) - 1, Console.CursorTop);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(versionInfo);

            Console.SetCursorPosition((Console.WindowWidth / 2) - ((devStatus[0].Length + devStatus[1].Length) / 2) - 1, Console.CursorTop);
            Console.Write(devStatus[0]);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(devStatus[1]);

            cursorPos = Console.GetCursorPosition();

            if (!VMTools.IsVMWare)
                Sounds.Sounds.LogonSound();

            // Surprise tool that will help us later.
            /*Random rnd = new Random();

            int number = rnd.Next(250);
            string decryptedKey = KeyDecryption.DecryptKey(
                ProductKeys.EncryptedKeys[number]
            );

            Console.WriteLine(decryptedKey);*/
        }
    }
}
