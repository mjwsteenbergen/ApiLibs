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
    public class Passwords
    {
        public Passwords(IStorage storage)
        {
            store = storage;
        }

        internal static string TodoistKey => GetPassword("TodoistKey");
        internal static string TodoistUserAgent => GetPassword("TodoistUserAgent");

        internal static string PocketKey => GetPassword("PocketKey");
        internal static string Pocket_access_token => GetPassword("Pocket_access_token");

        internal static string Telegram_token => GetPassword("Telegram_token");

        internal static string GitHub_clientID => GetPassword("GitHub_clientID");
        internal static string GitHub_client_secret => GetPassword("GitHub_client_secret");
        internal static string GitHub_access_token => GetPassword("GitHub_access_token");

        internal static readonly string OutlookKey = "";
        internal static readonly string OutlookID = "";

        internal static string Travis_Token => GetPassword("Travis_Token");

        internal static readonly string OutlookEmailAdres = "";

        internal static string GeneralRedirectUrl => GetPassword("GeneralRedirectUrl");
        internal static string WunderlistToken => GetPassword("WunderlistToken");
        internal static string WunderlistId => GetPassword("WunderlistId");
        internal static string WunderlistSecret => GetPassword("WunderlistSecret");

        internal static IStorage store;

        public static async Task ReadPasswords()
        {
            if(!allread)
            {
                passwords = await ReadFile<Dictionary<string, string>>("pass");
                allread = true;
            }
        }

        internal static async Task<T> ReadFile<T>(string filename) where T:new()
        {
            if (store == null)
            {
                throw new NullReferenceException(
                    "You have not declared an StorageClass please define it, or disable the storage by calling disable storage");
            }

            string readFile = await store.Read(filename);

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

        internal static void WriteFile(string v, object obj)
        {
            store.Write(v, obj);
        }

        internal static void WritePasswords()
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

        internal static void AddPassword(string key, string value)
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

        private static string GetPassword(string key)
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
