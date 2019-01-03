using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace ApiLibs.Wunderlist
{

    public class Auth
    {
        public string access_token { get; set; }
    }


    public class Lists
    {
        public WList[] Property1 { get; set; }
    }

    public class WList
    {
        public long id { get; set; }
        public string title { get; set; }
        public string owner_type { get; set; }
        public int owner_id { get; set; }
        public string list_type { get; set; }
        public bool _public { get; set; }
        public int revision { get; set; }
        public DateTime created_at { get; set; }
        public string type { get; set; }
        public string created_by_request_id { get; set; }
    }

    public class WTask
    {
        public long id { get; set; }
        public DateTime created_at { get; set; }
        public int created_by_id { get; set; }
        public string created_by_request_id { get; set; }
        public bool completed { get; set; }
        public bool starred { get; set; }
        public long list_id { get; set; }
        public int revision { get; set; }
        public string title { get; set; }
        public string type { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as WTask)?.id == id;
        }

        public override int GetHashCode()
        {
            var hashCode = 1399796180;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + created_at.GetHashCode();
            hashCode = hashCode * -1521134295 + created_by_id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(created_by_request_id);
            hashCode = hashCode * -1521134295 + completed.GetHashCode();
            hashCode = hashCode * -1521134295 + starred.GetHashCode();
            hashCode = hashCode * -1521134295 + list_id.GetHashCode();
            hashCode = hashCode * -1521134295 + revision.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(type);
            return hashCode;
        }

        public WPatchTask ToPatchTask()
        {
            WPatchTask patch = new WPatchTask
            {
                id = id,
                completed = completed,
                list_id = list_id,
                title = title,
                revision = revision
            };
            return patch;
        }
    }

    public class WPatchTask
    {
        public long? id;
        public int? revision;
        public long? list_id;
        public string title;
        public int? assignee;
        public bool? completed;
    }


    public class WRequestTask
    {
        public long list_id { get; set; }
        public string title { get; set; }
        public int? assignee_id { get; set; }
        public bool completed { get; set; }
        public string due_date { get; set; }
        public bool starred { get; set; }
    }


}
