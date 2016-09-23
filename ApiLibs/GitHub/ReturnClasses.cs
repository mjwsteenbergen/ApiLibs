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


    public class Owner
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
    }

    public class Repository
    {
        public int id { get; set; }
        public string name { get; set; }
        public string full_name { get; set; }
        public Owner owner { get; set; }
        public bool @private { get; set; }
        public string html_url { get; set; }
        public string description { get; set; }
        public bool fork { get; set; }
        public string url { get; set; }
        public string forks_url { get; set; }
        public string keys_url { get; set; }
        public string collaborators_url { get; set; }
        public string teams_url { get; set; }
        public string hooks_url { get; set; }
        public string issue_events_url { get; set; }
        public string events_url { get; set; }
        public string assignees_url { get; set; }
        public string branches_url { get; set; }
        public string tags_url { get; set; }
        public string blobs_url { get; set; }
        public string git_tags_url { get; set; }
        public string git_refs_url { get; set; }
        public string trees_url { get; set; }
        public string statuses_url { get; set; }
        public string languages_url { get; set; }
        public string stargazers_url { get; set; }
        public string contributors_url { get; set; }
        public string subscribers_url { get; set; }
        public string subscription_url { get; set; }
        public string commits_url { get; set; }
        public string git_commits_url { get; set; }
        public string comments_url { get; set; }
        public string issue_comment_url { get; set; }
        public string contents_url { get; set; }
        public string compare_url { get; set; }
        public string merges_url { get; set; }
        public string archive_url { get; set; }
        public string downloads_url { get; set; }
        public string issues_url
        {
            get { return real_issue_url.Replace("https://api.github.com/", "").Replace("{/number}", ""); }
            set { real_issue_url = value; }
        }

        private string real_issue_url;
        public string pulls_url { get; set; }
        public string milestones_url { get; set; }
        private string _notifications_url;
        public string notifications_url
        {
            get { return _notifications_url.Replace("https://api.github.com/", "").Replace("{?since,all,participating}", ""); }
            set
            {
                _notifications_url = value;
            }
        }
        public string labels_url { get; set; }
        public string releases_url { get; set; }

        public override string ToString()
        {
            return "Repository:" + owner.login + "/" + name;
        }
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
            return await (service as GitHubService).GetIssue(match.Groups[1].Value, match.Groups[2].Value, int.Parse(match.Groups[3].Value));

        }

        public async Task<Release> GetRelease()
        {
            if (subject.type != "Release")
            {
                throw new ArgumentException("This should be an release");
            }
            Match match = Regex.Match(subject.url, "https:\\/\\/api.github.com\\/repos\\/([^//]+)\\/([^//]+)\\/releases\\/(\\d+)");
            return await (service as GitHubService).GetRelease(match.Groups[1].Value, match.Groups[2].Value, int.Parse(match.Groups[3].Value));
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
            return await (service as GitHubService).GetEvents(match.Groups[1].Value, match.Groups[2].Value, int.Parse(match.Groups[3].Value));

        }

        //Created by me
        public DateTime updated_at_dt => DateTime.Parse(updated_at);

        public override string ToString()
        {
            return subject.type + ": " + repository.full_name + " " + subject.title + " because you " + reason;
        }
    }


    public class Rootobject
    {
        public Issue[] Property1 { get; set; }
    }

    public class Issue : ObjectSearcher
    {
        private string notUrl;
        public string url { get { return notUrl.Replace("https://api.github.com/", ""); } set { notUrl = value; } }
        public string labels_url { get; set; }
        public string comments_url { get; set; }
        public string events_url { get; set; }
        public string html_url { get; set; }
        public int id { get; set; }
        public int number { get; set; }
        public string title { get; set; }
        public User user { get; set; }
        public Label[] labels { get; set; }
        public string state { get; set; }
        public bool locked { get; set; }
        public Assignee assignee { get; set; }
        public Milestone milestone { get; set; }
        public int comments { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public object closed_at { get; set; }
        public string body { get; set; }
        public Pull_Request pull_request { get; set; }

        //OOP calls

        public async Task<Issue> Modify(ModifyIssue it)
        {
            return await (service as GitHubService).ModifyIssue(url, it);
        }

        //Added by me

        public override string ToString()
        {
            return title + "[" + id + "]";
        }

        public override bool Equals(object obj)
        {
            return (obj as Issue)?.id == id;
        }

        public override int GetHashCode()
        {
            return id;
        }

        public ModifyIssue ConvertToRequest()
        {
            return new ModifyIssue { title = title, body = body, assignee = assignee?.login, milestone = milestone?.number, labels = labels.ToList().ConvertAll((Label input) => input.name) };
        }
    }

    public class User
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
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarLint", "S101:Class names should comply with a naming convention", Justification = "ReturnClass")]
    public class Pull_Request
    {
        public string url { get; set; }
        public string html_url { get; set; }
        public string diff_url { get; set; }
        public string patch_url { get; set; }
    }

    public class Assignee
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
    }

    public class Milestone
    {
        public string url { get; set; }
        public string html_url { get; set; }
        public string labels_url { get; set; }
        public int id { get; set; }
        public int number { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public User creator { get; set; }
        public int open_issues { get; set; }
        public int closed_issues { get; set; }
        public string state { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public object due_on { get; set; }
        public object closed_at { get; set; }
    }

    public class Label
    {
        public string url { get; set; }
        public string name { get; set; }
        public string color { get; set; }
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
        public int? milestone { get; set; }
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
        public Actor actor { get; set; }
        public string @event { get; set; }
        public object commit_id { get; set; }
        public object commit_url { get; set; }
        public DateTime created_at { get; set; }
        public Milestone milestone { get; set; }
        public Assignee assignee { get; set; }
        public Assigner assigner { get; set; }
        public Label label { get; set; }
    }

    public class Actor
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
    }

    public class Assigner
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
    }


}


