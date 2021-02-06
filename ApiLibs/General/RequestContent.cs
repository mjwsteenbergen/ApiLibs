using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLibs.General
{
    public abstract class RequestContent
    {
        public abstract string ContentType { get; }
        public RequestContent(string content)
        {
            Content = content;
        }

        public string Content { get; }

    }

    public class HtmlContent : RequestContent
    {
        public HtmlContent(string content) : base(content)
        {
        }

        public override string ContentType => "text/html";

        public static HtmlContent BuildWebpage(string title, string body)
        {
            return new HtmlContent(GetHtml(title, body));
        }

        internal static string GetHtml(string title, string body)
        {
            return $@"<html><head><title>{title}</title><meta name=""created"" content=""{DateTime.Now.ToString()}"" /></head><body>{body}</body></html>";
        }
    }

    public class PlainTextContent : RequestContent
    {
        public PlainTextContent(string content) : base(content)
        {
        }

        public override string ContentType => "text/plain";
    }
}
