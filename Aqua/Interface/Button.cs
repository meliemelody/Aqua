using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqua.Interface
{
    class Button : input
    {
        public string Text { get; set; }

        public Button(string text, int x, int y, int width, int height, Window parent) : base(x, y, width, height, Utils.COL_BUTTON_DEFAULT, Utils.COL_BUTTON_TEXT, parent)
        {
            Text = text;
            Draw(false);
        }

        public override void Draw(bool _selected)
        {
            if (_selected == true)
            {
                _parent.DrawText("[" + Text + "]", X, Y, Utils.COL_BUTTON_SELECTED, Utils.COL_BUTTON_TEXT);
                _parent.SetCursorPos(X, Y);
            }
            else
                _parent.DrawText("[" + Text + "]", X, Y, Utils.COL_BUTTON_DEFAULT, Utils.COL_BUTTON_TEXT);
        }
    }
}
