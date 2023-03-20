using System;
using System.Collections.Generic;
using System.IO;

namespace Aqua.Interpreter
{
    /*
     * AquaLang, (c) 2023, ipluxteamx
     *
     * This is a custom interpreter
     * for the Aqua System.
     *
     * Made in 0.3.0
     */

    // Language class, contains all the methods for the interpreter to execute the script.
    // I didn't follow any specific syntax, just made it up as I went along.
    // I am stupid, so don't expect perfection.
    public class Language
    {
        // Constants for storing variables.
        // These are used to store the default value of a variable and the value of a null variable.
        private const string defaultValue = "$_DEFAULT";
        private const string nullValue = "$_NULL";

        // Store variables in a dictionary.
        private static Dictionary<string, object> variables = new Dictionary<string, object>();

        // Initialize the language.
        public Language() { }

        // Store the keywords to search for in the script.
        // These are the keywords that are used to execute commands.
        public static string[] searchText = new string[]
        {
            "write: ",
            "input: ",
            "keywait: ",
            "set: ",
            "if ",
            "while ",
            "clear",
            "color: ",
            "rem: "
        };

        // The run method is called to execute the script.
        // It takes the path to the script as a parameter.
        // This is the main method of the interpreter.
        public static void Run(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    foreach (string s in searchText)
                    {
                        List<string> results = FindInLine(line, s);
                        foreach (string r in results)
                        {
                            Console.WriteLine(r);
                        }
                    }
                }
            }

            variables.Clear();
        }

        // Find the keyword in the line of text.
        // This method is called by the run method.
        // It returns a list of strings containing the results.
        public static List<string> FindInLine(string line, string f)
        {
            List<string> results = new List<string>();

            line = line.TrimStart(); // remove any whitespace at the beginning of the line
            if (line.StartsWith(f))
            {
                int endIndex = line.IndexOf(";", f.Length);
                if (endIndex != -1)
                {
                    string text = line.Substring(f.Length, endIndex - f.Length);
                    switch (f)
                    {
                        // TODO: Add more commands here.
                        case "write: ":
                            if (text.StartsWith("\"") && text.EndsWith("\""))
                                // If the text is enclosed in quotes, treat it as a string to be printed
                                results.Add(text.Substring(1, text.Length - 2));
                            else
                                // Otherwise, treat it as the name of a variable to be printed
                                results.Add((string)GetVariable(text));
                            break;

                        case "input: ":
                            string[] args = text.Split(',');

                            if (args.Length == 2)
                            {
                                string input;
                                ConsoleKeyInfo key;

                                if (GetVariable(args[0].Trim()) == null)
                                    StoreVariable(args[0].Trim(), 0);

                                if (args[1].Trim() == "false" || args[1].Trim() == defaultValue)
                                {
                                    input = Console.ReadLine();
                                    StoreVariable(args[0].Trim(), input);
                                    break;
                                }
                                if (args[1].Trim() == "true")
                                {
                                    bool intercept;
                                    bool.TryParse(args[2].Trim(), out intercept);

                                    key = Console.ReadKey(intercept);
                                    StoreVariable(args[0].Trim(), key.Key);

                                    //Console.WriteLine(GetVariable(args[0].Trim()).GetType());
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine(
                                        "Syntax error: Please input a correct argument [boolean]."
                                    );
                                }
                            }
                            else
                            {
                                Console.WriteLine(
                                    "WARNING: Not enough arguments detected, default mode activated."
                                );
                                string input = Console.ReadLine();
                                StoreVariable(args[0].Trim(), input);
                            }
                            break;

                        case "set: ":
                            string[] parts = text.Split('=');
                            if (parts.Length == 2)
                            {
                                string variableName = parts[0].Trim();
                                string value = parts[1].Trim();

                                if (value == defaultValue)
                                    StoreVariable(variableName, 0);
                                else if (value == nullValue)
                                    StoreVariable(variableName, null);
                                else
                                    StoreVariable(variableName, value);
                            }
                            else
                            {
                                results.Add("Syntax error: Invalid variable assignment.");
                            }
                            break;

                        case "if ":
                            // Parse the condition and the command to be executed if the condition is true
                            string[] ifParts = text.Split(
                                new string[] { "then " },
                                StringSplitOptions.RemoveEmptyEntries
                            );
                            string[] orParts = ifParts[0].Split(
                                new string[] { "or" },
                                StringSplitOptions.RemoveEmptyEntries
                            );

                            if (ifParts.Length == 2)
                            {
                                string[] conditions = orParts;
                                string command = $"{ifParts[1].Trim()};";

                                // Evaluate the condition
                                bool[] conditionResult = new bool[orParts.Length];
                                for (int i = 0; i < conditions.Length; i++)
                                    conditionResult[i] = EvaluateCondition(conditions[i]);

                                foreach (bool result in conditionResult)
                                {
                                    if (result)
                                    {
                                        foreach (string s in searchText)
                                        {
                                            if (command.StartsWith(s))
                                            {
                                                List<string> rel = FindInLine(command, s);
                                                foreach (string r in rel)
                                                {
                                                    Console.WriteLine(r);
                                                }
                                            }
                                        }

                                        break;
                                    }
                                }

                                // TODO : else statement
                            }
                            else
                            {
                                results.Add("Syntax error: Invalid if statement.");
                            }
                            break;

                        case "while ":
                            string[] Do = text.Split(
                                new string[] { "do " },
                                StringSplitOptions.RemoveEmptyEntries
                            );
                            string[] whileParts = Do[0].Split(
                                new string[] { "or" },
                                StringSplitOptions.RemoveEmptyEntries
                            );

                            string cond = null;
                            bool status = false;

                            if (Do.Length == 2)
                            {
                                foreach (string condition in whileParts)
                                {
                                    status = EvaluateCondition(condition.Trim());

                                    if (status)
                                    {
                                        cond = condition.Trim();
                                        break;
                                    }
                                }

                                while (EvaluateCondition(cond))
                                {
                                    try
                                    {
                                        string command = $"{Do[1].Trim()};";
                                        foreach (string s in searchText)
                                        {
                                            if (command.StartsWith(s))
                                            {
                                                List<string> rel = FindInLine(command, s);
                                                foreach (string r in rel)
                                                {
                                                    Console.WriteLine(r);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                            }
                            else
                                Console.WriteLine("Syntax error: Invalid while statement.");

                            break;

                        case "clear":
                            Console.Clear();
                            break;

                        case "color: ":
                            var argsV = text.Split(", ");

                            if (argsV.Length == 2)
                                SwitchColor(argsV);
                            else
                                Console.WriteLine("Syntax error: Not enough arguments");
                            break;

                        case "rem: ":
                            break;
                    }
                }
                else
                {
                    results.Add("Syntax error: Required semicolon not found");
                }
            }

            return results;
        }

        private static void SwitchColor(string[] argsV)
        {
            switch (argsV[1])
            {
                case "true":
                    switch (argsV[0])
                    {
                        // Light colors
                        case "blue"
                        or "b":
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;

                        case "red"
                        or "r":
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;

                        case "yellow"
                        or "y":
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;

                        case "green"
                        or "g":
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;

                        case "magenta"
                        or "m":
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            break;

                        case "cyan"
                        or "c":
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;

                        // Dark-toned colors
                        case "darkblue"
                        or "db":
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            break;

                        case "darkgreen"
                        or "dg":
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            break;

                        case "darkcyan"
                        or "dc":
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            break;

                        case "darkmagenta"
                        or "dm":
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            break;

                        case "darkred"
                        or "dr":
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;

                        case "darkyellow"
                        or "dy":
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;

                        // Monochrome colors
                        case "gray"
                        or "g":
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;

                        case "darkgray"
                        or "dg":
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;

                        case "black"
                        or "d"
                        or defaultValue:
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;

                        case "white"
                        or "w":
                            Console.ForegroundColor = ConsoleColor.White;
                            break;

                        default:
                            Console.WriteLine("Syntax error: Unknown color.");
                            break;
                    }
                    break;

                case "false":
                    switch (argsV[0])
                    {
                        // Light colors
                        case "blue"
                        or "b":
                            Console.BackgroundColor = ConsoleColor.Blue;
                            break;

                        case "red"
                        or "r":
                            Console.BackgroundColor = ConsoleColor.Red;
                            break;

                        case "yellow"
                        or "y":
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            break;

                        case "green"
                        or "g":
                            Console.BackgroundColor = ConsoleColor.Green;
                            break;

                        case "magenta"
                        or "m":
                            Console.BackgroundColor = ConsoleColor.Magenta;
                            break;

                        case "cyan"
                        or "c":
                            Console.BackgroundColor = ConsoleColor.Cyan;
                            break;

                        // Dark-toned colors
                        case "darkblue"
                        or "db":
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            break;

                        case "darkgreen"
                        or "dg":
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            break;

                        case "darkcyan"
                        or "dc":
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                            break;

                        case "darkmagenta"
                        or "dm":
                            Console.BackgroundColor = ConsoleColor.DarkMagenta;
                            break;

                        case "darkred"
                        or "dr":
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            break;

                        case "darkyellow"
                        or "dy":
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            break;

                        // Monochrome colors
                        case "gray"
                        or "g":
                            Console.BackgroundColor = ConsoleColor.Gray;
                            break;

                        case "darkgray"
                        or "dg":
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            break;

                        case "black"
                        or "d":
                            Console.BackgroundColor = ConsoleColor.Black;
                            break;

                        case "white"
                        or "w"
                        or defaultValue:
                            Console.BackgroundColor = ConsoleColor.White;
                            break;

                        default:
                            Console.WriteLine("Syntax error: Unknown color.");
                            break;
                    }
                    break;

                default:
                    Console.WriteLine("Syntax error: Incorrect argument.");
                    break;
            }
        }

        private static bool EvaluateCondition(string condition)
        {
            string[] tokens = condition.Split(
                new string[] { "==", "!=", "<", ">", "<=", ">=" },
                StringSplitOptions.RemoveEmptyEntries
            );
            if (tokens.Length == 2)
            {
                var first = tokens[0].Trim();
                var second = tokens[1].Trim();

                int outfirst,
                    outsecond;

                if (first.StartsWith('"') && first.EndsWith('"'))
                {
                    var parts = tokens[0].Split('"');
                    first = parts[1];
                }
                else if (variables.ContainsKey(first))
                    first = (string)GetVariable(first);
                else
                    throw new Exception($"Invalid variable: {first}");

                if (second.StartsWith('"') && second.EndsWith('"'))
                {
                    var parts = tokens[1].Split('"');
                    second = parts[1];
                }
                else if (variables.ContainsKey(second))
                    second = (string)GetVariable(second);
                else
                    throw new Exception($"Invalid variable: {second}");

                if (int.TryParse(first, out outfirst) && int.TryParse(second, out outsecond))
                {
                    //Console.WriteLine("out = " + outfirst + " " + outsecond);
                    if (condition.Contains("=="))
                        return outfirst == outsecond;
                    else if (condition.Contains("!="))
                        return outfirst != outsecond;
                    else if (condition.Contains("<"))
                        return outfirst < outsecond;
                    else if (condition.Contains(">"))
                        return outfirst > outsecond;
                    else if (condition.Contains("<="))
                        return outfirst <= outsecond;
                    else if (condition.Contains(">="))
                        return outfirst >= outsecond;
                }
                else
                {
                    //Console.WriteLine(first + " " + second);
                    if (condition.Contains("=="))
                        return first == second;
                    else if (condition.Contains("!="))
                        return first != second;
                }
            }

            return false;
        }

        private static void StoreVariable(string variableName, object value)
        {
            if (variables.ContainsKey(variableName))
            {
                variables[variableName] = value;
            }
            else
            {
                variables.Add(variableName, value);
            }
        }

        private static object GetVariable(string variableName)
        {
            if (variables.ContainsKey(variableName))
                return variables[variableName];
            else
                return "Error: Variable not found";
        }
    }
}
