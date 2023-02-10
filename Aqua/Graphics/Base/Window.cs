using Cosmos.System;
using Cosmos.System.Graphics;
using PrismTools.IO;
using System.Drawing;

namespace Aqua.Graphics.Base
{
    public class Window
    {
        uint titleBarHeight = 15;

        string name;
        uint width, height, x, y;
        uint contentX, contentY, contentWidth, contentHeight;

        static bool windowEditMode = false;

        public Window(string name, uint width, uint height, uint x = 0, uint y = 0)
        {

            this.width = width;
            this.height = height;
            this.x = x;
            this.y = y;

            this.contentX = x + 2;
            this.contentY = y + titleBarHeight;
            this.contentWidth = width - 4;
            this.contentHeight = height - titleBarHeight - 1;

            this.name = name;
        }

        // THIS IS UNDER CONSTRUCTION
        public void Update(Canvas canvas)
        {
            // Draw the main window
            canvas.DrawFilledRectangle(Color.White, (int)this.x, (int)this.y, (int)this.width, (int)this.height);

            // Draw the title bar and the title bar string/program name.
            canvas.DrawFilledRectangle(Color.Aquamarine, (int)this.x, (int)((int)this.y - (titleBarHeight * 2)), 150, (int)titleBarHeight * 2);
            canvas.DrawString(name, Cosmos.System.Graphics.Fonts.PCScreenFont.Default, Color.Black, (int)x + 10, (int)((int)this.y - titleBarHeight) - (int)(titleBarHeight / 2));

            canvas.DrawString(windowEditMode.ToString(), Cosmos.System.Graphics.Fonts.PCScreenFont.Default, Color.Black, 10, canvas.Mode.Height - 15);
            windowEditMode = true;

            // Events for the Edit Mode
            if (windowEditMode)
            {
                // Move the window to the cursor's position, with the cursor being in the center of the window.
                this.x = (uint)MouseManager.X - (uint)(width / 2);
                this.y = (uint)MouseManager.Y - (uint)(height / 2);
                canvas.DrawFilledRectangle(Color.White, (int)this.x, (int)this.y, (int)this.width, (int)this.height);

                // If a mouse button is pressed, execute an event.
                if (MouseManager.LastMouseState == MouseState.Right)
                {
                    Kernel.guiStarted = false;
                    canvas.Disable();
                }
                /*else if (KeyboardManager.ReadKey().Key == ConsoleKeyEx.LAlt)
                {
                    windowEditMode = false;
                }*/
            }

            canvas.Display();
        }
    }
}
