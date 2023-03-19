using System;
using term = Aqua.Terminal.Screen;

// If you see any comments that are a little too explainatory, I was just testing Github Copilot.
namespace Aqua.ErrorHandler
{
    // This class handles crashes and writes the crash information to a file.
    // It also reboots the system.
    // This class is used when something goes terribly, horribly, and catastrophically wrong.
    public static class CrashHandler
    {
        public static void Handle(Exception ex)
        {
            // Create a random number for the crash log.
            Random rnd = new();
            int number = rnd.Next(100, 999);

            Console.WriteLine("\n");

            // Write the crash information to the terminal.
            term.DebugWrite(ex.ToString() + "\n", 5);
            term.DebugWrite("More information about this fatal system failure will be available as \'system-crash-" + number + ".log\'\n", 6);

            // Create the crash information folder if it doesn't exist.
            if (!System.IO.Directory.Exists(@"0:\AquaSys\Crash"))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(@"0:\AquaSys\Crash");
                    // term.DebugWrite("Successfully made the Crash directory.", 1);
                }
                catch (Exception e)
                {
                    term.DebugWrite("Could not create the crash information folder.\n  " + e.ToString() + "\n", 4);
                }
            }

            // Write the crash information to a file.
            try
            {
                // Create the crash information file.
                // The file is named system-crash-<random number>.log
                // The file is located in 0:\AquaSys\Crash
                System.IO.File.Create(@"0:\AquaSys\Crash\system-crash-" + number + ".log");

                // Write the crash information to the file.
                // This is the most important part of this class.
                // Without this, the crash information would be lost. 
                System.IO.File.WriteAllText("0:\\AquaSys\\Crash\\system-crash-" + number + ".log", DateTime.Now.ToString("HH:mm:ss | ") + ex.ToString() + " | " + ex.Data.ToString());
            }
            catch (Exception e)
            {
                term.DebugWrite(e.ToString(), 4);
            }

            // Reboot the system.
            Cosmos.HAL.Global.PIT.Wait(2500);
            Cosmos.System.Power.Reboot();
        }
    }
}
