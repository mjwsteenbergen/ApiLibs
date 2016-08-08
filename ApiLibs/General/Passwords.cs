
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace ApiLibs.General
{
    public class Passwords
    {
        public Passwords(string baseUrl)
        {
            mem = new Memory(baseUrl);
        }

        internal string TodoistKey => GetPassword("TodoistKey");
        internal string TodoistUserAgent => GetPassword("TodoistUserAgent");

        internal string PocketKey => GetPassword("PocketKey");
        internal string Pocket_access_token => GetPassword("Pocket_access_token");

        internal string Telegram_token => GetPassword("Telegram_token");

        internal string GitHub_clientID => GetPassword("GitHub_clientID");
        internal string GitHub_client_secret => GetPassword("GitHub_client_secret");
        internal string GitHub_access_token => GetPassword("GitHub_access_token");

        internal readonly string OutlookKey = "";
        internal readonly string OutlookID = "";

        internal string Travis_Token => GetPassword("Travis_Token");

        internal readonly string OutlookEmailAdres = "";

        internal string GeneralRedirectUrl => GetPassword("GeneralRedirectUrl");
        internal string WunderlistToken => GetPassword("WunderlistToken");
        internal string WunderlistId => GetPassword("WunderlistId");
        internal string WunderlistSecret => GetPassword("WunderlistSecret");

        internal Memory mem;

        public void ReadPasswords()
        {
            if(!allread)
            {
                passwords = mem.ReadFile<Dictionary<string, string>>("KeyFile.pass");
                allread = true;
            }
        }

        internal T ReadFile<T>(string filename) where T:new()
        {

            string readFile = mem.ReadFile(filename);

            T res;
            if (readFile != "")
            {
                res = JsonConvert.DeserializeObject<T>(readFile);
            }
            else
            {
                res = new T();
                WriteFile(filename, res);
            }
            return res;
        }

        internal void WriteFile(string v, object obj)
        {
            mem.WriteFile(v, obj);
        }

        internal void WritePasswords()
        {
            if (!allread)
            {
                throw new OperationCanceledException(
                    "This is not allowed until the passwords have been read with ReadPasswords()");
            }
            if (passwords == null)
            {
                passwords = new Dictionary<string, string>();
            }
            WriteFile("pass", passwords);
        }

        public void AddPassword(string key, string value)
        {
            if (!allread)
            {
                throw new OperationCanceledException(
                    "This is not allowed until the passwords have been read with ReadPasswords()");
            }
            if (!passwords.Keys.Contains(key))
            {
                passwords.Add(key, value);
                WritePasswords();
            }
        }

        private string GetPassword(string key)
        {
            if (!allread)
            {
                throw new OperationCanceledException(
                    "This is not allowed until the passwords have been read with ReadPasswords()");
            }
            if (passwords.Keys.Contains(key))
            {
                return passwords[key];
            }
            return null;
        }

        private static bool allread = false;
        private static Dictionary<string, string> passwords;
        
    }

    public interface IStorage
    {
        Task<T> Read<T>(string fileName);
        Task<string> Read(string fileName);
        void Write(string fileName, object obj);
    }
}
