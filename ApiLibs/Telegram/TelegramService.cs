using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiLibs.Telegram
{
    public class TelegramService : Service
    {
        private List<From> contacts;

        public event MessageHandler MessageRecieved;
        public delegate void MessageHandler(Message m, EventArgs e);

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
            await MakeRequest("/getMe", Call.POST, new List<Param>());
        }

        public async void SendMessage(int id, string message)
        {
            SendMessage(id, message, ParseMode.None);
        }

        public void SendMessage(string userid, string message)
        {
            SendMessage(userid, message, ParseMode.None);
        }

        public async void SendMessage(int id, string message, ParseMode mode)
        {
            List<Param> param = new List<Param>
            {
                new Param("chat_id", id.ToString()),
                new Param("text", message),
            };
            switch (mode)
            {
                case ParseMode.HTML:
                    param.Add(new Param("parse_mode","HTML"));
                    break;
                case ParseMode.Markdown:
                    param.Add(new Param("parse_mode", "Markdown"));
                    break;
            }

            await MakeRequest("/sendMessage", Call.GET, param);
        }

        public void SendMessage(string userid, string message, ParseMode mode)
        {
            foreach(From contact in contacts)
            {
                if(contact.username == userid)
                {
                    SendMessage(contact.id, message, mode);
                    return;
                }
            }
        }

        public void LookForMessages()
        {
            Thread t = new Thread(async () =>
            {
                while (true)
                {
                    List<Message> mList = await WaitForNextMessage();
                    foreach (Message m in mList)
                    {
                        MessageRecieved.Invoke(m, EventArgs.Empty);
                    }
                }
            });
            t.Start();
        }

        private async Task<List<Message>> WaitForNextMessage()
        {
            int updateId = Passwords.ReadFile<Result>("data/telegram/lastID").update_id;

            TelegramMessageObject messages = await MakeRequest<TelegramMessageObject>("/getUpdates", new List<Param> { new Param("timeout", "100"), new Param("offset", updateId.ToString()) });
            foreach (Result message in messages.result)
            {
                AddFrom(message.message.from);
            }

            List<Message> result = new List<Message>();

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

        public async Task<List<Message>> GetMessages()
        {
            int updateId = Passwords.ReadFile<Result>("data/telegram/lastID").update_id;

            TelegramMessageObject messages = await MakeRequest<TelegramMessageObject>("/getUpdates", new List<Param> { new Param("offset",updateId.ToString()) });
            foreach(Result message in messages.result)
            {
                AddFrom(message.message.from);
            }

            List<Message> result = new List<Message>();

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

    public enum ParseMode
    {
        None, Markdown, HTML
    }
}
