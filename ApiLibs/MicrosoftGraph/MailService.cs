using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLibs.MicrosoftGraph
{
    public class MailService : GraphSubService
    {
        public MailService(GraphService service) : base(service, "v1.0") { }

        /// <summary>
        /// Gets flagged data
        /// </summary>
        /// <param name="data"><see cref="OData"/> arguments</param>
        /// <returns>A list of <see cref="EmailMessage"/> objects</returns>
        public async Task<List<EmailMessage>> GetFlaggedEmail(OData data)
        {
            data.Filter = "Flag/FlagStatus eq 'Flagged'";
            return (await MakeRequest<MessageRoot>("me/messages", parameters: data.ConvertToParams())).Value;
        }

        private class Flagger
        {
            public Flag Flag;
        }

        /// <summary>
        /// Sets the flagstatus of a message
        /// </summary>
        /// <param name="m">A <see cref="EmailMessage"/> object</param>
        /// <param name="status">Status to set it to</param>
        /// <returns></returns>
        public async Task<EmailMessage> SetFlagged(EmailMessage m, FlagStatus status)
        {
            return await MakeRequest<EmailMessage>("me/messages/" + m.Id, Call.PATCH, content: new Flagger { Flag = new Flag { FlagStatus = status.ToString() } });
        }

        /// <summary>
        /// Mark a <see cref="EmailMessage"/> as read or unread
        /// </summary>
        /// <param name="m">A <see cref="EmailMessage"/> object</param>
        /// <param name="read">If the item should be marked as read (true) or unread (false)</param>
        /// <returns></returns>
        public async Task<EmailMessage> SetRead(EmailMessage m, bool read)
        {
            return await SetRead(m.Id, read);
        }

        /// <summary>
        /// Mark a <see cref="EmailMessage"/> as read or unread
        /// </summary>
        /// <param name="id">Message id</param>
        /// <param name="read">If the item should be marked as read (true) or unread (false)</param>
        /// <returns></returns>
        public async Task<EmailMessage> SetRead(string id, bool read)
        {
            return await MakeRequest<EmailMessage>("me/messages/" + id, Call.PATCH, content: new MessageChange { IsRead = read, Categories = new string[0] });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oData"><see cref="OData"/> arguments</param>
        /// <returns></returns>
        public async Task<List<EmailFolder>> GetFolders(OData oData)
        {
            var returns = (await MakeRequest<FolderRoot>("me/MailFolders", parameters: oData.ConvertToParams())).Value;
            return returns;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName">Name of the folder</param>
        /// <param name="oData"><see cref="OData"/> arguments</param>
        /// <returns></returns>
        public async Task<EmailFolder> GetFolder(string folderName, OData oData)
        {
            return (await GetFolders(oData)).Find(folder => folder.DisplayName == folderName);
        }

        /// <summary>
        /// Gets messages from a <see cref="DriveItem"/>
        /// </summary>
        /// <param name="folder">A <see cref="DriveItem"/> object</param>
        /// <param name="data"><see cref="OData"/> arguments</param>
        /// <returns></returns>
        public async Task<List<EmailMessage>> GetMessages(EmailFolder folder, OData data)
        {
            return (await MakeRequest<MessageRoot>("me/MailFolders/" + folder.Id + "/messages", parameters: data.ConvertToParams())).Value;
        }


        /// <summary>
        /// Move a message to a folder. This creates a new copy of the message in the destination folder and removes the original message.
        /// </summary>
        /// <param name="message">A <see cref="EmailMessage"/> object</param>
        /// <param name="folder">A <see cref="DriveItem"/> object</param>
        /// <returns></returns>
        public async Task Move(EmailMessage message, EmailFolder folder)
        {
            await Move(message.Id, folder.Id);
        }

        /// <summary>
        /// Move a message to a folder. This creates a new copy of the message in the destination folder and removes the original message.
        /// </summary>
        /// <param name="messageId">Id of the message</param>
        /// <param name="folderId">Id of the folder</param>
        /// <returns></returns>
        private async Task Move(string messageId, string folderId)
        {
            await MakeRequest<string>($"/me/messages/{messageId}/move", Call.POST, content: new
            {
                destinationId = folderId
            });
        }

        public async Task Archive(EmailMessage message)
        {
            EmailFolder archiveFolder = await GetFolder("Archive", new OData().GetMaxItems());
            await Move(message, archiveFolder);
        }
    }
}
