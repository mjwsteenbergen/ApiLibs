using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLibs.MicrosoftGraph
{
    public class GraphResult<T> {
        [JsonProperty("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonProperty("value")]
        public List<T> Value { get; set; }
    }

    public partial class Notebook
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }

        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }

        [JsonProperty("userRole")]
        public string UserRole { get; set; }

        [JsonProperty("isShared")]
        public bool IsShared { get; set; }

        [JsonProperty("sectionsUrl")]
        public Uri SectionsUrl { get; set; }

        [JsonProperty("sectionGroupsUrl")]
        public Uri SectionGroupsUrl { get; set; }

        [JsonProperty("createdBy")]
        public EdBy CreatedBy { get; set; }

        [JsonProperty("lastModifiedBy")]
        public EdBy LastModifiedBy { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }
    }

    public partial class RecentNotebook
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("lastAccessedTime")]
        public DateTimeOffset LastAccessedTime { get; set; }

        [JsonProperty("sourceService")]
        public string SourceService { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }
    }

    public partial class Links
    {
        [JsonProperty("oneNoteClientUrl")]
        public OneNoteUrl OneNoteClientUrl { get; set; }

        [JsonProperty("oneNoteWebUrl")]
        public OneNoteUrl OneNoteWebUrl { get; set; }
    }
    

    public partial class OneNoteUrl
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }

    public partial class Section
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }

        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }

        [JsonProperty("pagesUrl")]
        public Uri PagesUrl { get; set; }

        [JsonProperty("createdBy")]
        public EdBy CreatedBy { get; set; }

        [JsonProperty("lastModifiedBy")]
        public EdBy LastModifiedBy { get; set; }

        [JsonProperty("parentNotebook@odata.context")]
        public Uri ParentNotebookOdataContext { get; set; }

        [JsonProperty("parentNotebook")]
        public ParentNotebook ParentNotebook { get; set; }

        [JsonProperty("parentSectionGroup@odata.context")]
        public Uri ParentSectionGroupOdataContext { get; set; }

        [JsonProperty("parentSectionGroup")]
        public object ParentSectionGroup { get; set; }
    }

    public partial class ParentNotebook
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("self")]
        public Uri Self { get; set; }
    }

    public partial class Page
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("createdByAppId")]
        public string CreatedByAppId { get; set; }

        [JsonProperty("contentUrl")]
        public Uri ContentUrl { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }

        [JsonProperty("parentSection@odata.context")]
        public Uri ParentSectionOdataContext { get; set; }

        [JsonProperty("parentSection")]
        public ParentSection ParentSection { get; set; }
    }

    public partial class ParentSection
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("self")]
        public Uri Self { get; set; }
    }
}
