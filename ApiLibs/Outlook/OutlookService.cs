using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Outlook
{
    public class OutlookService : Service
    {
        private string _refreshToken;
        private string _outlookClientId;
        private string _outlookClientSecret;

        public event RefreshChangedEventHandler Changed;
        public delegate void RefreshChangedEventHandler(OutlookService sender, RefreshArgs e);

        private static readonly string basePath = "https://outlook.office.com/api/beta/";

        /// <summary>
        /// Create the outlook service if you need to authenticate
        /// </summary>
        public OutlookService() : base("https://login.microsoftonline.com/common/oauth2/v2.0/") { }

        /// <summary>
        /// Create a new outlook service if all tokens are available
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="outlookClientId"></param>
        /// <param name="outlookClientSecret"></param>
        /// <param name="emailAdress"></param>
        public OutlookService(string refreshToken, string outlookClientId, string outlookClientSecret, string emailAdress): base(basePath)
        {
            _refreshToken = refreshToken;
            _outlookClientId = outlookClientId;
            _outlookClientSecret = outlookClientSecret;

            AddStandardHeader(new Param("Accept", "application/json"));
            AddStandardHeader(new Param("X-AnchorMailbox", emailAdress));
            AddStandardHeader(new Param("Authorization", "None"));
        }

        /// <summary>
        /// Execute a call to the website to get an access token
        /// </summary>
        /// <param name="outlookId"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        public Task Connect(string outlookId, string redirectUrl, IOAuth auth)
        {
            var uri =
                new Uri("https://login.microsoftonline.com/common/oauth2/v2.0/authorize?client_id=" + outlookId +
                        "&redirect_uri=" + redirectUrl +
                        "&response_type=code&scope=https%3A%2F%2Foutlook.office.com%2FMail.ReadWrite+https%3A%2F%2Foutlook.office.com%2FCalendars.ReadWrite+" +
                        "offline_access");
            auth.ActivateOAuth(uri);
            return new Task(() => {});
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
                new Param("client_id", _outlookClientId),
                new Param("client_secret", _outlookClientSecret),
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


        internal override async Task<IRestResponse> HandleRequest(string url, Call call = Call.GET, List<Param> parameters = null, List<Param> headers = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            try
            {
                return await base.HandleRequest(url, call, parameters, headers, content);
            }
            catch (UnAuthorizedException)
            {
                await RefreshToken();
                return await base.HandleRequest(url, call, parameters, headers, content);
            }

        }

        /// <summary>
        /// Gets flagged data
        /// </summary>
        /// <param name="data"><see cref="OData"/> arguments</param>
        /// <returns>A list of <see cref="Message"/> objects</returns>
        public async Task<List<Message>> GetFlaggedEmail(OData data)
        {
            data.Filter = "Flag/FlagStatus eq 'Flagged'";
            MessageRoot root = (await MakeRequest<MessageRoot>("me/messages" + data.ConvertToUrl()));
            root.value.ToList().ForEach(message => message.service = root.service);
            return root.value.ToList();
        }

        private class Flagger
        {
            public Flag Flag;
        }

        /// <summary>
        /// Sets the flagstatus of a message
        /// </summary>
        /// <param name="m">A <see cref="Message"/> object</param>
        /// <param name="status">Status to set it to</param>
        /// <returns></returns>
        public async Task<Message> SetFlagged(Message m, FlagStatus status)
        {
            return await MakeRequest<Message>("me/messages/" + m.Id, Call.PATCH, content: new Flagger { Flag = new Flag { FlagStatus = status.ToString() }});
        }

        /// <summary>
        /// Mark a <see cref="Message"/> as read or unread
        /// </summary>
        /// <param name="m">A <see cref="Message"/> object</param>
        /// <param name="read">If the item should be marked as read (true) or unread (false)</param>
        /// <returns></returns>
        public async Task<Message> SetRead(Message m, bool read)
        {
            return await SetRead(m.Id, read);
        }

        /// <summary>
        /// Mark a <see cref="Message"/> as read or unread
        /// </summary>
        /// <param name="id">Message id</param>
        /// <param name="read">If the item should be marked as read (true) or unread (false)</param>
        /// <returns></returns>
        public async Task<Message> SetRead(string id, bool read)
        {
            return await MakeRequest<Message>("me/messages/" + id, Call.PATCH, content: new MessageChange { IsRead = read, Categories = new string[0] });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oData"><see cref="OData"/> arguments</param>
        /// <returns></returns>
        public async Task<List<Folder>> GetFolders(OData oData)
        {
            var returns =  (await MakeRequest<FolderRoot>("me/MailFolders" + oData.ConvertToUrl())).value.ToList();
            return returns;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName">Name of the folder</param>
        /// <param name="oData"><see cref="OData"/> arguments</param>
        /// <returns></returns>
        public async Task<Folder> GetFolder(string folderName, OData oData)
        {
            return (await GetFolders(oData)).Find(folder => folder.DisplayName == folderName);
        }

        /// <summary>
        /// Gets messages from a <see cref="Folder"/>
        /// </summary>
        /// <param name="folder">A <see cref="Folder"/> object</param>
        /// <param name="data"><see cref="OData"/> arguments</param>
        /// <returns></returns>
        public async Task<List<Message>> GetMessages(Folder folder, OData data)
        {
            return (await MakeRequest<MessageRoot>("me/MailFolders/" + folder.Id + "/messages" + data.ConvertToUrl())).value.ToList();
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
