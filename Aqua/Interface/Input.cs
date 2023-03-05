using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqua.Interface
{
    public abstract class input
    {
        private int _x = 0;
        private int _y = 0;
        private int _width = 10;
        private int _height = 10;
        private ConsoleColor _bg = ConsoleColor.Yellow;
        private ConsoleColor _fg = ConsoleColor.Black;
        public Window _parent = null;

        public virtual void KeyDown(ConsoleKeyInfo key)
        {

        }

        public input(int x, int y, int width, int height, ConsoleColor bg, ConsoleColor fg, Window parent)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
            _bg = bg;
            _fg = fg;
            _parent = parent;
            _parent.Inputs.Add(this);
            _parent.Draw();
        }

        public ConsoleColor Background
        {
            get
            {
                return _bg;
            }
            set
            {
                _bg = value;
                Draw(selected);
            }
        }

        public ConsoleColor Foreground
        {
            get
            {
                return _fg;
            }
            set
            {
                _fg = value;
                Draw(selected);
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
                _x = value;
                _parent.Draw();
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
                _y = value;
                _parent.Draw();
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
                _width = value;
                Draw(selected);
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
                _height = value;
                Draw(selected);
            }
        }

        public abstract void Draw(bool _selected);

        private bool selected = false;

        public void UnSelect()
        {
            selected = false;
            Draw(selected);
        }

        public void Select()
        {
            selected = true;
            Draw(selected);
        }
    }
}
