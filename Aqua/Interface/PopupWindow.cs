using Aqua.Commands.Executables;
using System;
using System.Xml.Linq;

namespace Aqua.Interface
{
    public class PopupWindow : Window
    {
        private const int width = 25, height = 5;
        static int x = Console.WindowWidth / 2 - width/2, y = Console.WindowHeight / 2 - height/2;

        static (int Left, int Top) conPos = Console.GetCursorPosition();

        public PopupWindow(string title) : base(title, width, height, x, y)
        {
            // Create a new button and add it to the window's Inputs list
            int buttonWidth = 5, buttonHeight = 3;

            Button button1 = new Button("OK", width/2-buttonWidth/2, 3, buttonWidth, buttonHeight, this);
            Inputs.Add(button1);
        }

        public bool WaitForInput()
        {
            while (true)
            {
                var input = Console.ReadKey();

                if (input.Key == ConsoleKey.Enter)
                {
                    Console.SetCursorPosition(conPos.Left, conPos.Top);
                    return true;
                }
                else if (input.Key == ConsoleKey.Escape)
                {
                    Console.SetCursorPosition(conPos.Left, conPos.Top);
                    return false;
                }
                else
                    Console.Beep();
            }
        }

        public void Clear()
        {
            Console.BackgroundColor = Kernel.bgColor;
            for (int screenX = x; screenX < width; screenX++)
            {
                for (int screenY = y; screenY < height; screenY++)
                {
                    Console.SetCursorPosition(screenX, screenY);
                    Console.Write(' ');
                }
            }
        }
    }
}
