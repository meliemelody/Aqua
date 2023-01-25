using Aqua.Commands;
using Cosmos.System.Graphics;
using System;
using System.Drawing;

namespace Aqua.Graphics
{
    public class Graphics
    {
        public static void GraphicsStart(Canvas canvas)
        {
            try
            {
                canvas = FullScreenCanvas.GetFullScreenCanvas();
                canvas.Clear(Color.Violet);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to initialize Graphics: " + e.Message);
            }
        }
    }

    public class grComs : Command
    {
        public grComs(String name) : base(name) { }

        public override string Execute(string[] args)
        {
            switch(args[0])
            {
                case "start":
                    Graphics.GraphicsStart(Kernel.canvas);
                    Kernel.guiStarted = true;
                    return null;
            }

            return null;
        }
    }
}
