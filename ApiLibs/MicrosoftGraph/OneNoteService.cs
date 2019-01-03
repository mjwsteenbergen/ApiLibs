using ApiLibs.General;
using System;
using System.Collections.Generic;
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

        public Task<Section> CreateSection(Notebook notebook, Section section) => MakeRequest<Section>("me/onenote/notebooks/{id}/sections", Call.POST, content: section);

        public Task<GraphResult<Page>> GetPage(Section section) => MakeRequest<GraphResult<Page>>($"me/onenote/sections/{section.Id}/pages");

        public Task<Page> CreatePage(Section section, HtmlContent page)
        {
            return MakeRequest<Page>($"/me/onenote/sections/{section.Id}/pages", Call.POST, content: page);
        }

        public Task<string> GetPageContent(Page p) => MakeRequest<string>($"me/onenote/pages/{p.Id}/content");
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
