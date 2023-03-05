using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqua.Interface
{
    public class Utils
    {
        public const ConsoleColor COL_BUTTON_DEFAULT = ConsoleColor.Gray;
        public const ConsoleColor COL_BUTTON_SELECTED = ConsoleColor.Black;
        public const ConsoleColor COL_BUTTON_TEXT = ConsoleColor.White;
        public const ConsoleColor COL_WINDOW_ACTIVE = ConsoleColor.White;
        public const ConsoleColor COL_WINDOW_INACTIVE = ConsoleColor.Gray;

        public static List<Window> Windows = new List<Window>();
        public static int SelectedWindow = 0;

        public static void ClearScreen(ConsoleColor bg)
        {
            Console.BackgroundColor = bg;
            Console.Clear();
        }

        public static void ClearArea(int x, int y, int width, int height, ConsoleColor bg)
        {
            Console.BackgroundColor = bg;
            for (int i = 0; i < height; i++)
            {
                Console.CursorLeft = x;
                Console.CursorTop = y + i;
                Console.Write(repeat(" ", width));
            }
        }

        public static string repeat(string input, int width)
        {
            string output = "";
            for (int i = 0; i < width; i++)
            {
                output += input;
            }
            return output;
        }

        public static void Write(int x, int y, string text, ConsoleColor bg, ConsoleColor fg)
        {
            Console.CursorLeft = x;
            Console.CursorTop = y;
            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
            Console.Write(text);
        }

        public static void WriteBlock(string text, int x, int y, int width, int height, ConsoleColor bg, ConsoleColor fg)
        {
            List<string> text_lines = new List<string>();
            //Separate lines of text.
            for (int i = 0; i < text.LastIndexOf(' '); i++)
            {
                if (i >= width)
                {
                    if (text[i] == ' ')
                    {
                        text_lines.Add(text.Substring(0, i + 1));
                        text = text.Remove(0, i + 1);
                        i = 0;
                    }
                }
            }
            if (text.Length > 0)
            {
                if (text.Length > width)
                {
                    for (int i = 0; i < text.LastIndexOf(' '); i++)
                    {
                        if (i >= width)
                        {
                            if (text[i] == ' ')
                            {
                                text_lines.Add(text.Substring(0, i + 1));
                                text = text.Remove(0, i + 1);
                                i = 0;
                            }
                        }
                    }
                    if (text.Length > 0)
                    {
                        text_lines.Add(text);
                    }
                }
                else
                {
                    text_lines.Add(text);
                }
            }
            //by now, I should be able to get the line with the most amount of chars and make it the width.
            int new_width = 0;
            foreach (var line in text_lines)
            {
                if (line.Length > new_width)
                {
                    new_width = line.Length;
                }
            }
            width = new_width;
            //Gotta set a proper height.
            if (text_lines.Count > height)
            {
                height = text_lines.Count;
            }
            ClearArea(x, y, width, height, bg);
            for (int i = 0; i < text_lines.Count; i++)
            {
                Write(x, y + i, text_lines[i], bg, fg);
            }
        }

        public static int get_window_width(string title)
        {
            int width = title.Length + 3;

            return width;
        }

        public static List<string> split_string(int width, string text)
        {
            width = width - 1; //So we're not constantly comparing to width - 1
            var returnSet = new List<string>();
            var currLength = 0;
            var oldOffset = 0;
            for (var i = 0; i < text.Length; i++)
            {
                if (currLength >= width && text[i] == ' ')
                {
                    returnSet.Add(text.Substring(oldOffset, i - oldOffset));
                    oldOffset = i + 1;
                    currLength = 0;
                    continue;
                }
                currLength++;
            }
            if (oldOffset < text.Length)
                returnSet.Add(text.Substring(oldOffset));

            return returnSet;
        }
    }
}
