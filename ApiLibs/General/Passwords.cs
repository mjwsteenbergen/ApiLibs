using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.Telegram;
// ReSharper disable InconsistentNaming

namespace ApiLibs
{
    internal static class Passwords
    {
        public static readonly string DirectoryPath = @"C:\Users\Martijn\AppData\Roaming\Zeus\";

        public static string TodoistKey => GetPassword("TodoistKey");
        public static string TodoistUserAgent => GetPassword("TodoistUserAgent");

        public static string PocketKey => GetPassword("PocketKey");
        public static string Pocket_access_token => GetPassword("Pocket_access_token");

        public static string Telegram_token => GetPassword("Telegram_token");

        public static string GitHub_clientID => GetPassword("GitHub_clientID");
        public static string GitHub_client_secret => GetPassword("GitHub_client_secret");
        public static string GitHub_access_token => GetPassword("GitHub_access_token");

        public static readonly string OutlookKey = "";
        public static readonly string OutlookID = "";

        public static string Travis_Token => GetPassword("Travis_Token");


//        public static string OutlookToken;

        public static readonly string OutlookEmailAdres = "";

        public static string GeneralRedirectUrl { get { return GetPassword("GeneralRedirectUrl"); } }


        public static void ReadPasswords()
        {
            if(!allread)
            {
                passwords = ReadFile<Dictionary<string, string>>("pass");
                allread = true;
            }
        }

        internal static T ReadFile<T>(string filename) where T:new()
        {
            string FilePath = DirectoryPath + filename;

            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            if (File.Exists(FilePath))
            {
                FileStream stream = File.Open(FilePath, FileMode.Open);
                T res;
                using (StreamReader reader = new StreamReader(stream))
                {
                    res = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());

                    stream.Close();
                    reader.Close();
                }

                return res;
            }
            else
            {
                T res = new T();
                WriteFile(filename, res);
                return res;
            }
        }

        internal static void WriteFile(string v, object obj)
        {
            FileStream stream = File.Create(DirectoryPath + v);
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine(JsonConvert.SerializeObject(obj));
                writer.Close();
            }
            stream.Close();
        }

        public static void WritePasswords()
        {
            if (passwords == null)
            {
                passwords = new Dictionary<string, string>();
            }
            WriteFile("pass", passwords);
        }

        public static void AddPassword(string key, string value)
        {
            if (!passwords.Keys.Contains(key))
            {
                passwords.Add(key, value);
                WritePasswords();
            }
        }

        private static string GetPassword(string key)
        {
            if (passwords.Keys.Contains(key))
            {
                return passwords[key];
            }
            return null;
        }

        private static bool allread = false;
        private static Dictionary<string, string> passwords;
        
    }
}
