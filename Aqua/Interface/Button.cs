using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * A button is a type of input that can be selected and clicked on.
 * It is drawn with a square bracket around the text.
 * The button is drawn with the default color when it is not selected, and vice versa.
 */
namespace Aqua.Interface
{
    class Button : input
    {
        // The text to be displayed on the button.
        public string Text { get; set; }

        // Create a new button with the given text, position, size and parent window.
        // The button is drawn with the default color.
        public Button(string text, int x, int y, int width, int height, Window parent)
            : base(x, y, width, height, Utils.COL_BUTTON_DEFAULT, Utils.COL_BUTTON_TEXT, parent)
        {
            Text = text;
            Draw(false);
        }

        // Draw the button with the correct color and text depending on if it is selected or not.
        // This is called by the parent window when it is drawn.
        public override void Draw(bool _selected)
        {
            if (_selected == true)
            {
                _parent.DrawText(
                    "[" + Text + "]",
                    X,
                    Y,
                    Utils.COL_BUTTON_SELECTED,
                    Utils.COL_BUTTON_TEXT
                );
                _parent.SetCursorPos(X, Y);
            }
            else
                _parent.DrawText(
                    "[" + Text + "]",
                    X,
                    Y,
                    Utils.COL_BUTTON_DEFAULT,
                    Utils.COL_BUTTON_TEXT
                );
        }

        // This is a function to delete the button, using Console.Write to overwrite the button with spaces.
        public void Delete()
        {
            _parent.DrawText(
                new string(' ', Text.Length + 2),
                X,
                Y,
                Utils.COL_BUTTON_DEFAULT,
                Utils.COL_BUTTON_TEXT
            );
        }
    }
}
