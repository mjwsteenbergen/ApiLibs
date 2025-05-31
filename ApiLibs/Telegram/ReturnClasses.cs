﻿using System.Collections.Generic;
using System.Reflection;
// ReSharper disable InconsistentNaming
#pragma warning disable 659

namespace ApiLibs.Telegram
{
    public class From
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }

        public override bool Equals(object obj)
        {
            From other = obj as From;
            if (other == null) 
                return false;
            return (id == other.id) && (first_name == other.first_name) && (last_name == other.last_name) &&
                   (username == other.username);
        }
    }

    public class Chat
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
    }

    public class Photo
    {
        public string file_id { get; set; }
        public int file_size { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Thumb
    {
    }

    public class Document
    {
        public string file_name { get; set; }
        public string mime_type { get; set; }
        public Thumb thumb { get; set; }
        public string file_id { get; set; }
        public int file_size { get; set; }
    }

    /// <summary>
    /// The contact that is shared with the bot
    /// </summary>
    public class Contact
    {
        public string phone_number { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int user_id { get; set; }
    }

    public class Thumb2
    {
        public string file_id { get; set; }
        public int file_size { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Sticker
    {
        public int width { get; set; }
        public int height { get; set; }
        public Thumb2 thumb { get; set; }
        public string file_id { get; set; }
        public int file_size { get; set; }
    }

    public class ConvertableMessage
    {
        public int message_id { get; set; }
        public string id { get; set; }
        public From from { get; set; }
        public Chat chat { get; set; }
        public int date { get; set; }
        public string text { get; set; }
        public User forward_from { get; set; }
        public int forward_date { get; set; }
        public TgMessage reply_to_message { get; set; }
        public string query { get; set; }
        public bool isMessage => text != null;

        public override string ToString()
        {
            return from.username + ": " + text;
        }
    }

    public class User
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
    }

    public class Update
    {
        public int? update_id { get; set; }
        public ConvertableMessage message { get; set; }
        public ConvertableMessage inline_query { get; set; }
        public ChosenInlineResult chosen_inline_result { get; set; }

        public bool IsTgMessage => message != null;
        public bool IsInlineQuery => inline_query != null;
        public bool IsChosenInlineResult => chosen_inline_result != null;
    }

    public class ChosenInlineResult
    {
        public string result_id { get; set; }
        public User from { get; set; }
        public TgLocation location { get; set; }
        public string inline_message_id { get; set; }
        public string query { get; set; }
    }

    public class TgLocation
    {
        public float longitude { get; set; }
        public float latitude { get; set; }
    }

    public class TgUpdateObject
    {
        public bool ok { get; set; }
        public List<Update> result { get; set; }
    }

    public class TgSendUpdateObject
    {
        public bool ok { get; set; }
        public TgMessage result { get; set; }
    }

    public class TgMessages
    {
        public List<TgInlineQuery> tgInlineQueries { get; }
        public List<ChosenInlineResult> ChosenInlineResults { get; }
        public List<TgMessage> Messages { get; }

        public TgMessages(List<TgMessage> tgMessageses, List<TgInlineQuery> tgInlineQueries, List<ChosenInlineResult> chosenInlineResults)
        {
            Messages = tgMessageses;
            this.tgInlineQueries = tgInlineQueries;
            ChosenInlineResults = chosenInlineResults;
        }

        public TgMessages()
        {
            Messages = new List<TgMessage>();
            ChosenInlineResults = new List<ChosenInlineResult>();
            tgInlineQueries = new List<TgInlineQuery>();
        }

        public bool HasMessages()
        {
            return Messages.Count != 0 | tgInlineQueries.Count != 0 | ChosenInlineResults.Count != 0;
        }
    }

    public class TgResult
    {
        public int message_id { get; set; }

        public static T Convert<T>(ConvertableMessage message) where T:new()
        {
            T t = new T();
            PropertyInfo[] propertyInfos = typeof(ConvertableMessage).GetProperties();
            var toSetProperties = typeof(T).GetProperties();
            foreach (PropertyInfo toSet in toSetProperties)
            {
                foreach (PropertyInfo info in propertyInfos)
                {
                    if (info.GetMethod.Name.Replace("get_", "") == toSet.SetMethod?.Name.Replace("set_",""))
                    {
                        toSet.SetValue(t, info.GetValue(message));
                        break;
                    }
                }
            }

            return t;
        }

    }

    public class TgMessage : TgResult
    {
        public From from { get; set; }
        public Chat chat { get; set; }
        public int date { get; set; }
        public string text { get; set; }
        public User forward_from { get; set; }
        public int forward_date { get; set; }
        public TgMessage reply_to_message { get; set; }
        public List<Photo> photo { get; set; }
        public Document document { get; set; }
        public Contact contact { get; set; }
        public Sticker sticker { get; set; }

        public override string ToString()
        {
            return from.username + ": " + text;
        }
    }


    public class TgInlineQuery : TgResult
    {
        public string id { get; set; }
        public From from { get; set; }
        public string query { get; set; }
        public string offset { get; set; }
    }

    

    public class InputTextMessageContent
    {
        public string message_text { get; set; }
        public ParseMode? parse_mode { get; set; } = null;
        public bool? disable_web_page_preview { get; set; } = null;
    }
}
