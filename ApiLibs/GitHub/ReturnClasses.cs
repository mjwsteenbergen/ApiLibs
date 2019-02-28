using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.Telegram;

// ReSharper disable InconsistentNaming

namespace ApiLibs.GitHub
{
    public class Subject : ObjectSearcher
    {
        public string title { get; set; }
        public string url { get; set; }
        public string latest_comment_url { get; set; }
        public string type { get; set; }
    }


    public class GitHubUser
    {
        public string login { get; set; }
        public int id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        public string organizations_url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
        public string name { get; set; }
        public object company { get; set; }
        public string blog { get; set; }
        public object location { get; set; }
        public object email { get; set; }
        public object hireable { get; set; }
        public object bio { get; set; }
        public int public_repos { get; set; }
        public int public_gists { get; set; }
        public int followers { get; set; }
        public int following { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class NotificationsObject : ObjectSearcher
    {
        public string id { get; set; }
        public bool unread { get; set; }
        public string reason { get; set; }
        public string updated_at { get; set; }
        public string last_read_at { get; set; }
        public Subject subject { get; set; }
        public Repository repository { get; set; }
        private string _url;
        public string url { get { return _url.Replace("https://api.github.com/", ""); } set { _url = value; } }
        public string subscription_url { get; set; }

        //Objects

        public async Task<Issue> GetIssue()
        {
            Match match;
            if (subject.type == "Issue")
            {
               match = Regex.Match(subject.url, "https:\\/\\/api.github.com\\/repos\\/([^//]+)\\/([^//]+)\\/issues\\/(\\d+)");
            }
            else if (subject.type == "PullRequest")
            {
                match = Regex.Match(subject.url, "https:\\/\\/api.github.com\\/repos\\/([^//]+)\\/([^//]+)\\/pulls\\/(\\d+)");
            }
            else
            {
                throw new ArgumentException("This should be an issue or pull request");
            }
            return await (service as GitHubService).IssueService.GetIssue(match.Groups[1].Value, match.Groups[2].Value, int.Parse(match.Groups[3].Value));

        }

        public async Task<Release> GetRelease()
        {
            if (subject.type != "Release")
            {
                throw new ArgumentException("This should be an release");
            }
            Match match = Regex.Match(subject.url, "https:\\/\\/api.github.com\\/repos\\/([^//]+)\\/([^//]+)\\/releases\\/(\\d+)");
            return await (service as GitHubService).RepositoryService.GetRelease(match.Groups[1].Value, match.Groups[2].Value, int.Parse(match.Groups[3].Value));
        }

        public async Task<List<Event>> GetEvents()
        {
            Match match;
            if (subject.type == "Issue")
            {
                match = Regex.Match(subject.url, "https:\\/\\/api.github.com\\/repos\\/([^//]+)\\/([^//]+)\\/issues\\/(\\d+)");
            }
            else if (subject.type == "PullRequest")
            {
                match = Regex.Match(subject.url, "https:\\/\\/api.github.com\\/repos\\/([^//]+)\\/([^//]+)\\/pulls\\/(\\d+)");
            }
            else if (subject.type == "Release")
            {
                match = Regex.Match(subject.url, "https:\\/\\/api.github.com\\/repos\\/([^//]+)\\/([^//]+)\\/releases\\/(\\d+)");
            }
            else
            {
                throw new ArgumentException("Unrecognized subject type");
            }
            return await (service as GitHubService).ActivityService.GetEvents(match.Groups[1].Value, match.Groups[2].Value, int.Parse(match.Groups[3].Value));

        }

        //Created by me
        public DateTime updated_at_dt => DateTime.Parse(updated_at);

        public override string ToString()
        {
            return subject.type + ": " + repository.FullName + " " + subject.title + " because you " + reason;
        }
    }

    public class OpenIssue
    {
        public OpenIssue()
        {
            labels = new List<string>();
        }

        public string title { get; set; }
        public string body { get; set; }
        public string assignee { get; set; }
        public long? milestone { get; set; }
        public List<string> labels { get; set; }

    }

    public class ModifyIssue : OpenIssue
    {
        public string state { get; set; }
    }


    public class ReleaseRootobject
    {
        public Release[] ReleaseList { get; set; }
    }

    public class Release
    {
        public string url { get; set; }
        public string assets_url { get; set; }
        public string upload_url { get; set; }
        public string html_url { get; set; }
        public int id { get; set; }
        public string tag_name { get; set; }
        public string target_commitish { get; set; }
        public string name { get; set; }
        public bool draft { get; set; }
        public User author { get; set; }
        public bool prerelease { get; set; }
        public DateTime created_at { get; set; }
        public DateTime published_at { get; set; }
        public Asset[] assets { get; set; }
        public string tarball_url { get; set; }
        public string zipball_url { get; set; }
        public string body { get; set; }
    }

    public class Asset
    {
        public string url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public object label { get; set; }
        public User uploader { get; set; }
        public string content_type { get; set; }
        public string state { get; set; }
        public int size { get; set; }
        public int download_count { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string browser_download_url { get; set; }
    }

    public class Event
    {
        public int id { get; set; }
        public string url { get; set; }
        public User actor { get; set; }
        public string @event { get; set; }
        public object commit_id { get; set; }
        public object commit_url { get; set; }
        public DateTime created_at { get; set; }
        public Milestone milestone { get; set; }
        public User assignee { get; set; }
        public User assigner { get; set; }
        public Label label { get; set; }
    }
}


