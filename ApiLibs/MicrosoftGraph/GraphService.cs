using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using ApiLibs.General;
using RestSharp;

namespace ApiLibs.MicrosoftGraph
{
    public class GraphService : Service
    {
        private string _refreshToken;
        private readonly string _clientId;
        private readonly string _clientSecret;

        public event RefreshChangedEventHandler Changed;
        public delegate void RefreshChangedEventHandler(GraphService sender, RefreshArgs e);

        public CalendarService CalendarService { get; }
        public PeopleService PeopleService { get; }
        public OneDriveService OneDriveService { get; }
        public MailService MailService { get; }
        public TodoService TodoService { get; }
        public OneNoteService OneNoteService { get; set; }

        private static readonly string basePath = "https://graph.microsoft.com/";

        /// <summary>
        /// Create the outlook service if you need to authenticate
        /// </summary>
        public GraphService() : base("https://login.microsoftonline.com/common/oauth2/v2.0/")
        {
        }

        /// <summary>
        /// Create a new outlook service if all tokens are available
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="emailAdress"></param>
        public GraphService(string refreshToken, string clientId, string clientSecret, string emailAdress): base(basePath)
        {
            _refreshToken = refreshToken;
            _clientId = clientId;
            _clientSecret = clientSecret;

            AddStandardHeader(new Param("Accept", "application/json"));
            AddStandardHeader(new Param("X-AnchorMailbox", emailAdress));
            AddStandardHeader(new Param("Authorization", "None"));
            CalendarService = new CalendarService(this);
            PeopleService = new PeopleService(this);
            OneDriveService = new OneDriveService(this);
            MailService = new MailService(this);
            TodoService = new TodoService(this);
            OneNoteService = new OneNoteService(this);
        }


        /// <summary>
        /// Execute a call to the website to get an access token
        /// </summary>
        /// <param name="outlookId"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        public void Connect(string outlookId, string redirectUrl, IOAuth auth, List<Scopes> scopes)
        {
            var uri =
                new Uri("https://login.microsoftonline.com/common/oauth2/v2.0/authorize?" + 
                        "client_id=" + outlookId +
                        "&redirect_uri=" + HttpUtility.UrlEncode(redirectUrl)  +
                        "&response_type=code" +
                        "&response_mode=query" +
                        "&scope=" + scopes.Select(i => i.ToString().Replace("_", ".")).Aggregate((i, j) => i + "+" + j) +
                        "+offline_access");
            auth.ActivateOAuth(uri);
        }

        public enum Scopes
        {
            Calendars_ReadWrite, Calendars_ReadWrite_Shared, Contacts_ReadWrite, Device_ReadWrite_All, Directory_ReadWrite_All, Files_ReadWrite_All, Mail_ReadWrite, Mail_Send, Notes_ReadWrite_All, People_Read, People_Read_All, User_ReadWrite_All,
            Device_Read, Tasks_Read, Tasks_ReadWrite, Notes_Read_All, Notes_Create, Notes_Read, Notes_ReadWrite
        }


        /// <summary>
        /// Convert the login code to a token
        /// </summary>
        /// <param name="outlookClientId"></param>
        /// <param name="outlookClientSecret"></param>
        /// <param name="loginCode">The login recieved from <seealso cref="Connect"/> method</param>
        /// <param name="redirect_url"></param>
        /// <returns></returns>
        public async Task<string> ConvertToToken(string outlookClientId, string outlookClientSecret, string loginCode, string redirect_url)
        {
            SetBaseUrl("https://login.microsoftonline.com/common/oauth2/v2.0/");

            List<Param> parameters = new List<Param>
            {
                new Param("client_id", outlookClientId),
                new Param("client_secret", outlookClientSecret),
                new Param("code", loginCode),
                new Param("redirect_uri", redirect_url),
                new Param("grant_type", "authorization_code")
            };

            string token = (await MakeRequest<AccessTokenObject>("token", Call.POST, parameters)).refresh_token;

            SetBaseUrl(basePath);
            return token;
        }


        /// <summary>
        /// Refresh the accestoken
        /// </summary>
        /// <returns>An object which has the new token</returns>
        public async Task<AccessTokenObject> RefreshToken()
        {
            SetBaseUrl("https://login.microsoftonline.com/common/oauth2/v2.0/");
            RemoveStandardHeader("Authorization");


            List<Param> parameters = new List<Param>
            {
                new Param("client_id", _clientId),
                new Param("client_secret", _clientSecret),
                new Param("refresh_token", _refreshToken),
                new Param("redirect_uri", "https://www.microsoft.com"),
                new Param("grant_type", "refresh_token")
            };

            AccessTokenObject accessToken = await MakeRequest<AccessTokenObject>("token", Call.POST, parameters);

            _refreshToken = accessToken.refresh_token;
            AddStandardHeader(new Param("Authorization", "Bearer " + accessToken.access_token));
            Changed?.Invoke(this, new RefreshArgs { RefreshToken = _refreshToken});
            SetBaseUrl(basePath);

            return accessToken;
        }

        public class RefreshArgs : EventArgs
        {
            public string RefreshToken { get; set; }
        }


        protected internal override async Task<string> HandleRequest(string url, Call call = Call.GET,
            List<Param> parameters = null, List<Param> headers = null, object content = null,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            try
            {
                return await base.HandleRequest(url, call, parameters, headers, content, statusCode);
            }
            catch (UnAuthorizedException)
            {
                await RefreshToken();
                return await base.HandleRequest(url, call, parameters, headers, content, statusCode);
            }

        }

        public void PreferTimeZone(TimeZoneInfo timezone)
        {
            AddStandardHeader("Prefer", $"outlook.timezone=\"{timezone.StandardName}\"");
        }
    }

    public abstract class GraphSubService : SubService
    {
        public GraphSubService(Service service, string version) : base(service)
        {
            Version = version;
        }

        public string Version { get; }

        protected override Task<string> HandleRequest(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return base.HandleRequest($"{Version}/" + url, m, parameters, header, content, statusCode);
        }

        protected override Task<T> MakeRequest<T>(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return base.MakeRequest<T>($"{Version}/" + url, m, parameters, header, content, statusCode);
        }
    }

    public enum FlagStatus
    {
        Complete, Flagged, NotFlagged
    }

    public class MessageChange
    {
        public string[] Categories;
        public bool IsRead;
    }

    public class OData
    {
        public string Filter;
        public string Select;
        public int Top = -1;


        public string ConvertToUrl()
        {
            string res = "";
            string convToken = "?";
            string switchToken = "&";

            if (Filter != null)
            {
                res += convToken + "$filter=" + Filter;
                convToken = switchToken;
            }
            if (Select != null)
            {
                res += convToken + "$select=" + Select;
                convToken = switchToken;
            }
            if (Top != -1)
            {
                res += convToken + "$top=" + Top;
                convToken = switchToken;
            }

            return res;
        }

        public OData AddUnreadSelector(bool isRead)
        {
            string s = "IsRead eq " + isRead.ToString().ToLower();
            Filter = Filter == null ? s : Filter + " " + s;
            return this;
        }

        public OData GetMaxItems()
        {
            Top = 50;
            return this;
        }
    }
}
