using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.MicrosoftGraph
{
    public class OneDriveService : GraphSubService
    {
        public OneDriveService(GraphService service) : base(service, "v1.0") { }

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

        public Task SetContent(string filepath, string text)
        {
            return MakeRequest(new Request<DriveItem>($"/me/drive/root:/{filepath}:/content")
            {
                Method = Call.PUT,
                Content = new PlainTextContent(text),
                ExpectedStatusCode = new HttpStatusCode[] { HttpStatusCode.NoContent, HttpStatusCode.OK },
            });
        }
    }
}
