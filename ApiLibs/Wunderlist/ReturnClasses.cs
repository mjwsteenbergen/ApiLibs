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
        public int id { get; set; }
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
        public int id { get; set; }
        public DateTime created_at { get; set; }
        public int created_by_id { get; set; }
        public string created_by_request_id { get; set; }
        public bool completed { get; set; }
        public bool starred { get; set; }
        public int list_id { get; set; }
        public int revision { get; set; }
        public string title { get; set; }
        public string type { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as WTask)?.id == id;
        }

        public override int GetHashCode()
        {
            return id;
        }
    }


    public class WRequestTask
    {
        public int list_id { get; set; }
        public string title { get; set; }
        public int? assignee_id { get; set; }
        public bool completed { get; set; }
        public string due_date { get; set; }
        public bool starred { get; set; }
    }


}
