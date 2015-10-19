using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.Outlook
{
    public class OutlookService : Service
    {
        public OutlookService()
        {
            SetUp("https://login.microsoftonline.com/common/oauth2/v2.0/");
        }

        public async Task Connect(IOAuth auth)
        {
            string return_url = await auth.ActivateOAuth(new Uri("https://login.microsoftonline.com/common/oauth2/v2.0/authorize?client_id=" + Passwords.OutlookID + "&redirect_uri=" + Passwords.GeneralRedirectUrl + "&response_type=code&scope=https%3A%2F%2Foutlook.office.com%2FMail.ReadWrite+https%3A%2F%2Foutlook.office.com%2FCalendars.ReadWrite"));

            string login_code = return_url.Replace("code=","");




            List<Param> parameters = new List<Param>();
            parameters.Add(new Param("client_id", Passwords.OutlookID));
            parameters.Add(new Param("client_secret", Passwords.OutlookKey));
            parameters.Add(new Param("code", login_code));
            parameters.Add(new Param("redirect_uri", "https://www.microsoft.com"));
            parameters.Add(new Param("grant_type", "authorization_code"));
            parameters.Add(new Param("resource", "https://outlook.office.com"));


            IRestResponse resp = await MakeRequestPost("token", parameters);

            Passwords.OutlookToken = JsonConvert.DeserializeObject<ReturnClasses.AccessTokenObject>(resp.Content).access_token;

            AddStandardHeader(new Param("Authorization", "Bearer " + Passwords.OutlookToken));
            //AddStandardHeader(new Param("X-AnchorMailbox", Passwords.OutlookEmailAdres));

            setBaseUrl("https://outlook.office.com/api/v1.0/");
        }

        public async Task<object> GetImportantEmail()
        {
            List<Param> parameters = new List<Param>();
            parameters.Add(new Param("Accept", "application/json"));
            parameters.Add(new Param("Authorization", Passwords.OutlookToken));
        https://outlook.office.com/api/v1.0/
            //"me/messages?$search=\"Category: Belangrijk\""
            IRestResponse resp = await MakeRequest("me/messages?$select=Subject,From,DateTimeReceived&$top=25", parameters);

            Console.WriteLine("Completed");

            return null;
        }
    }
}
