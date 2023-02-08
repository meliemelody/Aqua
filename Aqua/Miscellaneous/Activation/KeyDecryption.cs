using System;

namespace Aqua.Miscellaneous.Activation
{
    public class KeyDecryption
    {
        public static string DecryptKey(string a)
        {
            string c = null;

            foreach (char d in a)
            {
                if (d % 2 == 0)
                {
                    c += (char)(d + 2);
                }
                else
                {
                    c += (char)(d - 2);
                }
            }
            return c;
        }
    }
}
