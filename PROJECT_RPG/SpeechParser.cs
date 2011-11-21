using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PROJECT_RPG
{
    class SpeechParser
    {
        public static String[] ParseString(String text)
        {
            String[] temp = text.Split(new char[] { '/' });
            return temp;
        }

        public static String ParseASCII(String text, int instance)
        {
            String retString = "";
            switch (instance)
            {
                case 1:
                    retString = ParseHex(text, 3);
                    break;
                case 2:
                    retString = ParseHex(text, 2);
                    break;
                case 3:
                    retString = ParseHex(text, 1);
                    break;
                case 4:
                    retString = ParseBinary(ParseHex(text, 1), 2);
                    break;
                case 5:
                    retString = ParseBinary(ParseHex(text, 1), 1);
                    break;
            }
            return retString;
        }

        private static String ParseHex(String text, int num)
        {
            String retString = "";
            int charNum;
            for (int i = 0; i < text.Length; i++)
            {
                if (i % num == 0)
                {
                    charNum = text[i];
                    retString += charNum.ToString("x");
                }
                else
                {
                    retString += text[i];
                }
            }
            return retString;

        }

        private static String ParseBinary(String text, int num)
        {
            String retString = "";
            char hex;
            for (int i = 0; i < text.Length; i++)
            {
                if (i % num == 0)
                {
                    hex = text[i];
                    retString += ParseHexToBinary(hex);
                }
                else
                    retString += text[i];
            }
            return retString;
        }

        private static String ParseHexToBinary(char hex)
        {
            String retString = "";
            switch (hex)
            {
                case '0':
                    retString = "0000";
                    break;
                case '1':
                    retString = "0001";
                    break;
                case '2':
                    retString = "0010";
                    break;
                case '3':
                    retString = "0011";
                    break;
                case '4':
                    retString = "0100";
                    break;
                case '5':
                    retString = "0101";
                    break;
                case '6':
                    retString = "0110";
                    break;
                case '7':
                    retString = "0111";
                    break;
                case '8':
                    retString = "1000";
                    break;
                case '9':
                    retString = "1001";
                    break;
                case 'A':
                    retString = "1010";
                    break;
                case 'B':
                    retString = "1011";
                    break;
                case 'C':
                    retString = "1100";
                    break;
                case 'D':
                    retString = "1101";
                    break;
                case 'E':
                    retString = "1110";
                    break;
                case 'F':
                    retString = "1111";
                    break;
            }
            return retString;
        }
    }
}
