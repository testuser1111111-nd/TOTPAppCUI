using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Diagnostics;


namespace ConsoleApp1
{
    public static class Program
    {
        public static void Main()
        {
            int span = 30;
            List<string[]> keys = ImportKey();
            int status = 0;
            bool flag = true;
            while (flag)
            {
                switch (status)
                {//酷過ぎるコード
                    case 0:
                        {//エディタで折りたたむためだけの中括弧で特に意味はない。他のケースのも同じ
                            Console.WriteLine("this is main menu.");
                            Console.WriteLine("choose option...");
                            Console.WriteLine("0 => return to this menu");
                            Console.WriteLine("1 => show saved key(s) in Base32");
                            Console.WriteLine("2 => delete key(s)");
                            Console.WriteLine("3 => calculate TOTP from saved key(s)");
                            Console.WriteLine("4 => input new key(s) in Base32");
                            Console.WriteLine("5 => input new key(s) in Base64(not recommended)");
                            Console.WriteLine("6 => show saved key(s) in Base64(not recommended)");
                            Console.WriteLine("7 => clear console");
                            Console.WriteLine("8 => generate a key to test");
                            Console.WriteLine("9 => save and quit");
                            Console.WriteLine("10 => Licences");
                            try
                            {
                                status = int.Parse(string.Empty + Console.ReadLine());
                            }
                            catch
                            {
                                Console.WriteLine("wrong input format. please input only integer");
                                status = 0;
                            }
                        }
                        break;
                    case 1:
                        {
                            Console.WriteLine("Keys ...");
                            Console.WriteLine();
                            if (keys == null || keys.Count == 0)
                            {
                                Console.WriteLine("no saved key");
                            }
                            else
                            {
                                int counter = 0;
                                foreach (string[] s in keys)
                                {
                                    Console.WriteLine("index = {2},name : {0} \t, key {1}", s[0], s[1], counter);
                                    counter++;
                                }
                            }
                            Console.WriteLine("press any key to return main menu");
                            Console.ReadKey();
                            Console.WriteLine();
                            status = 0;
                        }
                        break;
                    case 2:
                        {
                            if (keys == null || keys.Count == 0)
                            {
                                Console.WriteLine("No saved key.");
                                Console.WriteLine("Returning to main menu");
                                status = 0;
                            }
                            else
                            {
                                bool answered = false;
                                while (!answered)
                                {

                                    bool answered2 = false;
                                    Console.WriteLine("Enter the index of key you want to delete.");
                                    int counter = 0;
                                    foreach (string[] s in keys)
                                    {
                                        Console.WriteLine("index:{0} name:{1} key:{2}", counter, s[0], s[1]);
                                        counter++;
                                    }
                                    int index = 0;
                                    bool deleteindexcheck = false;
                                    while (!deleteindexcheck)
                                    {


                                        try
                                        {
                                            index = int.Parse(string.Empty + Console.ReadLine());
                                            Console.WriteLine("do you really want to delete {0}'s key? answer in Yes/No", keys[index][0]);
                                            deleteindexcheck = true;
                                        }
                                        catch
                                        {
                                            Console.WriteLine("wrong input. try again.");
                                        }
                                    }
                                    string input = string.Empty + Console.ReadLine();
                                    while (!answered2)
                                    {
                                        if (input == "Yes")
                                        {
                                            try
                                            {
                                                keys.RemoveAt(index);
                                                Console.WriteLine("Deleted key. Delete another key(Yes) or return to main menu(No)?");
                                                string input2 = string.Empty + Console.ReadLine();
                                                bool answered3 = false;
                                                while (!answered3)
                                                {
                                                    if (input2 == "Yes")
                                                    {
                                                        answered2 = true;
                                                        answered3 = true;
                                                        if (keys.Count == 0)
                                                        {
                                                            Console.WriteLine("No saved key.");
                                                            Console.WriteLine("Returning to main menu");
                                                            answered = true;
                                                            status = 0;
                                                        }
                                                    }
                                                    else if (input2 == "No")
                                                    {
                                                        answered = true;
                                                        answered2 = true;
                                                        answered3 = true;
                                                        status = 0;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("wrong answer. try again.");
                                                        input2 = string.Empty + Console.ReadLine();
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("Error:{0}", ex.Message);
                                            }
                                        }

                                        else if (input == "No")
                                        {
                                            Console.WriteLine("Aborted deleting key. Delete another key(Yes) or return to main menu(No)?");
                                            string input2 = string.Empty + Console.ReadLine();
                                            bool answered3 = false;
                                            while (!answered3)
                                            {
                                                if (input2 == "Yes")
                                                {
                                                    answered2 = true;
                                                    answered3 = true;
                                                }
                                                else if (input2 == "No")
                                                {
                                                    answered = true;
                                                    answered2 = true;
                                                    answered3 = true;
                                                    status = 0;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("wrong answer. try again.");
                                                    input2 = string.Empty + Console.ReadLine();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("wrong answer. try again.");
                                            input = string.Empty + Console.ReadLine();
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        {
                            if (keys == null || keys.Count == 0)
                            {
                                Console.WriteLine("No saved key to Generate TOTP");
                                Console.WriteLine("returning to main menu");
                                status = 0;
                            }
                            else
                            {

                                int counter = 0;
                                foreach (string[] key in keys)
                                {
                                    try
                                    {
                                        Console.WriteLine("name:{0} , TOTP:{1:000000}", key[0], LibraryForTOTP.RFC6238andRFC4226.GenTOTP(LibraryForTOTP.RFC4648Base32.FromBase32String(key[1]), 0, span));
                                    }
                                    catch(Exception ex)
                                    {
                                        Console.WriteLine("name:{0} , TOTP:Error {1}",key[0],ex.ToString());
                                    }
                                    counter++;
                                }
                                int time = DateTime.UtcNow.Second;
                                long timecounter = LibraryForTOTP.RFC6238andRFC4226.GenCounter();
                                Console.WriteLine("Remaining time:{0} second", span - time % span);
                                Console.WriteLine("Regenerate? \"Yes\" to regenerate key.\"Time\" to show time.\"No\" to return main menu");
                                bool answered = false;
                                while (!answered)
                                {
                                    string answer = string.Empty + Console.ReadLine();

                                    if (answer == "Yes")
                                    {
                                        status = 3;
                                        answered = true;
                                    }
                                    else if (answer == "Time")
                                    {
                                        int remaintime = span - time % span - (DateTime.UtcNow.Second - time);
                                        if (LibraryForTOTP.RFC6238andRFC4226.GenCounter()==timecounter)
                                        {
                                            Console.WriteLine("Remaining time:{0} second", remaintime);
                                        }
                                        else
                                        {
                                            Console.WriteLine("TOTP(s) are expired. regenerating next key.");
                                            status = 3;
                                            answered = true;
                                        }
                                    }
                                    else if (answer == "No")
                                    {
                                        status = 0;
                                        answered = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("wrong input try again");
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        {
                            Console.WriteLine("Enter the name of new key");
                            string inputname = string.Empty + Console.ReadLine();
                            Console.WriteLine("Enter the new key in Base 32");
                            bool keyconfirm = false;
                            while (!keyconfirm)
                            {
                                string inputkey = string.Empty + Console.ReadLine();
                                if (Regex.IsMatch(inputkey, "^([A-Za-z2-7]{8})*(([A-Za-z2-7]{8})|([A-Za-z2-7]{7}={1})|([A-Za-z2-7]{6}={2})|([A-Za-z2-7]{5}={3})|([A-Za-z2-7]{4}={4})|([A-Za-z2-7]{3}={5})|([A-Za-z2-7]{2}={6})|([A-Za-z2-7]{1}={7})){1}(={8})*$"))
                                {
                                    
                                    //fuck cs8602 warning
                                        keys.Add(new string[] { inputname, inputkey });
                                    Console.WriteLine("your key has been added successfully. returning to main menu");
                                    status = 0;
                                    keyconfirm = true;
                                }
                                else
                                {
                                    Console.WriteLine("wrong input format. returning to main menu");
                                    status = 0;
                                    keyconfirm = true;
                                }
                            }
                        }
                        break;
                    case 5:
                        {

                            Console.WriteLine("Enter the name of new key");
                            string inputname = string.Empty + Console.ReadLine();
                            Console.WriteLine("Enter the new key in Base 64");
                            bool keyconfirm = false;
                            while (!keyconfirm)
                            {
                                string inputkey = string.Empty + Console.ReadLine();
                                if (Regex.IsMatch(inputkey, "^([A-Za-z0-9+/]{4})*(([A-Za-z0-9+/]{4})|([A-Za-z0-9+/]{3}={1})|([A-Za-z0-9+/]{2}={2})|([A-Za-z0-9+/]{1}={3})){1}(={4})*$"))
                                {

                                    //fuck cs8602 warning
                                    keys.Add(new string[] { inputname, LibraryForTOTP.RFC4648Base32.ToBase32String(Convert.FromBase64String(inputkey)) });
                                    Console.WriteLine("your key has been added successfully. returning to main menu");
                                    status = 0;
                                    keyconfirm = true;
                                }
                                else
                                {
                                    Console.WriteLine("wrong input format. returning to main menu");
                                    status = 0;
                                    keyconfirm = true;
                                }
                            }
                        }
                        break;
                    case 6:
                        {
                            Console.WriteLine("Keys ...");
                            Console.WriteLine();
                            if (keys == null || keys.Count == 0)
                            {
                                Console.WriteLine("no saved key");
                            }
                            else
                            {
                                int counter = 0;
                                foreach (string[] s in keys)
                                {
                                    try
                                    {
                                        Console.WriteLine("index = {2},name : {0} \t, key {1}", s[0], Convert.ToBase64String(LibraryForTOTP.RFC4648Base32.FromBase32String(s[1])), counter);
                                    }
                                    catch(Exception ex)
                                    {
                                        Console.WriteLine("index = {2},name : {0} \t, key :Error {1}", s[0], ex.ToString(), counter);
                                    }
                                        counter++;
                                }
                            }
                            Console.WriteLine("press any key to return main menu");
                            Console.ReadKey();
                            Console.WriteLine();
                            status = 0;
                        }
                        break;
                    case 7:
                        {
                            Console.Clear();
                            status = 0;
                        }
                        break;
                    case 8:
                        {
                            Console.WriteLine("input key name");
                            string name = string.Empty+Console.ReadLine();
                            Console.WriteLine("input key byte length(default is 10)");
                            int length = 10;
                            try
                            {
                                length = int.Parse(Console.ReadLine()+string.Empty);
                                if (length <= 0) { length = 10; }
                            }
                            catch{}
                            byte[] bytes = new byte[length];
                            RandomNumberGenerator.Fill(bytes);
                            //cs8602 warning
                            keys.Add(new string[] {name,LibraryForTOTP.RFC4648Base32.ToBase32String(bytes)});
                            Console.WriteLine("added a new key to the list");
                            status = 0;
                        }
                        break;
                    case 9:
                        {
                            Console.WriteLine("save " + ExportKey(keys));
                            flag = false;
                        }
                        break;
                    case 10:
                        {
                                Console.WriteLine("Access https://www.npca.jp/about/agreements/ to see the licence agreement");
                            
                            status = 0;
                        }
                        break;
                    default:
                        {
                            Console.WriteLine("wrong input. try again.");
                            status = 0;
                        }
                        break;
                }   
            }
        }
        private static List<string[]> ImportKey(string PathOfKey = "Keys.csv")
        {
                List<string[]> keys = new List<string[]>();
                var keyfile = File.Open(PathOfKey,FileMode.OpenOrCreate);
                keyfile.Close();
                string[] keyarray = File.ReadAllLines(keyfile.Name);
                foreach (string key in keyarray)
                {
                    string[] splited = key.Split('\u002C');
                    keys.Add(splited);
                }
                return keys;   
        }

        private static string ExportKey(List<string[]>? Keys, string PathOfKey = "Keys.csv",string BackupName = "backup.csv")
        {
            try
            {
                string temp = string.Empty;
                if(Keys==null || Keys.Count == 0) {
                    Console.WriteLine("No key in list. deleting the keyfile contents");
                }
                else
                {
                    foreach (string[] Key in Keys)
                    {
                        foreach (string key in Key)
                        {
                            temp += key + ',';
                        }
                        temp = temp.Substring(0, temp.Length - 1);
                        temp += '\n';
                    }
                }
                var keys = File.Open(PathOfKey, FileMode.OpenOrCreate);
                var bkup = File.Open(BackupName, FileMode.OpenOrCreate);
                keys.Close();
                bkup.Close();
                File.Copy(PathOfKey, BackupName,true);
                File.WriteAllText(PathOfKey, temp);
                return "success";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        
    }
    public static class RFC4648Base32
    {
        //requires "using System;" and "using System.Collections.Generic;" on top of code
        public static int CharToInt(char c)
        {
            switch (c) {
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
        public static byte[] FromBase32String(string base32text,char padding = '=')
        {
            if(base32text == null || base32text.Length == 0)
            {
                return Array.Empty<byte>();
            }
            base32text = base32text.Trim().TrimEnd(padding);
            long len = base32text.Length;
            const int cutlength = 8;
            long len2 = len % cutlength == 0 ? len / cutlength : (len / cutlength) + 1;
            string[] splitedtext = new string[len2];
            for(int i = 0; i < splitedtext.Length; i++)
            {
                splitedtext[i] = base32text.Substring(0, cutlength<base32text.Length?cutlength:base32text.Length);
                base32text = base32text.Substring(cutlength < base32text.Length ? cutlength : base32text.Length);
            }
            List<byte> decoded = new List<byte>();
            int len3 = 0;
            int len4 = splitedtext[splitedtext.Length-1].Length;
            
            switch (len4)
            {
                case 1: len3 = 1; break;
                case 2: len3 = 1; break;
                case 3: len3 = 2; break;
                case 4: len3 = 2; break;
                case 5: len3 = 3; break;
                case 6: len3 = 4; break;
                case 7: len3 = 4; break;
                case 8: len3 = 5; break;
            }
            for(int i = 0; i < splitedtext.Length; i++)
            {
                ulong piece = 0;
                for(int j = 0; j < cutlength; j++)
                {
                    
                    piece <<= 5;
                    
                    if (j < splitedtext[i].Length)
                    {
                        piece |= (uint)CharToInt(splitedtext[i][j]);
                    }
                }
                for(int j = 0; j < 5; j++)
                {
                    ulong aaa = (piece>>(4 - j) * 8)&255;
                    if (i != splitedtext.Length-1|j<len3)
                    {
                        decoded.Add((byte)aaa);
                    }
                    
                }
            }
            return decoded.ToArray();
        }
        public const  string table = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        public static string ToBase32String(byte[] data,char padding = '=')
        {
            const uint mask = 31;
            int divideinto= data.Length%5==0?data.Length/5:data.Length/5+1;
            string encoded = string.Empty;
            for (int i=0;i<divideinto;i++)
            {
                ulong temp = 0;
                for(int j = 0; j < 5; j++)
                {
                    temp <<= 8;
                    if (i*5+j <data.Length)
                    {
                        temp |= data[i*5+j];
                    }
                }
                for(int j = 0; j < 8; j++)
                {
                    int finallength=8;
                    switch (data.Length%5) 
                    {
                        case 0: finallength = 8; break;
                        case 1: finallength = 2; break;
                        case 2: finallength = 4; break;
                        case 3: finallength = 5; break;
                        case 4: finallength = 7; break;
                    }

                    if (i<divideinto-1|(i == divideinto-1 && j<finallength))
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
