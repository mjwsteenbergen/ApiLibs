using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        private readonly string basePath = "https://outlook.office.com/api/beta/";


        public OutlookService()
        {
            SetUp("https://login.microsoftonline.com/common/oauth2/v2.0/");
        }

        public OutlookService(string refreshToken, string outlookClientId, string outlookClientSecret, string emailAdress)
        {
            _refreshToken = refreshToken;
            _outlookClientId = outlookClientId;
            _outlookClientSecret = outlookClientSecret;
            SetUp(basePath);

            AddStandardHeader(new Param("Accept", "application/json"));
            AddStandardHeader(new Param("X-AnchorMailbox", emailAdress));
            AddStandardHeader(new Param("Authorization", "None"));

        }

        public async Task Connect(string outlookId, string redirectUrl, IOAuth authe)
        {
            var uri =
                new Uri("https://login.microsoftonline.com/common/oauth2/v2.0/authorize?client_id=" + outlookId +
                        "&redirect_uri=" + redirectUrl +
                        "&response_type=code&scope=https%3A%2F%2Foutlook.office.com%2FMail.ReadWrite+https%3A%2F%2Foutlook.office.com%2FCalendars.ReadWrite+" +
                        "offline_access");
            string return_url = authe.ActivateOAuth(uri);
            

            string login_code = return_url.Replace("code=","");

            SetBaseUrl("https://outlook.office.com/api/v1.0/");
        }

        public async Task<string> ConvertToToken(string outlookClientId, string outlookClientSecret, string loginCode)
        {
            List<Param> parameters = new List<Param>();
            parameters.Add(new Param("client_id", outlookClientId));
            parameters.Add(new Param("client_secret", outlookClientSecret));
            parameters.Add(new Param("code", loginCode));
            parameters.Add(new Param("redirect_uri", "https://www.microsoft.com"));
            parameters.Add(new Param("grant_type", "authorization_code"));

            string token = (await MakeRequest<AccessTokenObject>("token", Call.POST, parameters)).access_token;

            AddStandardHeader(new Param("Authorization", "Bearer " + token));
            return token;
        }

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

            SetBaseUrl(basePath);

            return accessToken;
        }

        internal override async Task<IRestResponse> HandleRequest(string url, Call call = Call.GET, List<Param> parameters = null, List<Param> headers = null, object content = null)
        {
            try
            {
                return await base.HandleRequest(url, call, parameters, headers);
            }
            catch (UnAuthorizedException e)
            {
                await RefreshToken();
                return await base.HandleRequest(url, call, parameters, headers);
            }

        }

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

        public async Task<Message> SetFlagged(Message m, FlagStatus status)
        {
            return await MakeRequest<Message>("me/messages/" + m.Id, Call.PATCH, content: new Flagger { Flag = new Flag { FlagStatus = status.ToString() }});
        }

        public async Task<Message> SetRead(Message m, bool read)
        {
            return await SetRead(m.Id, read);
        }

        private class ReadChange
        {
            public MessageChange change;
        }

        public async Task<Message> SetRead(string id, bool read)
        {
            return await MakeRequest<Message>("me/messages/" + id, Call.PATCH, content: new MessageChange { IsRead = read, Categories = new string[0] });
        }

        public async Task<List<Folder>> GetFolders(OData oData)
        {
            var returns =  (await MakeRequest<FolderRoot>("me/MailFolders" + oData.ConvertToUrl())).value.ToList();
            return returns;
        }

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
