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

    public class Language
    {
        private static Dictionary<string, object> variables = new Dictionary<string, object>();

        public Language() { }

        public static string[] searchText = new string[] {
                "write: ",
                "input: ",
                "keywait: ",
                "set: ",
                "if "
            };

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

                            for (int i = 0; i < args.Length; i++)
                                Console.WriteLine(args[i].Trim());

                            if (args.Length == 3)
                            {
                                string input;
                                ConsoleKeyInfo key;

                                if (args[1].Trim() == "true")
                                {
                                    bool intercept;
                                    bool.TryParse(args[2].Trim(), out intercept);

                                    key = Console.ReadKey(intercept);
                                    StoreVariable(text.Trim(), key.Key);

                                    Console.WriteLine(GetVariable(text.Trim()).GetType());
                                }
                                else if (args[1].Trim() == "false")
                                {
                                    input = Console.ReadLine();
                                    StoreVariable(text.Trim(), input);
                                }
                                else
                                {
                                    Console.WriteLine("Syntax error: Not enough arguments");
                                }
                            }
                            else
                            {
                                string input = Console.ReadLine();
                                StoreVariable(text.Trim(), input);
                            }
                            break;

                        case "set: ":
                            string[] parts = text.Split('=');
                            if (parts.Length == 2)
                            {
                                string variableName = parts[0].Trim();
                                string value = parts[1].Trim();
                                StoreVariable(variableName, value);
                            }
                            else
                            {
                                results.Add("Syntax error: Invalid variable assignment");
                            }
                            break;

                        case "if ":
                            // Parse the condition and the command to be executed if the condition is true
                            string[] ifparts = text.Split(new string[] { "then " }, StringSplitOptions.RemoveEmptyEntries);
                            if (ifparts.Length == 2)
                            {
                                string condition = ifparts[0].Trim();
                                string command = $"{ifparts[1].Trim()};";

                                // Evaluate the condition
                                bool conditionResult = EvaluateCondition(condition);

                                //Console.WriteLine(command + " " + conditionResult);

                                // If the condition is true, execute the command
                                if (conditionResult)
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
                                }
                            }
                            else
                            {
                                results.Add("Syntax error: Invalid if statement");
                            }
                            break;
                    }
                }
                else
                {
                    results.Add("Syntax error : Required semicolon not found");
                }
            }

            return results;
        }

        private static bool EvaluateCondition(string condition)
        {
            string[] tokens = condition.Split(new string[] { "==" }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 2)
            {
                var first = tokens[0].Trim();
                var second = tokens[1].Trim();

                int outfirst, outsecond;

                if (variables.ContainsKey(first))
                    first = (string)GetVariable(first);
                if (variables.ContainsKey(second))
                    second = (string)GetVariable(second);

                if (int.TryParse(first, out outfirst) && int.TryParse(second, out outsecond))
                {
                    //Console.WriteLine("out = " + outfirst + " " + outsecond);
                    return outfirst == outsecond;
                }
                else
                {
                    //Console.WriteLine(first + " " + second);
                    return first == second;
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
