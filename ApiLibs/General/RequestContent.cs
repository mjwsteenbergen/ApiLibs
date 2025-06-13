using System;
using System.IO;
using System.Linq;

namespace ApiLibs
{
    public abstract class RequestContent
    {
        public abstract string ContentType { get; }
    }

    public class FileByteRequestContent : RequestContent
    {
        private readonly string _contentType;

        public FileByteRequestContent(string contentType, string name, byte[] bytes)
        {
            _contentType = contentType;
            Name = name;
            Bytes = bytes;
        }

        public override string ContentType => _contentType;

        public string Name { get; internal set; }
        public byte[] Bytes { get; internal set; }
    }

    public class FileStreamRequestContent : RequestContent
    {
        private readonly string _contentType;

        public FileStreamRequestContent(string name, Stream stream, string filename)
        {
            Name = name;
            Stream = stream;
            Filename = filename;
            _contentType = filename.Split('.').Last() switch
            {
                "gif" => "image/gif",
                "jpeg" => "image/jpeg",
                "jpg" => "image/jpeg",
                "png" => "image/png",
                "tiff" => "image/tiff",
                "webp" => "image/webp",
                "heic" => "image/heic",
                _ => throw new Exception("Unsupported file extension: " + filename.Split('.').Last())
            };
        }

        public override string ContentType => _contentType;
        public string Name { get; internal set; }
        public Stream Stream { get; internal set; }
        public string Filename { get; }
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
