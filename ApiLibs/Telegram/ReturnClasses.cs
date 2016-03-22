﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public class Message
    {
        public int message_id { get; set; }
        public From from { get; set; }
        public Chat chat { get; set; }
        public int date { get; set; }
        public string text { get; set; }
        public User forward_from { get; set; }
        public int forward_date { get; set; }
        public Message reply_to_message { get; set; }
        public List<Photo> photo { get; set; }
        public Document document { get; set; }
        public Contact contact { get; set; }
        public Sticker sticker { get; set; }

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

    public class Result
    {
        public int update_id { get; set; }
        public Message message { get; set; }
    }

    public class TelegramMessageObject
    {
        public bool ok { get; set; }
        public List<Result> result { get; set; }
    }
}