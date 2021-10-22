using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using ApiLibs.General;

namespace ApiLibs.MicrosoftGraph
{
    public class GraphService : RestSharpService
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
        public CloudCommunicationsService CloudCommunicationsService { get; }
        public ChangeNotificationService ChangeNotificationService { get; }

        private static readonly string basePath = "https://graph.microsoft.com/";

        /// <summary>
        /// Create the outlook service if you need to authenticate
        /// </summary>
        public GraphService() : base("https://login.microsoftonline.com/common/oauth2/v2.0/")
        {
            CalendarService = new CalendarService(this);
            PeopleService = new PeopleService(this);
            OneDriveService = new OneDriveService(this);
            MailService = new MailService(this);
            TodoService = new TodoService(this);
            OneNoteService = new OneNoteService(this);
            CloudCommunicationsService = new CloudCommunicationsService(this);
            ChangeNotificationService = new ChangeNotificationService(this);
            //Don't forget the other constructor

            SetupBase();
        }

        /// <summary>
        /// Create a new outlook service if all tokens are available
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="emailAdress"></param>
        public GraphService(string refreshToken, string clientId, string clientSecret, string emailAdress) : base(basePath)
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
            CloudCommunicationsService = new CloudCommunicationsService(this);
            ChangeNotificationService = new ChangeNotificationService(this);
            //Don't forget the other constructor

            SetupBase();
        }

        private void SetupBase()
        {
            RequestResponseMiddleware.Add(async (resp) =>
            {
                if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var request = resp.Request;
                    await RefreshToken();
                    request.Retries++;
                    return await base.HandleRequest(request);
                }

                return resp;
            });
        }

        public async Task ConvertToToken(string clientId, string clientSecret, string username, string password, string TenantID)
        {
            SetBaseUrl($"https://login.microsoftonline.com/{TenantID}/oauth2/v2.0/");
            var res = await MakeRequest<AccessTokenObject>($"token", Call.POST, parameters: new List<Param> {
                new Param("grant_type", "password"),
                new Param("client_id", clientId),
                new Param("client_secret", clientSecret),
                new Param("scope", "https://graph.microsoft.com/.default"),
                new Param("userName", username),
                new Param("password", password)
            });


            AddStandardHeader(new Param("Authorization", "Bearer " + res.access_token));
            SetBaseUrl(basePath);
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
            Connect(outlookId, redirectUrl, auth, scopes.Select(i => i.ToString().Replace("_", ".")).ToList());
        }

        /// <summary>
        /// Execute a call to the website to get an access token
        /// </summary>
        /// <param name="outlookId"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        public void Connect(string outlookId, string redirectUrl, IOAuth auth, List<string> scopes)
        {
            var uri =
                new Uri("https://login.microsoftonline.com/common/oauth2/v2.0/authorize?" +
                        "client_id=" + outlookId +
                        "&redirect_uri=" + HttpUtility.UrlEncode(redirectUrl) +
                        "&response_type=code" +
                        "&response_mode=query" +
                        "&scope=" + scopes.Aggregate((i, j) => i + "+" + j) +
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
            Changed?.Invoke(this, new RefreshArgs { RefreshToken = _refreshToken });
            SetBaseUrl(basePath);

            return accessToken;
        }

        public class RefreshArgs : EventArgs
        {
            public string RefreshToken { get; set; }
        }

        public void PreferTimeZone(TimeZoneInfo timezone)
        {
            AddStandardHeader("Prefer", $"outlook.timezone=\"{timezone.StandardName}\"");
        }
    }

    public abstract class GraphSubService : SubService<GraphService>
    {
        public GraphSubService(GraphService service, string version) : base(service)
        {
            Version = version;

            RequestMiddleware.Add((req) =>
            {
                req.EndPoint = $"{Version}/" + req.EndPoint;
                return Task.FromResult(req);
            });
        }

        public string Version { get; }
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
        public static Param TopToParam(int number) => new Param("$top", number);
        public static Param FilterToParam(string filter) => new Param("$filter", filter);
        public static Param SelectToParam(string select) => new Param("$select", select);

        public string Filter;
        public string Select;
        public int Top = -1;

        public List<Param> ConvertToParams()
        {
            return new List<(string param, string value)> {
                ("$filter", Filter),
                ("$select", Select),
                ("$top", Top == -1 ? null : Top.ToString())
            }
            .Where(i => i.value != null && i.value != "-1")
            .Select(i => new Param(i.param, i.value))
            .ToList();
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
