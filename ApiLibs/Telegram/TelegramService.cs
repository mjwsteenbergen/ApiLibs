using ApiLibs.General;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiLibs.Telegram
{
    public class TelegramService : RestSharpService
    {
        public string Telegram_token;

        public TelegramService(string token, string applicationDirectory) : base("https://api.telegram.org/bot")
        {
            Telegram_token = token;
            SetBaseUrl("https://api.telegram.org/bot" + Telegram_token);
        }

        public async Task<TgMessage> SendMessage(int id, string message, ParseMode? mode = null, bool webPreview = true, int? replyToMessageId = null, ReplyKeyboardMarkup replyMarkup = null, bool? disableNotification = null)
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
            var req = await MakeRequest<OKResponse<string>, BadRequestResponse<string>>("answerInlineQuery", parameters: new List<Param>
                {
                    new Param("inline_query_id", inlineQueryId),
                    new Param("results", results)
            });

            req.Switch(
                (s) => {},
                (s) =>
                {
                    if (!s.Content().Contains("QUERY_ID_INVALID"))
                    {
                        throw new RequestException(s.Response);
                    }
                    else
                    {
                        Console.WriteLine(inlineQueryId);
                        results.ToList().ForEach(Console.WriteLine);
                    }
                }
            );
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

    public static class TelegramServiceExtension {
        public static string EscapeMarkdown(this string input)
        {
            foreach (char c in new char[] {'_', '*', '`'})
            {
                input = input?.Replace(c.ToString(), "\\" + c);
            }

            return input;
        }
    }
}
