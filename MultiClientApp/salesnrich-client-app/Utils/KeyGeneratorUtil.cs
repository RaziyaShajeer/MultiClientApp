using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNR_ClientApp.Utils
{
    public class KeyGeneratorUtil
    {

        private static readonly char[] alphabet = "0123456789abcdefghijklmnopqrstuvwxyz".ToCharArray();
        private static readonly Random rand = new Random();

        public static string GetRandomCustomString(char[] str, int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("Invalid length to create random string");
            }
            StringBuilder sb = new StringBuilder(length);
            Random rand = new Random();
            for (int i = 0; i < length; i++)
            {
                int randomIndex = rand.Next(str.Length);
                sb.Append(str[randomIndex]);
            }
            return sb.ToString();
        }

        internal static string getRandomAlphaNumericString(int length)
        {
            
                if (length < 0)
                {
                    throw new ArgumentException("Invalid length to create random string");
                }
                StringBuilder sb = new StringBuilder(length);
                for (int i = 0; i < length; i++)
                {
                    int randomIndex = rand.Next(alphabet.Length);
                    sb.Append(alphabet[randomIndex]);
                }
                return sb.ToString();
            
        }
    }
}
