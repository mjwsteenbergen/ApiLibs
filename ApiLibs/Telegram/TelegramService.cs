using ApiLibs.General;
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

        public TelegramService(string token, string applicationDirectory) : base("https://api.telegram.org/bot")
        {
            Telegram_token = token;
            SetBaseUrl("https://api.telegram.org/bot" + Telegram_token);
            mem = new Memory(applicationDirectory);
        }

        public async Task<TgMessages> GetMessages(int? timeout = null)
        {
            int? updateId = mem.ReadFile<Update>("data/telegram/lastID").update_id;

            TgUpdateObject messages = await MakeRequest<TgUpdateObject>("/getUpdates", parameters: new List<Param>
            {
                new OParam("timeout", timeout.ToString()),
                new OParam("offset", updateId.ToString())
            });
            messages.result.Reverse();

            List<TgMessage> tgMessages = new List<TgMessage>();
            List<TgInlineQuery> tgInlineQueries = new List<TgInlineQuery>();
            List<ChosenInlineResult> chosenInlineResults = new List<ChosenInlineResult>();

            List<From> inlineQueryHandled = new List<From>();

            foreach (Update message in messages.result)
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
                    if (inlineQueryHandled.Contains(message.inline_query.from))
                    {
                        continue;
                    }

                    tgInlineQueries.Add(TgResult.Convert<TgInlineQuery>(message.inline_query));
                    inlineQueryHandled.Add(message.inline_query.from);
                }
                if (message.IsChosenInlineResult)
                {
                    chosenInlineResults.Add(message.chosen_inline_result);
                }
            }

            TgMessages result = new TgMessages(tgMessages, tgInlineQueries, chosenInlineResults);

            if (result.HasMessages())
            {
                mem.WriteFile("data/telegram/lastID", messages.result[0]);
            }

            return result;
        }

        public async Task<TgMessage> SendMessage(int id, string message, ParseMode? mode = null, bool webPreview = true, int? replyToMessageId = null, object replyMarkup = null, bool? disableNotification = null)
        {
            if (message.Length > 4096)
            {
                message = message.Substring(4090);
            }

            return (await MakeRequest<TgSendUpdateObject>("/sendMessage", Call.GET, new List<Param>
            {
                new Param("chat_id", id.ToString()),
                new Param("text", message),
                new Param("disable_web_page_preview", (!webPreview).ToString()),
                new OParam("parse_mode", mode.ToString()),
                new OParam("reply_to_message_id", replyToMessageId),
                new OParam("reply_markup", replyMarkup),
                new OParam("disable_notification", disableNotification)
            })).result;
        }

        public async Task AnswerInlineQuery(string inlineQueryId, IEnumerable<InlineQueryResultArticle> results)
        {
            try
            {
                await HandleRequest("answerInlineQuery", parameters: new List<Param>
                {
                    new Param("inline_query_id", inlineQueryId),
                    new Param("results", results)
                });
            }
            catch (BadRequestException e)
            {
                if (!e.Message.Contains("QUERY_ID_INVALID"))
                {
                    throw;
                }
                else
                {
                    Console.WriteLine(inlineQueryId);
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

        

        public async Task<TgMessage> EditMessageText(TgMessage message, string newText, ParseMode? mode = null, bool? disableWebPagePreview = null)
        {
            return await EditMessageText(newText, message.chat.id, message.message_id, null, mode, disableWebPagePreview);
        }

        public async Task<TgMessage> EditMessageText(string text, int chatId, int messageId, int? inlineMessageId = null, ParseMode? mode = null, bool? disableWebPagePreview = null)
        {
            return await MakeRequest<TgMessage>("/editMessageText", parameters: new List<Param>
            {
                new Param("text", text),
                new Param("chat_id", chatId),
                new Param("message_id", messageId),
                new OParam("parse_mode", mode),
                new OParam("disable_web_page_preview", disableWebPagePreview)
            });
        }

        public async Task<TgMessage> EditMessageText(string text, int inlineMessageId, ParseMode? mode = null, bool? disableWebPagePreview = null)
        {
            return await MakeRequest<TgMessage>("/editMessageText", parameters: new List<Param>
            {
                new Param("text", text),
                new Param("inline_message_id", inlineMessageId),
                new OParam("parse_mode", mode),
                new OParam("disable_web_page_preview", disableWebPagePreview)
            });
        }
    }
}
