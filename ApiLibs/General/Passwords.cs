using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs
{
    class Passwords
    {
        public static string PasswordPath = @"C:\Users\Martijn\Coding\Zeus\Zeus\bin\Debug\pass";

        public static string TodoistKey { get { return getPassword("TodoistKey"); } }
        public static string TodoistUserAgent { get { return getPassword("TodoistUserAgent"); } }

        public static string PocketKey { get { return getPassword("PocketKey"); } }
        public static string Pocket_access_token { get { return getPassword("Pocket_access_token"); } }

        public static readonly string OutlookKey = "";

        public static readonly string OutlookID  = "";

        public static string OutlookToken;

        public static readonly string OutlookEmailAdres = "";

        public static string GeneralRedirectUrl { get { return getPassword("GeneralRedirectUrl"); } }

        public static void readPasswords()
        {
            FileStream stream = File.Open(PasswordPath, FileMode.OpenOrCreate);
            StreamReader reader = new StreamReader(stream);
            passwords = JsonConvert.DeserializeObject<Dictionary<string,string>>(reader.ReadToEnd());
            allread = true;
        }

        public static void writePasswords()
        {
            FileStream stream = File.Create(PasswordPath);
            if(passwords == null)
            {
                passwords = new Dictionary<string, string>();
            }
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(JsonConvert.SerializeObject(passwords));
            writer.Close();
            stream.Close();
        }

        public static void addPassword(string key, string value)
        {
            if(!passwords.Keys.Contains(key))
            {
                passwords.Add(key, value);
            }
        }

        private static string getPassword(string key)
        {
            if(passwords.Keys.Contains(key))
            {
                return passwords[key];
            }
            return null;
        }

        private static bool allread = false;
        private static Dictionary<string, string> passwords;

    }
}
