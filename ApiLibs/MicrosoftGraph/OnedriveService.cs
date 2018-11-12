using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using RestSharp;

namespace ApiLibs.MicrosoftGraph
{
    public class OneDriveService : SubService
    {
        public OneDriveService(GraphService service) : base(service) { }

        internal override Task<T> MakeRequest<T>(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return base.MakeRequest<T>("v1.0/" + url, m, parameters, header, content, statusCode);
        }

        internal override Task<IRestResponse> HandleRequest(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return base.HandleRequest("v1.0/" + url, m, parameters, header, content, statusCode);
        }

        public async Task<List<DriveItem>> GetFolderChildren(string name)
        {
            return (await MakeRequest<FolderChildrenRoot>($"me/drive/root:/{name}:/children")).Value;    
        }

        public async Task<DriveItem> GetFolder(string name)
        {
            return await MakeRequest<DriveItem>($"me/drive/root:/{name}:/");
        }

        public async Task<string> GetContents(string path)
        {
            return await MakeRequest<string>($"me/drive/root:/{path}:/content");
        }

        public async Task<string> GetContents(DriveItem driveItem)
        {
            return await MakeRequest<string>($"/me/drive/items/{driveItem.Id}/content");
        }
    }
}
