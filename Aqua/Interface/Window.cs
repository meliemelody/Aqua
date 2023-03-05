using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqua.Interface
{
    public abstract class Window
    {
        private int _x = 0;
        private int _y = 0;
        private int _width = 0;
        private int _height = 0;
        private string _title = "Window Title";
        int selected_input = 0;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                Draw();
            }
        }
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                Utils.ClearArea(_x, _y, _width, _height, ConsoleColor.Black);
                _width = value;
                Draw();
            }
        }
        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                Utils.ClearArea(_x, _y, _width, _height, ConsoleColor.Black);
                _height = value;
                Draw();
            }
        }

        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                Utils.ClearArea(_x, _y, _width, _height, ConsoleColor.Black);
                _x = value;
                Draw();
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                Utils.ClearArea(_x, _y, _width, _height, ConsoleColor.Black);
                _y = value;
                Draw();
            }
        }

        public List<input> Inputs = null;

        public void DrawText(string text, int x, int y, ConsoleColor bg, ConsoleColor fg)
        {
            Utils.Write(X + x, Y + y, text, bg, fg);
        }

        public void SetCursorPos(int x, int y)
        {
            Console.CursorLeft = X + x;
            Console.CursorTop = Y + y;
        }

        public Window(string title, int width, int height, int x, int y)
        {
            Inputs = new List<input>();
            _title = title;
            if (width < title.Length + 2)
            {
                width = title.Length + 2;
            }
            if (height < 10)
            {
                height = 10;
            }
            _width = width;
            _height = height;
            _x = x;
            _y = y;
            if (Utils.Windows.Count > 0)
            {
                Utils.Windows[Utils.SelectedWindow].UnSelect();
            }
            Utils.Windows.Add(this);
            Utils.SelectedWindow = Utils.Windows.Count - 1;
            this.Select();
            Draw();
        }

        private bool _selected = false;

        public void Select()
        {
            _selected = true;
            Draw();
        }

        public void UnSelect()
        {
            _selected = false;
            Draw();
        }



        public void Draw()
        {
            //Clear area of screen.
            Utils.ClearArea(X, Y, Width, Height, ConsoleColor.Yellow);
            //Clear titlebar area.
            if (_selected)
            {
                Utils.ClearArea(X, Y, Width, 1, Utils.COL_WINDOW_ACTIVE);
                Utils.Write(X + 1, Y, Title, Utils.COL_WINDOW_ACTIVE, ConsoleColor.Black);
            }
            else
            {
                Utils.ClearArea(X, Y, Width, 1, Utils.COL_WINDOW_INACTIVE);
                Utils.Write(X + 1, Y, Title, Utils.COL_WINDOW_INACTIVE, ConsoleColor.Black);
            }
            //draw inputs
            foreach (var input in Inputs)
            {
                input.UnSelect();
            }
        }

        public void KeyDown(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.LeftArrow)
            {
                Inputs[selected_input].UnSelect();
                if (selected_input > 0)
                {
                    selected_input -= 1;
                }
                Inputs[selected_input].Select();
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                Inputs[selected_input].UnSelect();
                if (selected_input < Inputs.Count - 1)
                {
                    selected_input += 1;
                }
                Inputs[selected_input].Select();
            }
            else
            {
                Inputs[selected_input].KeyDown(key);
            }
        }
    }
}
