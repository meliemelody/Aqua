using System;
using System.Security.Cryptography;
using term = Aqua.Terminal.Terminal;

namespace Aqua.ErrorHandler
{
    public static class CrashHandler
    {
        public static void CrashHandle(Exception ex)
        {
            term.DebugWrite(ex.ToString(), 5);
            term.DebugWrite("More information about the crash will be available as \'system-crash-*number*.txt\'", 6);

            if (!System.IO.Directory.Exists(@"0:\Crash"))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(@"0:\Crash");
                    // term.DebugWrite("Successfully made the Crash directory.", 1);
                }
                catch (Exception e)
                {
                    term.DebugWrite("Could not create the crash information folder.\n  " + e.ToString(), 4);
                }
            }

            try
            {
                Random rnd = new Random();
                int number = rnd.Next(3);

                System.IO.File.Create(@"0:\Crash\system-crash-" + number + ".txt");
                System.IO.File.WriteAllText("0:\\Crash\\system-crash-" + number + ".txt", ex.ToString());
            }
            catch (Exception e)
            {
                term.DebugWrite(e.ToString(), 4);
            }

            Cosmos.HAL.Global.PIT.Wait(2500);
            Cosmos.System.Power.Reboot();
        }
    }
}
