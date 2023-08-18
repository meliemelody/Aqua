using Cosmos.Core.Memory;
using Cosmos.System;
using PrismAPI.Graphics;
using PrismAPI.Hardware.GPU;

namespace Aqua.Interface
{
    public class Window
    {
        private Display canvas;
        private int x, y;
        private int width, height;

        private Color backgroundColor = Color.White;
        private Color titlebarColor = Color.Cyan;
        private Color titlebarTextColor = Color.Black;

        private int titlebarHeight = 24;

        private string title = string.Empty;

        private bool isDragging = false;
        private bool shouldClose;
        private int offsetX, offsetY;

        public Window(Display display, string windowTitle, int initialX, int initialY, int windowWidth, int windowHeight)
        {
            canvas = display;
            title = windowTitle;
            x = initialX;
            y = initialY;
            width = windowWidth;
            height = windowHeight + titlebarHeight;
            shouldClose = false;
        }

        public void Update()
        {
            if (shouldClose)
            {
                return;
            }

            HandleMouseInput();
            Render();

            Heap.Collect();
        }

        private void HandleMouseInput()
        {
            if (MouseManager.MouseState == MouseState.Left)
            {
                if (IsMouseOverTitlebar())
                {
                    if (!isDragging)
                    {
                        if (MouseManager.Y - y < titlebarHeight)
                        {
                            isDragging = true;
                            offsetX = (int)MouseManager.X - x;
                            offsetY = (int)MouseManager.Y - y;
                        }
                    }
                }
                else if (IsMouseOverCloseButton())
                {
                    shouldClose = true;
                }
            }
            else
            {
                isDragging = false;
            }

            if (isDragging)
            {
                x = (int)MouseManager.X - offsetX;
                y = (int)MouseManager.Y - offsetY;
            }
        }

        private bool IsMouseOverTitlebar()
        {
            return MouseManager.X >= x && MouseManager.X <= x + width
                && MouseManager.Y >= y && MouseManager.Y <= y + titlebarHeight;
        }

        private bool IsMouseOverCloseButton()
        {
            return MouseManager.X >= x + width - 24 && MouseManager.X <= x + width - 5
                && MouseManager.Y >= y - 24 && MouseManager.Y <= y - 5;
        }

        private void Render()
        {
            // Draw title bar
            canvas.DrawFilledRectangle(x, y, (ushort)width, (ushort)titlebarHeight, 0, titlebarColor);

            // Draw window content
            canvas.DrawFilledRectangle(x, y + titlebarHeight, (ushort)width, (ushort)(height - titlebarHeight), 0, backgroundColor);

            // Render title text
            canvas.DrawString(x + 5, y + 2, title, default, titlebarTextColor);

            // Draw close button
            canvas.DrawFilledRectangle(x + width - 24, y - 24, (ushort)24, (ushort)24, 0, Color.Red);
        }
    }
}
