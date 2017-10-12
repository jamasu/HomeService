using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeService.Business
{
    public static class Helper
    {
        public static string ToLower(string origString)
        {
            char[] temp = new char[origString.Length];
            Dictionary<char, char> toLower = new Dictionary<char, char>();
            InitAlphaDictionary(toLower);

            try
            {
                for (int i = 0; i < origString.Length; i++)
                {

                    if (toLower.ContainsKey(origString.ElementAt(i)))
                    {
                        if (i == 0)
                        {
                            temp[i] = origString.ElementAt(i);
                            continue;
                        }
                        temp[i] = toLower[origString.ElementAt(i)];
                    }
                    else
                        temp[i] = origString.ElementAt(i);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return new string(temp);
        }

        private static void InitAlphaDictionary(Dictionary<char, char> toLower)
        {
            toLower.Add('A', 'a');
            toLower.Add('B', 'b');
            toLower.Add('C', 'c');
            toLower.Add('D', 'd');
            toLower.Add('E', 'e');
            toLower.Add('F', 'f');
            toLower.Add('G', 'g');
            toLower.Add('H', 'h');
            toLower.Add('I', 'i');
            toLower.Add('J', 'j');
            toLower.Add('K', 'k');
            toLower.Add('L', 'l');
            toLower.Add('M', 'm');
            toLower.Add('N', 'n');
            toLower.Add('O', 'o');
            toLower.Add('P', 'p');
            toLower.Add('Q', 'q');
            toLower.Add('R', 'r');
            toLower.Add('S', 's');
            toLower.Add('T', 't');
            toLower.Add('U', 'u');
            toLower.Add('V', 'v');
            toLower.Add('W', 'w');
            toLower.Add('X', 'x');
            toLower.Add('Y', 'y');
            toLower.Add('Z', 'z');
            toLower.Add('Æ', 'æ');
            toLower.Add('Ø', 'ø');
            toLower.Add('Å', 'å');
        }
    }
}
