using System;

namespace ApiLibs
{
    public abstract class RequestContent
    {
        public abstract string ContentType { get; }
    }

    public class FileRequestContent : RequestContent
    {
        public override string ContentType => throw new System.NotImplementedException();

        public string Name { get; internal set; }
        public byte[] Bytes { get; internal set; }
    }

    public abstract class TextRequestContent : RequestContent
    {
        public TextRequestContent(string content)
        {
            Content = content;
        }

        public string Content { get; }
    }

    public class HtmlContent : TextRequestContent
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
            return $@"<html><head><title>{title}</title><meta name=""created"" content=""{DateTime.Now}"" /></head><body>{body}</body></html>";
        }
    }

    public class PlainTextContent : TextRequestContent
    {
        public PlainTextContent(string content) : base(content)
        {
        }

        public override string ContentType => "text/plain";
    }
}
