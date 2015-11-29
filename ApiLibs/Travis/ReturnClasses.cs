
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace ApiLibs.Travis
{
    public class Auth
    {
        public string access_token { get; set; }
    }


    public class BuildList
    {
        public Build[] builds { get; set; }
        public Commit[] commits { get; set; }
    }

    public class Build
    {
        public int id { get; set; }
        public int repository_id { get; set; }
        public int commit_id { get; set; }
        public string number { get; set; }
        public string event_type { get; set; }
        public bool pull_request { get; set; }
        public object pull_request_title { get; set; }
        public object pull_request_number { get; set; }
        public Config config { get; set; }
        public string state { get; set; }
        public DateTime started_at { get; set; }
        public string finished_at { get; set; }
        public int duration { get; set; }
        public int[] job_ids { get; set; }
    }

    public class Config
    {
        public bool sudo { get; set; }
        public string dist { get; set; }
        public string language { get; set; }
        public string[] compiler { get; set; }
        public Notifications notifications { get; set; }
        public string[] before_install { get; set; }
        public string[] install { get; set; }
        public string[] before_script { get; set; }
        public string[] script { get; set; }
        public string result { get; set; }
    }

    public class Notifications
    {
        public string on_success { get; set; }
        public string on_failure { get; set; }
    }

    public class Commit
    {
        public int id { get; set; }
        public string sha { get; set; }
        public string branch { get; set; }
        public string message { get; set; }
        public DateTime committed_at { get; set; }
        public string author_name { get; set; }
        public string author_email { get; set; }
        public string committer_name { get; set; }
        public string committer_email { get; set; }
        public string compare_url { get; set; }
        public object pull_request_number { get; set; }
    }


}
