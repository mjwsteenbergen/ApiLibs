﻿using ApiLibs.General;
using HttpMultipartParser;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiLibs.MicrosoftGraph
{
    public class OneNoteService : GraphSubService
    {
        public OneNoteService(GraphService service) : base(service, "v1.0") { }

        public Task<GraphResult<Notebook>> GetMyNotebooks()
        {
            return MakeRequest<GraphResult<Notebook>>("me/onenote/notebooks");
        }

        public Task<Notebook> GetNotebook(string id)
        {
            return MakeRequest<Notebook>($"me/onenote/notebooks/{id}");
        }

        public Task<GraphResult<RecentNotebook>> GetMyRecentNotebooks(bool includePersonal) {
            return MakeRequest<GraphResult<RecentNotebook>>($"me/onenote/notebooks/getRecentNotebooks(includePersonalNotebooks={includePersonal})");
        }

        public async Task<Section> GetSection(Notebook book, string name)
        {
            return (await GetSections(book)).Value.Find(i => i.DisplayName == name);
        }

        public async Task<Notebook> GetMyNotebook(string name)
        {
            return (await GetMyNotebooks()).Value.Find(i => i.DisplayName == name);
        }

        public Task<GraphResult<Section>> GetSections(Notebook notebook)
        {
            return MakeRequest<GraphResult<Section>>($"me/onenote/notebooks/{notebook.Id}/sections");
        }

        public Task<GraphResult<Section>> GetSections() => MakeRequest<GraphResult<Section>>("me/onenote/sections");

        public Task<Section> CreateSection(Notebook notebook, Section section) => MakeRequest<Section>($"me/onenote/notebooks/{notebook.Id}/sections", Call.POST, content: section);

        public Task<GraphResult<Page>> GetPage(Section section) => MakeRequest<GraphResult<Page>>($"me/onenote/sections/{section.Id}/pages");

        public Task<Page> CreatePage(Section section, HtmlContent page)
        {
            return MakeRequest<Page>($"/me/onenote/sections/{section.Id}/pages", Call.POST, content: page, statusCode: System.Net.HttpStatusCode.Created);
        }

        public Task<string> GetPageContent(Page p) => MakeRequest<string>($"me/onenote/pages/{p.Id}/content");

        public Task<InkReturnType> GetPageInk(Page p) => GetPageInk(p.Id);

        public async Task<InkReturnType> GetPageInk(string pageId) 
        {
            var res = await MakeRequest<string>($"me/onenote/pages/{pageId}/content", 
            parameters: new List<Param> {
                new Param("includeInkML","true")
            }, 
            headers: new List<Param>
            {
                new Param("Accept", "*/*")
            });

            var parser = MultipartFormDataParser.Parse(GenerateStreamFromString(res));
            return new InkReturnType {
                HTML = new StreamReader(parser.Files.First(i => i.ContentType == "text/html").Data, Encoding.UTF8).ReadToEnd(),
                Ink = new StreamReader(parser.Files.First(i => i.ContentType == "application/inkml+xml").Data, Encoding.UTF8).ReadToEnd(),
            };
        }

        public class InkReturnType {
            public string HTML { get; set; }
            public string Ink { get; set; }

            public static string DefaultInk => "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<inkml:ink xmlns:emma=\"http://www.w3.org/2003/04/emma\" xmlns:msink=\"http://schemas.microsoft.com/ink/2010/main\" xmlns:inkml=\"http://www.w3.org/2003/InkML\">\r\n  <inkml:definitions />\r\n  <inkml:traceGroup />\r\n</inkml:ink>";
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public Task DeletePage(Page t2) => DeletePage(t2.Id);

        public Task DeletePage(string id) => MakeRequest<string>("/me/onenote/pages/" + id, Call.DELETE, statusCode: System.Net.HttpStatusCode.NoContent);

        public Task UpdatePageTitle(Page page, string name) => UpdatePage(page, new PatchContentCommand {
            Action = "replace",
            Content = name,
            Target = "title"
        });

        public Task UpdatePage(Page page, PatchContentCommand command) => UpdatePage(page.Id, command);

        public Task UpdatePage(string id, PatchContentCommand command) => UpdatePage(id, new List<PatchContentCommand> { command });

        private Task UpdatePage(string id, List<PatchContentCommand> commands) => MakeRequest<string>($"/me/onenote/pages/{id}/content", Call.PATCH, content: commands, statusCode: System.Net.HttpStatusCode.NoContent);
    }

    public class PatchContentCommand
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }
    }

    public class OneNoteHtmlContent : HtmlContent
    {
        public OneNoteHtmlContent(string content) : base(content) { }

        public static HtmlContent BuildWebpage(string title, string body, HtmlStyleCreator htmlStyleCreator)
        {
            return new OneNoteHtmlContent(htmlStyleCreator.Apply(GetHtml(title, body)));
        }
    }

    public class HtmlStyleCreator
    {
        public HtmlStyleCreator()
        {
            DefaultStyle = new HtmlStyle();

            BodyStyle = new HtmlStyle(DefaultStyle);

            Header = new HtmlStyle(DefaultStyle);
            Header1 = new HtmlStyle(Header);
            Header2 = new HtmlStyle(Header);
            Header3 = new HtmlStyle(Header);

            ListStyle = new HtmlStyle(DefaultStyle);

            SelectedStyle = DefaultStyle;
        }

        public HtmlStyle SelectedStyle { get; private set; }
        public HtmlStyle DefaultStyle { get; private set; }
        public HtmlStyle BodyStyle { get; private set; }
        public HtmlStyle Header { get; private set; }
        public HtmlStyle Header1 { get; private set; }
        public HtmlStyle Header2 { get; private set; }
        public HtmlStyle Header3 { get; private set; }
        public HtmlStyle ListStyle { get; private set; }

        public static HtmlStyleCreator CreateStyle()
        {
            return new HtmlStyleCreator();
        }

        public HtmlStyleCreator Color(string Color)
        {
            SelectedStyle.Color = Color;
            return this;
        }

        public HtmlStyleCreator FontFamily(string fontFamily)
        {
            SelectedStyle.FontFamily = fontFamily;
            return this;
        }

        public HtmlStyleCreator FontSize(int fontSize)
        {
            SelectedStyle.FontSize = fontSize;
            return this;
        }

        public HtmlStyleCreator Headers()
        {
            SelectedStyle = Header;
            return this;
        }

        public HtmlStyleCreator H1()
        {
            SelectedStyle = Header1;
            return this;
        }

        public HtmlStyleCreator H2()
        {
            SelectedStyle = Header2;
            return this;
        }

        public HtmlStyleCreator H3()
        {
            SelectedStyle = Header3;
            return this;
        }

        internal string Apply(string text)
        {
            text = Regex.Replace(text, "<body>", $"<body {BodyStyle.Apply()}>");
            text = Regex.Replace(text, "<h1>", $"<h1 {Header1.Apply()}>");
            text = Regex.Replace(text, "<h2>", $"<h2 {Header2.Apply()}>");
            text = Regex.Replace(text, "<h3>", $"<h3 {Header3.Apply()}>");
            text = Regex.Replace(text, "<li>", $"<li {ListStyle.Apply()}>");
            return text;
        }
    }

    public class HtmlStyle
    {
        private HtmlStyle superStyle;

        public HtmlStyle()
        {
        }

        public HtmlStyle(HtmlStyle superStyle)
        {
            this.superStyle = superStyle;
        }

        private string fontFam;

        public string FontFamily
        {
            get { return fontFam ?? superStyle?.FontFamily; }
            set { fontFam = value; }
        }

        private string color;

        public string Color
        {
            get { return color ?? superStyle?.Color; }
            set { color = value; }
        }

        private int fsize;

        public int FontSize
        {
            get { return fsize == 0 ? superStyle?.FontSize ?? 0 : fsize; }
            set { fsize = value; }
        }


        internal object Apply()
        {
            string result = "style=\"";

            if(!string.IsNullOrEmpty(FontFamily))
            {
                result += $"font-family:{FontFamily};";
            } 

            if (!string.IsNullOrEmpty(Color))
            {
                result += $"color:{Color};";
            }

            if (FontSize != 0)
            {
                result += $"font-size:{FontSize}pt;";
            }

            result = result.Substring(0, result.Length - 1);

            return result += "\"";

        }

        internal bool HasAnyStyles()
        {
            return !string.IsNullOrEmpty(FontFamily) || !string.IsNullOrEmpty(Color) || FontSize != 0 || superStyle.HasAnyStyles();
        }
    }
}
