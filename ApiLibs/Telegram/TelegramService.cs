using ApiLibs.General;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace ApiLibs.Telegram
{
    public class TelegramService : Service
    {
        private List<From> contacts;

        public event MessageHandler MessageRecieved;

        public delegate void MessageHandler(Message m, EventArgs e);

        public Memory mem;

        public string Telegram_token;

        public TelegramService(string token, string applicationDirectory)
        {
            Telegram_token = token;
            SetUp("https://api.telegram.org/bot" + Telegram_token);
            mem = new Memory(applicationDirectory);
            ReadStoredUsernames();
        }

        //TODO
        public async Task GetMe()
        {
            await HandleRequest("/getMe", Call.POST, new List<Param>());
        }

        public async Task SendMessage(string username, string message, ParseMode mode = ParseMode.None, bool webPreview = true, int replyToMessageId = -1)
        {
            await SendMessage(ConvertFromUsernameToID(username), message, mode, webPreview, replyToMessageId);
        }

        public async Task SendMessage(int id, string message, ParseMode mode = ParseMode.None, bool webPreview = true, int replyToMessageId = -1, object replyMarkup = null)
        {
            if (message.Length > 4096)
            {
                message = message.Substring(4090);
            }
            List<Param> param = new List<Param>
            {
                new Param("chat_id", id.ToString()),
                new Param("text", message),
                new Param("disable_web_page_preview", (!webPreview).ToString()),
            };
            switch (mode)
            {
                case ParseMode.HTML:
                    param.Add(new Param("parse_mode", "HTML"));
                    break;
                case ParseMode.Markdown:
                    param.Add(new Param("parse_mode", "Markdown"));
                    break;
            }

            if (replyToMessageId != -1)
            {
                param.Add(new Param("reply_to_message_id", replyToMessageId.ToString()));
            }
            if (replyMarkup != null)
            {
                param.Add(new Param("reply_markup", replyMarkup));
            }

            Message m = await MakeRequest<Message>("/sendMessage", Call.GET, param);
        }

        public int ConvertFromUsernameToID(string userid)
        {
            foreach (From contact in contacts)
            {
                if (contact.username == userid)
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
                try
                {
                    while (true)
                    {
                        try
                        {
                            List<Message> mList = await WaitForNextMessage();
                            foreach (Message m in mList)
                            {
                                MessageRecieved?.Invoke(m, EventArgs.Empty);
                            }
                        }
                        catch (NoInternetException)
                        {
                            Thread.Sleep(TimeSpan.FromMinutes(1));
                        }
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message + " " + exception.StackTrace);
                }
            });
            t.Start();
        }

        private async Task<List<Message>> WaitForNextMessage()
        {
            int updateId = (mem.ReadFile<Result>("data/telegram/lastID")).update_id + 1;

            TelegramMessageObject messages = await MakeRequest<TelegramMessageObject>("/getUpdates", parameters: new List <Param> { new Param("timeout", "10000"), new Param("offset", updateId.ToString()) });
            foreach (Result message in messages.result)
            {
                AddFrom(message.message.from);
            }

            List<Message> result = new List<Message>();

            messages.result.Reverse();
            foreach (Result message in messages.result)
            {
                if (message.update_id == updateId - 1)
                {
                    break;
                }
                result.Add(message.message);
            }

            if (result.Count != 0)
            {
                mem.WriteFile("data/telegram/lastID", messages.result[0]);
            }

            return result;
        }

        public async Task<List<Message>> GetMessages()
        {
            int updateId = mem.ReadFile<Result>("data/telegram/lastID").update_id;
            List<Param> parameters = new List<Param>();
            if (updateId != -1)
            {
                parameters.Add(new Param("offset", updateId.ToString()));
            }
            TelegramMessageObject messages = await MakeRequest<TelegramMessageObject>("/getUpdates", parameters: parameters);
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
                mem.WriteFile("data/telegram/lastID", messages.result[0]);
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
            mem.WriteFile("data/telegram/usernames", contacts);
        }

        private void ReadStoredUsernames()
        {
            contacts = mem.ReadFile<List<From>>("data/telegram/usernames");
        }

        
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ReplyKeyboardMarkup
    {
        public KeyboardButton[][] keyboard { get; set; }
        public bool resize_keyboard { get; set; }
        public bool one_time_keyboard { get; set; }
        public bool selective { get; set; }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class KeyboardButton
    {
        public string text { get; set; }
        public bool request_contact { get; set; }
        public bool request_location { get; set; }
    }

    public enum ParseMode
    {
        None, Markdown, HTML
    }
}
