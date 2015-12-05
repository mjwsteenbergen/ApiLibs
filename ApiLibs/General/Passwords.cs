using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.Telegram;

namespace ApiLibs
{
    class Passwords
    {
        public static string DirectoryPath = @"C:\Users\Martijn\AppData\Roaming\Zeus\";

        public static string TodoistKey { get { return getPassword("TodoistKey"); } }
        public static string TodoistUserAgent { get { return getPassword("TodoistUserAgent"); } }

        public static string PocketKey { get { return getPassword("PocketKey"); } }
        public static string Pocket_access_token { get { return getPassword("Pocket_access_token"); } }

        public static string Telegram_token { get { return getPassword("Telegram_token"); } }

        public static string GitHub_clientID { get { return getPassword("GitHub_clientID"); } }
        public static string GitHub_client_secret { get { return getPassword("GitHub_client_secret"); } }
        public static string GitHub_access_token { get { return getPassword("GitHub_access_token"); } }

        public static readonly string OutlookKey = "";
        public static readonly string OutlookID = "";

        public static string OutlookToken;

        public static readonly string OutlookEmailAdres = "";

        public static string GeneralRedirectUrl { get { return getPassword("GeneralRedirectUrl"); } }
        public static string WunderlistToken => getPassword("WunderlistToken");
        public static string WunderlistId => getPassword("WunderlistId");
        public static string WunderlistSecret => getPassword("WunderlistSecret");

        public static void readPasswords()
        {
            if(!allread)
            {
                passwords = readFile<Dictionary<string, string>>("pass");
                allread = true;
            }
        }

        internal static T readFile<T>(string filename) where T:new()
        {
            string FilePath = DirectoryPath + filename;

            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            if (File.Exists(FilePath))
            {
                FileStream stream = File.Open(FilePath, FileMode.Open);
                StreamReader reader = new StreamReader(stream);

                T res = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());

                stream.Close();
                reader.Close();

                return res;
            }
            else
            {
                T res = new T();
                //File.Create(FilePath);
                writeFile(filename, res);
                return res;
            }
        }

        internal static void writeFile(string v, object obj)
        {
            FileStream stream = File.Create(DirectoryPath + v);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(JsonConvert.SerializeObject(obj));
            writer.Close();
            stream.Close();
        }

        public static void writePasswords()
        {
            if (passwords == null)
            {
                passwords = new Dictionary<string, string>();
            }
            writeFile("pass", passwords);
        }

        public static void addPassword(string key, string value)
        {
            if (!passwords.Keys.Contains(key))
            {
                passwords.Add(key, value);
            }
        }

        private static string getPassword(string key)
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
