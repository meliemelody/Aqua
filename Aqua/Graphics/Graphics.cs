using System;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using System.Drawing;
using Cosmos.System.Graphics.Fonts;
using Cosmos.Debug.Kernel;

namespace Aqua.Graphics
{
    public class Graphics
    {
        public Canvas canvas;
        public static void GraphicsStart(Canvas canvas)
        {
            try
            {
                canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(640, 480, ColorDepth.ColorDepth32));
                canvas.Clear(Color.DarkSlateBlue);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to initialize Graphics: " + e.Message);
            }
        }

        public void Update()
        {
            try
            {
                // canvas.DrawFilledEllipse(Color.White, 60, 60, 50, 50);
                // canvas.DrawString("test", PCScreenFont.Default, Color.White, 50, 250);
            }
            catch
            {
                canvas.Clear(Color.Red);
                //mDebugger.Send("Exception occurred: " + e.Message);
            }

            canvas.Display();
        }
    }
}
