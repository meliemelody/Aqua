using System;
using term = Aqua.Terminal.Terminal;

namespace Aqua.ErrorHandler
{
    public static class CrashHandler
    {
        public static void Handle(Exception ex)
        {
            Random rnd = new();
            int number = rnd.Next(100, 999);

            Console.WriteLine("\n");

            term.DebugWrite(ex.ToString() + "\n", 5);
            term.DebugWrite("More information about this fatal system failure will be available as \'system-crash-" + number + ".log\'\n", 6);

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

            try
            {
                System.IO.File.Create(@"0:\AquaSys\Crash\system-crash-" + number + ".log");
                System.IO.File.WriteAllText("0:\\AquaSys\\Crash\\system-crash-" + number + ".log", DateTime.Now.ToString("HH:mm:ss | ") + ex.ToString() + " | " + ex.Data.ToString());
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
