using System;

namespace Aqua.Miscellaneous
{
    public class Decryption
    {
        private const int V = 2;

        public static string Decrypt(string a)
        {
            string c = null;
            int p = V;

            foreach (char d in a)
            {
                if (d % V == 0)
                {
                    c += (char)(d + p);
                }
                else
                {
                    c += (char)(d - p);
                }
            }
            return c;
        }

        public static string Encrypt(string a)
        {
            string c = null;

            foreach (char d in a)
            {
                if (d % V == 0)
                {
                    c += (char)(d - V);
                }
                else
                {
                    c += (char)(d + V);
                }
            }
            return c;
        }
    }
}
