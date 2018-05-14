using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace ApiLibs.Telegram
{
    public class InlineQueryResult
    {
        public string type { get; set; }
        public string id { get; set; }
        public string title { get; set; }
    }

    public class InlineQueryResultArticle : InlineQueryResult
    {
        public InputTextMessageContent input_message_content { get; set; }

        public InlineQueryResultArticle()
        {
            type = "article";
        }

        public override string ToString()
        {
            return id + ":" + title;
        }
    }

    public class InlineQueryResultPhoto : InlineQueryResult
    {
        public string photo_url { get; set; }
        public string thumb_url { get; set; }
        public int photo_width { get; set; }
        public int photo_height { get; set; }
        public string description { get; set; }
        public string caption { get; set; }

        public InlineQueryResultPhoto()
        {
            type = "photo";
        }
    }

    public class InlineQueryResultGif : InlineQueryResult
    {
        public string gif_url { get; set; }
        public int gif_width { get; set; }
        public int gif_height { get; set; }
        public int gif_duration { get; set; }
        public string caption { get; set; }

        public InlineQueryResultGif()
        {
            type = "gif";
        }
    }

    public class InlineQueryResultMpeg4Gif : InlineQueryResult
    {
        public string mpeg4_url { get; set; }
        public int mpeg4_width { get; set; }
        public int mpeg4_height { get; set; }
        public int mpeg4_duration { get; set; }
        public string thumb_url { get; set; }
        public string caption { get; set; }

        public InlineQueryResultMpeg4Gif()
        {
            type = "mpeg4_gif";
        }
    }

    public class InlineQueryResultVideo : InlineQueryResult
    {
        public string video_url { get; set; }
        public string mime_type { get; set; }
        public string thumb_url { get; set; }
        public string caption { get; set; }
        public int video_width { get; set; }
        public int video_height { get; set; }
        public int video_duration { get; set; }
        public string description { get; set; }

        public InlineQueryResultVideo()
        {
            type = "video";
        }
    }

    public class InlineQueryResultAudio : InlineQueryResult
    {
        public string audio_url { get; set; }
        public string caption { get; set; }
        public string performer { get; set; }
        public int audio_duration { get; set; }

        public InlineQueryResultAudio()
        {
            type = "audio";
        }
    }

    public class InlineQueryResultVoice : InlineQueryResult
    {
        public string voice_url { get; set; }
        public string caption { get; set; }
        public int voice_duration { get; set; }

        public InlineQueryResultVoice()
        {
            type = "voice";
        }
    }

    public class InlineQueryResultDocument : InlineQueryResult
    {
        public string document_url { get; set; }
        public string caption { get; set; }
        public string mime_type { get; set; }
        public string description { get; set; }
        public string thumb_url { get; set; }
        public int thumb_width { get; set; }
        public int thumb_height { get; set; }


        public InlineQueryResultDocument()
        {
            type = "document";
        }
    }

    public class InlineQueryResultLocation : InlineQueryResult
    {
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string thumb_url { get; set; }
        public int thumb_width { get; set; }
        public int thumb_height { get; set; }


        public InlineQueryResultLocation()
        {
            type = "location";
        }
    }

    public class InlineQueryResultVenue : InlineQueryResult
    {
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string adress { get; set; }
        public string foursquare_id { get; set; }
        public string thumb_url { get; set; }
        public int thumb_width { get; set; }
        public int thumb_height { get; set; }


        public InlineQueryResultVenue()
        {
            type = "venue";
        }
    }

    public class InlineQueryResultContact : InlineQueryResult
    {
        public string phone_number { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string thumb_url { get; set; }
        public int thumb_width { get; set; }
        public int thumb_height { get; set; }


        public InlineQueryResultContact()
        {
            type = "contact";
        }
    }

    public class InlineQueryResultGame : InlineQueryResult
    {
        public string game_short_name { get; set; }

        public InlineQueryResultGame()
        {
            type = "game";
        }
    }

    public class ReplyKeyboardMarkup
    {
        public ReplyKeyboardMarkup() {
            one_time_keyboard = true;
        }

        public KeyboardButton[][] keyboard { get; set; }
        public bool resize_keyboard { get; set; }
        public bool one_time_keyboard { get; set; }
        public bool selective { get; set; }
    }

    public class KeyboardButton
    {
        public string text { get; set; }
        public bool request_contact { get; set; }
        public bool request_location { get; set; }
    }

    public class HideKeyBoard : ReplyKeyboardMarkup
    {
        public bool hideKeyboard { get; set; }
    }

    public enum ParseMode
    {
        Markdown, HTML
    }
}
