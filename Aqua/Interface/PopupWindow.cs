using Aqua.Commands.Executables;
using System;
using System.Xml.Linq;

namespace Aqua.Interface
{
    public class PopupWindow : Window
    {
        const int new_Width = 25, new_Height = 5;
        static readonly int _x = Console.WindowWidth / 2 - new_Width / 2, _y = Console.WindowHeight / 2 - new_Height / 2;

        static (int Left, int Top) conPos = Console.GetCursorPosition();

        // This is the constructor for the PopupWindow class.
        // It is used to create a popup window.
        public PopupWindow(string title) : base(title, new_Width, new_Height, _x, _y)
        {
            // Create a new button and add it to the window's Inputs list
            int buttonWidth = 5, buttonHeight = 3;

            Button button1 = new Button("OK", Width / 2 - buttonWidth / 2, 3, buttonWidth, buttonHeight, this);
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
            for (int screenX = _x; screenX < Width; screenX++)
            {
                for (int screenY = _y; screenY < Height; screenY++)
                {
                    Console.SetCursorPosition(screenX, screenY);
                    Console.Write(' ');
                }
            }
        }
    }
}
