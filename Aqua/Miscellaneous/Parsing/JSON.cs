using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqua.Miscellaneous.Parsing
{
    public class JSON
    {
        public static Dictionary<string, object> Parse(string jsonString)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            int i = 0;
            while (i < jsonString.Length)
            {
                char c = jsonString[i];
                switch (c)
                {
                    case '{':
                        // Nested object
                        int end = jsonString.IndexOf('}', i);
                        string nestedJson = jsonString.Substring(i + 1, end - i - 1);
                        result = Parse(nestedJson);
                        i = end;
                        break;
                    case '}':
                        // End of object
                        return result;
                    case ',':
                        // Next key-value pair
                        i++;
                        break;
                    case ':':
                        // Value follows colon
                        i++;
                        break;
                    case '"':
                        // String value
                        int endQuote = jsonString.IndexOf('"', i + 1);
                        string key = jsonString.Substring(i + 1, endQuote - i - 1);
                        i = endQuote + 1;
                        // Find colon and parse value
                        int colon = jsonString.IndexOf(':', i);
                        string valueString = jsonString.Substring(colon + 1);
                        object value = ParseValue(valueString);
                        result[key] = value;
                        // Move i past value
                        i += valueString.Length;
                        break;
                    case ' ':
                        // Ignore whitespace
                        i++;
                        break;
                    default:
                        // Number, true, false, or null
                        int endValue = jsonString.IndexOfAny(new char[] { ',', '}', ' ' }, i);
                        string valueString2 = jsonString.Substring(i, endValue - i);
                        object value2 = ParseValue(valueString2);
                        result[value2.ToString()] = value2;
                        i = endValue;
                        break;
                }
            }
            return result;
        }

        private static object ParseValue(string valueString)
        {
            // Try to parse as number
            double number;
            if (double.TryParse(valueString, out number))
            {
                return number;
            }

            // Try to parse as boolean or null
            switch (valueString.ToLower())
            {
                case "true":
                    return true;
                case "false":
                    return false;
                case "null":
                    return null;
                default:
                    return valueString;
            }
        }
    }
}
