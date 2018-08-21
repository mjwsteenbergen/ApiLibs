using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Outlook
{
    public class AccessTokenObject
    {
        public string expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }

    public class MessageRoot : ObjectSearcher
    {
        public string odatacontext { get; set; }
        public Message[] value { get; set; }
        public string odatanextLink { get; set; }
    }

    public class Message : ObjectSearcher
    {
        public string odataid { get; set; }
        public string odataetag { get; set; }
        public string Id { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public string ChangeKey { get; set; }
        public object[] Categories { get; set; }
        public DateTime? ReceivedDateTime { get; set; }
        public DateTime? SentDateTime { get; set; }
        public bool HasAttachments { get; set; }
        public string InternetMessageId { get; set; }
        public string Subject { get; set; }
        public Body Body { get; set; }
        public string BodyPreview { get; set; }
        public string Importance { get; set; }
        public string ParentFolderId { get; set; }
        public Sender Sender { get; set; }
        public From From { get; set; }
        public Torecipient[] ToRecipients { get; set; }
        public object[] CcRecipients { get; set; }
        public object[] BccRecipients { get; set; }
        public Replyto[] ReplyTo { get; set; }
        public string ConversationId { get; set; }
        public string ConversationIndex { get; set; }
        public object IsDeliveryReceiptRequested { get; set; }
        public bool IsReadReceiptRequested { get; set; }
        public bool IsRead { get; set; }
        public bool IsDraft { get; set; }
        public string WebLink { get; set; }
        public object MentionsPreview { get; set; }
        public string InferenceClassification { get; set; }
        public string[] UnsubscribeData { get; set; }
        public bool UnsubscribeEnabled { get; set; }
        public Flag Flag { get; set; }

        public async Task SetFlag(FlagStatus status)
        {
            await (service as GraphService).SetFlagged(this, status);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="read">True means that the maessage will be marked as read</param>
        /// <returns></returns>
        public async Task SetRead(bool read)
        {
            await (service as GraphService).SetRead(this, read);
        }
    }

    public class Body
    {
        public string ContentType { get; set; }
        public string Content { get; set; }
    }

    public class Sender
    {
        public Emailaddress EmailAddress { get; set; }
    }

    public class Emailaddress
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class From
    {
        public Emailaddress EmailAddress { get; set; }
    }

    public class Flag
    {
        public string FlagStatus { get; set; }
        public Duedatetime DueDateTime { get; set; }
        public Startdatetime StartDateTime { get; set; }
    }

    public class Duedatetime
    {
        public DateTime DateTime { get; set; }
        public string TimeZone { get; set; }
    }

    public class Startdatetime
    {
        public DateTime DateTime { get; set; }
        public string TimeZone { get; set; }
    }

    public class Torecipient
    {
        public Emailaddress EmailAddress { get; set; }
    }

    public class Replyto
    {
        public Emailaddress EmailAddress { get; set; }
    }


    public class FolderRoot : ObjectSearcher
    {
        public string odatacontext { get; set; }
        public Folder[] value { get; set; }
        public string odatanextLink { get; set; }

        public new void Search(Service inputService)
        {
            base.Search(inputService);

            foreach (Folder folder in value)
            {
                folder.service = inputService;
            }
        }


    }

    public class Folder : ObjectSearcher
    {
        public string odataid { get; set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string ParentFolderId { get; set; }
        public int ChildFolderCount { get; set; }
        public int UnreadItemCount { get; set; }
        public int TotalItemCount { get; set; }
        public object WellKnownName { get; set; }

        public async Task<List<Message>> GetMessages(OData odata)
        {
            return await (service as GraphService).GetMessages(this, odata);
        }
    }






}
