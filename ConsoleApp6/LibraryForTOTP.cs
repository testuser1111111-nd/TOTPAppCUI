using System;
using System.Security.Cryptography;
using System.Collections.Generic;
namespace LibraryForTOTP
{
    public static class RFC6238andRFC4226
    {
        //requires "using System;" and "using System.Security.Cryptography;" on top of code
        public static int GenTOTP(byte[] S, int adjust = 0, int span = 30)
        {

            TimeSpan time = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var counter = (long)time.TotalSeconds / span;
            return GenHOTP(S, counter + adjust);
        }
        public static long GenCounter(long span = 30)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return (long)ts.TotalSeconds / span;
        }
        public static int GenHOTP(byte[] S, long C, int digit = 6)
        {
            var hmsha = new HMACSHA1();
            hmsha.Key = S;
            var counter = BitConverter.GetBytes(C);
            Array.Reverse(counter, 0, counter.Length);
            var hs = hmsha.ComputeHash(counter);
            return DTruncate(hs) % (int)(Math.Pow(10, digit));
        }
        static int DTruncate(byte[] vs)
        {
            var offset = vs[vs.Length - 1] & 15;
            var P = (vs[offset] << 24 | vs[offset + 1] << 16 | vs[offset + 2] << 8 | vs[offset + 3]) & 0x7fffffff;
            return P;
        }
    }
    public static class RFC4648Base32
    {
        //requires "using System;" and "using System.Collections.Generic;" on top of code
        public static int CharToInt(char c)
        {
            switch (c)
            {
                case 'a': return 0;
                case 'b': return 1;
                case 'c': return 2;
                case 'd': return 3;
                case 'e': return 4;
                case 'f': return 5;
                case 'g': return 6;
                case 'h': return 7;
                case 'i': return 8;
                case 'j': return 9;
                case 'k': return 10;
                case 'l': return 11;
                case 'm': return 12;
                case 'n': return 13;
                case 'o': return 14;
                case 'p': return 15;
                case 'q': return 16;
                case 'r': return 17;
                case 's': return 18;
                case 't': return 19;
                case 'u': return 20;
                case 'v': return 21;
                case 'w': return 22;
                case 'x': return 23;
                case 'y': return 24;
                case 'z': return 25;
                case 'A': return 0;
                case 'B': return 1;
                case 'C': return 2;
                case 'D': return 3;
                case 'E': return 4;
                case 'F': return 5;
                case 'G': return 6;
                case 'H': return 7;
                case 'I': return 8;
                case 'J': return 9;
                case 'K': return 10;
                case 'L': return 11;
                case 'M': return 12;
                case 'N': return 13;
                case 'O': return 14;
                case 'P': return 15;
                case 'Q': return 16;
                case 'R': return 17;
                case 'S': return 18;
                case 'T': return 19;
                case 'U': return 20;
                case 'V': return 21;
                case 'W': return 22;
                case 'X': return 23;
                case 'Y': return 24;
                case 'Z': return 25;
                case '2': return 26;
                case '3': return 27;
                case '4': return 28;
                case '5': return 29;
                case '6': return 30;
                case '7': return 31;
                default: return -1;
            }

        }
        public static byte[] FromBase32String(string base32text, char padding = '=')
        {
            if (base32text == null || base32text.Length == 0)
            {
                return Array.Empty<byte>();
            }
            base32text = base32text.Trim().TrimEnd(padding);
            long len = base32text.Length;
            const int cutlength = 8;
            long len2 = len % cutlength == 0 ? len / cutlength : (len / cutlength) + 1;
            string[] splitedtext = new string[len2];
            for (int i = 0; i < splitedtext.Length; i++)
            {
                splitedtext[i] = base32text.Substring(0, cutlength < base32text.Length ? cutlength : base32text.Length);
                base32text = base32text.Substring(cutlength < base32text.Length ? cutlength : base32text.Length);
            }
            List<byte> decoded = new List<byte>();
            int len3 = 0;
            int len4 = splitedtext[splitedtext.Length - 1].Length;

            switch (len4)
            {
                case 1: throw new FormatException("Base32 length not appropriate");
                case 2: len3 = 1; break;
                case 3: throw new FormatException("Base32 length not appropriate");
                case 4: len3 = 2; break;
                case 5: len3 = 3; break;
                case 6: throw new FormatException("Base32 length not appropriate");
                case 7: len3 = 4; break;
                case 8: len3 = 5; break;
            }
            for (int i = 0; i < splitedtext.Length; i++)
            {
                ulong piece = 0;
                for (int j = 0; j < cutlength; j++)
                {

                    piece <<= 5;

                    if (j < splitedtext[i].Length)
                    {
                        piece |= (uint)CharToInt(splitedtext[i][j]);
                    }
                }
                for (int j = 0; j < 5; j++)
                {
                    ulong aaa = (piece >> (4 - j) * 8) & 255;
                    if (i != splitedtext.Length - 1 | j < len3)
                    {
                        decoded.Add((byte)aaa);
                    }

                }
            }
            return decoded.ToArray();
        }
        public const string table = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        public static string ToBase32String(byte[] data, char padding = '=')
        {
            const uint mask = 31;
            int divideinto = data.Length % 5 == 0 ? data.Length / 5 : data.Length / 5 + 1;
            string encoded = string.Empty;
            for (int i = 0; i < divideinto; i++)
            {
                ulong temp = 0;
                for (int j = 0; j < 5; j++)
                {
                    temp <<= 8;
                    if (i * 5 + j < data.Length)
                    {
                        temp |= data[i * 5 + j];
                    }
                }
                for (int j = 0; j < 8; j++)
                {
                    int finallength = 8;
                    switch (data.Length % 5)
                    {
                        case 0: finallength = 8; break;
                        case 1: finallength = 2; break;
                        case 2: finallength = 4; break;
                        case 3: finallength = 5; break;
                        case 4: finallength = 7; break;
                    }

                    if (i < divideinto - 1 | (i == divideinto - 1 && j < finallength))
                    {
                        encoded += table[(int)((temp >> 5 * (7 - j)) & mask)];
                    }
                    else
                    {
                        encoded += padding;
                    }

                }
            }
            return encoded;
        }
    }
}