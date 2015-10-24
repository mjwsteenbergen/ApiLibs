using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.Telegram
{
    public class TelegramService : Service
    {
        private List<From> contacts;

        public TelegramService()
        {
            SetUp("https://api.telegram.org/bot");
            setBaseUrl("https://api.telegram.org/bot" + Passwords.Telegram_token);
            readStoredUsernames();
        }

        public async void GetMe()
        {
            var request = await MakeRequestPost("/getMe", new List<Param>());
        }

        public async void SendMessage(int id, string message)
        {
            List<Param> param = new List<Param>();
            param.Add(new Param("chat_id", id.ToString()));
            param.Add(new Param("text", message));

            var request = await MakeRequest("/sendMessage", param);
        }

        public void SendMessage(string userid, string message)
        {
            foreach(From contact in contacts)
            {
                if(contact.username == userid)
                {
                    SendMessage(contact.id, message);
                    return;
                }
            }
        }

        public async Task<TelegramMessageObject> getMessages()
        {
            var request = await MakeRequest("/getUpdates", new List<Param>());
            TelegramMessageObject messages = await JsonConvert.DeserializeObjectAsync<TelegramMessageObject>(request.Content);
            foreach(Result message in messages.result)
            {
                addFrom(message.message.from);
            }
            return messages;
        }

        private void addFrom(From from)
        {
            if (!contacts.Contains(from))
            {
                contacts.Add(from);
            }
            writeUsernames();
        }

        private void writeUsernames()
        {
            Passwords.writeFile("telegram", contacts);
        }

        private void readStoredUsernames()
        {
            contacts = Passwords.readFile<List<From>>("telegram");
        }
    }
}
