using ApiLibs.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiLibs.Telegram
{
    public class TelegramService : Service
    {
        public event MessageHandler MessageRecieved;

        public delegate void MessageHandler(TgMessages m, EventArgs e);

        public Memory mem;

        public string Telegram_token;

        public TelegramService(string token, string applicationDirectory)
        {
            Telegram_token = token;
            SetUp("https://api.telegram.org/bot" + Telegram_token);
            mem = new Memory(applicationDirectory);
        }

        //TODO
        public async Task GetMe()
        {
            await HandleRequest("/getMe", Call.POST, new List<Param>());
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
                param.Add(new Param("reply_markup", JsonConvert.SerializeObject(replyMarkup)));
            }

            await HandleRequest("/sendMessage", Call.GET, param);
        }

        public async Task answerInlineQuery(string inline_query_id, IEnumerable<InlineQueryResultArticle> results)
        {
            try
            {
                await HandleRequest("answerInlineQuery", parameters: new List<Param>() { new Param("inline_query_id", inline_query_id), new Param("results", JsonConvert.SerializeObject(results)) });
            }
            catch (BadRequestException e)
            {
                if (!e.Message.Contains("QUERY_ID_INVALID"))
                {
                    throw;
                }
                else
                {
                    Console.WriteLine(inline_query_id);
                    results.ToList().ForEach(Console.WriteLine);
                }
            }
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
                            TgMessages mList = await GetMessages(1000);
                            MessageRecieved?.Invoke(mList, EventArgs.Empty);
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

        public async Task<TgMessages> GetMessages(int? timeout = null)
        {
            int updateId = mem.ReadFile<Result>("data/telegram/lastID").update_id;

            List<Param> parameters = new List<Param>();

            if (updateId != -1)
            {
                parameters.Add(new Param("offset", updateId.ToString()));
            }

            if (timeout != null)
            {
                parameters.Add(new Param("timeout", timeout.ToString()));
            }

            TgResponseObject messages = await MakeRequest<TgResponseObject>("/getUpdates", parameters: parameters);
            messages.result.Reverse();

            List<TgMessage> tgMessages = new List<TgMessage>();
            List<TgInlineQuery> tgInlineQueries = new List<TgInlineQuery>();

            List<From> alreadyInUse = new List<From>();

            foreach (Result message in messages.result)
            {
                if (message.update_id == updateId)
                {
                    break;
                }
                if (message.IsTgMessage)
                {
                    TgMessage tgMessage = TgResult.Convert<TgMessage>(message.message);
                    tgMessages.Add(tgMessage);
                }
                if (message.IsInlineQuery)
                {
                    if (alreadyInUse.Contains(message.inline_query.from))
                    {
                        continue;
                    }

                    tgInlineQueries.Add(TgResult.Convert<TgInlineQuery>(message.inline_query)); 
                    alreadyInUse.Add(message.inline_query.from);
                }
            }

            TgMessages result = new TgMessages(tgMessages, tgInlineQueries);

            if (result.HasMessages())
            {
                mem.WriteFile("data/telegram/lastID", messages.result[0]);
            }

            return result;
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
