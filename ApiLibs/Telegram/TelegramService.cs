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
        private readonly IStorage _storage;
        private List<From> contacts;

        public event MessageHandler MessageRecieved;
        public delegate void MessageHandler(Message m, EventArgs e);

        public string Telegram_token;

        public TelegramService(Passwords pass, IStorage storage)
        {
            _storage = storage;
            Telegram_token = pass.Telegram_token;
        }

        public TelegramService(string token)
        {
            Telegram_token = token;
        }

        public void Connect()
        {
            SetUp("https://api.telegram.org/bot" + Telegram_token);
        }

        //TODO
        public async Task GetMe()
        {
            await MakeRequest("/getMe", Call.POST, new List<Param>());
        }


        public void SendMessage(int id, string message)
        {
            SendMessage(id, message, ParseMode.None, true, -1);
        }

        public void SendMessage(string username, string message)
        {
            SendMessage(username, message, ParseMode.None);
        }

        public void SendMessage(string username, string message, ParseMode mode)
        {
            SendMessage(username, message, mode, true);
        }

        public void SendMessage(string username, string message, ParseMode mode, bool webPreview)
        {
            SendMessage(username, message, mode, webPreview, -1);
        }

        public void SendMessage(string username, string message, ParseMode mode, bool webPreview, int reply_to_message_id)
        {
            SendMessage(ConvertFromUsernameToID(username), message, mode, webPreview, reply_to_message_id);
        }
        
        public async void SendMessage(int id, string message, ParseMode mode, bool webPreview, int reply_to_message_id)
        {
            List<Param> param = new List<Param>
            {
                new Param("chat_id", id.ToString()),
                new Param("text", message),
                new Param("disable_web_page_preview", (!webPreview).ToString()),
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

            if (reply_to_message_id != -1)
            {
                param.Add(new Param("reply_to_message_id", reply_to_message_id.ToString()));
            }

            await MakeRequest("/sendMessage", Call.GET, param);
        }

        public int ConvertFromUsernameToID(string userid)
        {
            foreach(From contact in contacts)
            {
                if(contact.username == userid)
                {
                    return contact.id;
                }
            }
            throw new KeyNotFoundException(userid + "was not found. Did you type it correctly?");
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
            int updateId = (await _storage.Read<Result>("data/telegram/lastID")).update_id;

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
                _storage.Write("data/telegram/lastID", messages.result[0]);
            }

            return result;
        }

        public async Task<List<Message>> GetMessages()
        {
            int updateId = (await _storage.Read<Result>("data/telegram/lastID")).update_id;

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
                _storage.Write("data/telegram/lastID", messages.result[0]);
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
            _storage.Write("data/telegram/usernames", contacts);
        }

        private async void ReadStoredUsernames()
        {
            contacts = await _storage.Read<List<From>>("data/telegram/usernames");
        }

        
    }

    public enum ParseMode
    {
        None, Markdown, HTML
    }
}
