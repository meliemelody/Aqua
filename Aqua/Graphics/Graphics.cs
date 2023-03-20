using Aqua.Commands;
using Cosmos.System;
using Cosmos.System.Graphics;
using System;
using System.Drawing;
using term = Aqua.Terminal.Screen;
using SharpDX.Mathematics.Interop;
using Cosmos.HAL;
using System.Threading;
using Aqua.Graphics.Base;

namespace Aqua.Graphics
{
    public class Graphics
    {
        static int[] winSize = new int[2] { 500, 250 },
            windowPos =  { 0, 30 };

        public static Canvas GraphicsStart()
        {
            try
            {
                Canvas canvas = FullScreenCanvas.GetFullScreenCanvas(
                    new Mode(640, 480, ColorDepth.ColorDepth32)
                );
                return canvas;
            }
            catch (Exception e)
            {
                ErrorHandler.CrashHandler.Handle(e);
                return null;
            }
        }

        public static void Draw(Canvas canvas)
        {
            // you have found the doofenshmirtz evil incorporated thing i dk   yea im tired
            canvas.Clear(Color.LightSeaGreen);

            canvas.DrawString(
                "Graphical User Interface Testing Environment",
                Cosmos.System.Graphics.Fonts.PCScreenFont.Default,
                Color.White,
                0,
                0
            );
            /*if (MouseManager.LastMouseState == MouseState.Left && canMove == true)
                dragging = !dragging;*/

            Window window =
                new(
                    "Test Window",
                    (uint)winSize[0],
                    (uint)winSize[1],
                    (uint)windowPos[0],
                    (uint)windowPos[1]
                );
            window.Update(canvas);

            canvas.DrawFilledRectangle(
                Color.Black,
                (int)MouseManager.X - 5,
                (int)MouseManager.Y - 5,
                10,
                10
            );
            canvas.Display();
        }
    }

    public class grComs : Command
    {
        public grComs(String name, String description)
            : base(name, description) { }

        public override string Execute(string[] args)
        {
            switch (args[0])
            {
                case "start":
                    Kernel.canvas = Graphics.GraphicsStart();

                    MouseManager.ScreenWidth = (uint)Kernel.canvas.Mode.Width;
                    MouseManager.ScreenHeight = (uint)Kernel.canvas.Mode.Height;

                    MouseManager.X = MouseManager.ScreenWidth / 2;
                    MouseManager.Y = MouseManager.ScreenHeight / 2;

                    Kernel.guiStarted = true;
                    return null;
            }

            return null;
        }
    }
}
