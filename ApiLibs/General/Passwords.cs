
using Martijn.Extensions.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace ApiLibs.General
{
    public class Passwords
    {
        private static readonly string FileName = "KeyFile.pass";
        private Passwords(Dictionary<string, string> dictionary)
        {
            this.passwords = dictionary;
        }
        private readonly Dictionary<string, string> passwords;

        public string GetPasssword(string name)
        {
            if (passwords.ContainsKey(name))
            {
                return passwords[name];
            }
            else
            {
                return null;
            }
        }


        public void AddPassword(string key, string value)
        {
            if (passwords.ContainsKey(key))
            {
                passwords.Remove(key);
            }
            passwords.Add(key, value);
        }

        public string GitHub_clientID { get => GetPasssword("GitHub_clientID"); set => AddPassword("GitHub_clientID", value); }
        public string GitHub_client_secret { get => GetPasssword("GitHub_client_secret"); set => AddPassword("GitHub_client_secret", value); }
        public string GitHub_access_token { get => GetPasssword("GitHub_access_token"); set => AddPassword("GitHub_access_token", value); }

        public string Instaper_ID { get => GetPasssword("Instaper_ID"); set => AddPassword("Instaper_ID", value); }
        public string Instaper_secret { get => GetPasssword("Instaper_secret"); set => AddPassword("Instaper_secret", value); }
        public string Instaper_user_token { get => GetPasssword("Instaper_user_token"); set => AddPassword("Instaper_user_token", value); }
        public string Instaper_user_secret { get => GetPasssword("Instaper_user_secret"); set => AddPassword("Instaper_user_secret", value); }

        public string OutlookClientSecret { get => GetPasssword("OutlookClientSecret"); set => AddPassword("OutlookClientSecret", value); }
        public string OutlookClientID { get => GetPasssword("OutlookClientID"); set => AddPassword("OutlookClientID", value); }
        public string OutlookAccessToken { get => GetPasssword("OutlookAccessToken"); set => AddPassword("OutlookAccessToken", value); }
        public string OutlookEmail { get => GetPasssword("OutlookEmail"); set => AddPassword("OutlookEmail", value); }
        public string OutlookRefreshToken { get => GetPasssword("OutlookRefreshToken"); set => AddPassword("OutlookRefreshToken", value); }

        public string PocketKey { get => GetPasssword("PocketKey"); set => AddPassword("PocketKey", value); }
        public string Pocket_access_token { get => GetPasssword("Pocket_access_token"); set => AddPassword("Pocket_access_token", value); }

        public string RedditClient { get => GetPasssword("RedditClient"); set => AddPassword("RedditClient", value); }
        public string RedditSecret { get => GetPasssword("RedditSecret"); set => AddPassword("RedditSecret", value); }
        public string RedditRefreshToken { get => GetPasssword("RedditRefreshToken"); set => AddPassword("RedditRefreshToken", value); }
        public string RedditUser { get => GetPasssword(nameof(RedditUser)); set => AddPassword(nameof(RedditUser), value); }

        public string SpotifyClientId { get => GetPasssword("SpotifyClientId"); set => AddPassword("SpotifyClientId", value); }
        public string SpotifySecret { get => GetPasssword("SpotifySecret"); set => AddPassword("SpotifySecret", value); }
        public string SpotifyRefreshToken { get => GetPasssword("SpotifyRefreshToken"); set => AddPassword("SpotifyRefreshToken", value); }

        public string Telegram_token { get => GetPasssword("Telegram_token"); set => AddPassword("Telegram_token", value); }

        public string TodoistKey { get => GetPasssword("TodoistKey"); set => AddPassword("TodoistKey", value); }
        public string TodoistUserAgent { get => GetPasssword("TodoistUserAgent"); set => AddPassword("TodoistUserAgent", value); }

        public string Travis_Token { get => GetPasssword("Travis_Token"); set => AddPassword("Travis_Token", value); }

        public string TraktId { get => GetPasssword("TraktId"); set => AddPassword("TraktId", value); }
        public string TraktSecret { get => GetPasssword("TraktSecret"); set => AddPassword("TraktSecret", value); }
        public string TraktRefreshToken { get => GetPasssword("TraktRefreshToken"); set => AddPassword("TraktRefreshToken", value); }
        public string TraktAccessToken { get => GetPasssword("TraktAccessToken"); set => AddPassword("TraktAccessToken", value); }

        public string GeneralRedirectUrl { get => GetPasssword("GeneralRedirectUrl"); set => AddPassword("GeneralRedirectUrl", value); }
        public string WunderlistToken { get => GetPasssword("WunderlistToken"); set => AddPassword("WunderlistToken", value); }
        public string WunderlistId { get => GetPasssword("WunderlistId"); set => AddPassword("WunderlistId", value); }
        public string WunderlistSecret { get => GetPasssword("WunderlistSecret"); set => AddPassword("WunderlistSecret", value); }

        internal AsyncMemory mem;

        public Task WriteToFile()
        {
            return mem.Write(Passwords.FileName, passwords);
        }

        public static async Task<Passwords> ReadPasswords(AsyncMemory mem = null)
        {
            mem ??= new Memory
            {
                Application = "Laurentia"
            };
            Passwords passwords = new Passwords(await mem.Read<Dictionary<string, string>>(Passwords.FileName))
            {
                mem = mem
            };
            return passwords;
        }


        
    }
}
