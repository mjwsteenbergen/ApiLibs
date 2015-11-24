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
            SetBaseUrl("https://api.telegram.org/bot" + Passwords.Telegram_token);
            ReadStoredUsernames();
        }

        public void Connect()
        {
            // Telegram Service does not need to connect
        }

        //TODO
        public async void GetMe()
        {
            await MakeRequestPost("/getMe", new List<Param>());
        }

        public async void SendMessage(int id, string message)
        {
            List<Param> param = new List<Param>();
            param.Add(new Param("chat_id", id.ToString()));
            param.Add(new Param("text", message));

            await MakeRequest("/sendMessage", param);
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

        public async Task<List<Message>> GetMessages()
        {
            TelegramMessageObject messages = await MakeRequest<TelegramMessageObject>("/getUpdates", new List<Param>());
            foreach(Result message in messages.result)
            {
                AddFrom(message.message.from);
            }

            List<Message> result = new List<Message>();

            int updateId = Passwords.ReadFile<Result>("data/telegram/lastID").update_id;
            messages.result.Reverse();
            foreach (Result message in messages.result)
            {
                if (message.update_id == updateId)
                {
                    break;
                }
                result.Add(message.message);
            }

            if (result.Count != 0)
            {
                Passwords.WriteFile("data/telegram/lastID", messages.result[0]);
            }

            return result;
        }

        private void AddFrom(From from)
        {
            if (!contacts.Contains(from))
            {
                contacts.Add(from);
            }
            WriteUsernames();
        }

        private void WriteUsernames()
        {
            Passwords.WriteFile("data/telegram/usernames", contacts);
        }

        private void ReadStoredUsernames()
        {
            contacts = Passwords.ReadFile<List<From>>("data/telegram/usernames");
        }

        
    }
}
