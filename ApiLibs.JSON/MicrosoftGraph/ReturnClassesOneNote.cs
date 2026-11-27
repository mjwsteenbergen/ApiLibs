using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace ApiLibs.JSON.MicrosoftGraph
{
    public class GraphResult<T> {
        [JsonPropertyName("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonPropertyName("value")]
        public List<T> Value { get; set; }
    }

    public partial class Notebook
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("self")]
        public Uri Self { get; set; }

        [JsonPropertyName("createdDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }

        [JsonPropertyName("isDefault")]
        public bool IsDefault { get; set; }

        [JsonPropertyName("userRole")]
        public string UserRole { get; set; }

        [JsonPropertyName("isShared")]
        public bool IsShared { get; set; }

        [JsonPropertyName("sectionsUrl")]
        public Uri SectionsUrl { get; set; }

        [JsonPropertyName("sectionGroupsUrl")]
        public Uri SectionGroupsUrl { get; set; }

        [JsonPropertyName("createdBy")]
        public EdBy CreatedBy { get; set; }

        [JsonPropertyName("lastModifiedBy")]
        public EdBy LastModifiedBy { get; set; }

        [JsonPropertyName("links")]
        public Links Links { get; set; }
    }

    public partial class RecentNotebook
    {
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("lastAccessedTime")]
        public DateTimeOffset LastAccessedTime { get; set; }

        [JsonPropertyName("sourceService")]
        public string SourceService { get; set; }

        [JsonPropertyName("links")]
        public Links Links { get; set; }
    }

    public partial class Links
    {
        [JsonPropertyName("oneNoteClientUrl")]
        public OneNoteUrl OneNoteClientUrl { get; set; }

        [JsonPropertyName("oneNoteWebUrl")]
        public OneNoteUrl OneNoteWebUrl { get; set; }
    }
    

    public partial class OneNoteUrl
    {
        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

    public partial class Section
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("self")]
        public Uri Self { get; set; }

        [JsonPropertyName("createdDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }

        [JsonPropertyName("isDefault")]
        public bool IsDefault { get; set; }

        [JsonPropertyName("pagesUrl")]
        public Uri PagesUrl { get; set; }

        [JsonPropertyName("createdBy")]
        public EdBy CreatedBy { get; set; }

        [JsonPropertyName("lastModifiedBy")]
        public EdBy LastModifiedBy { get; set; }

        [JsonPropertyName("parentNotebook@odata.context")]
        public Uri ParentNotebookOdataContext { get; set; }

        [JsonPropertyName("parentNotebook")]
        public ParentNotebook ParentNotebook { get; set; }

        [JsonPropertyName("parentSectionGroup@odata.context")]
        public Uri ParentSectionGroupOdataContext { get; set; }

        [JsonPropertyName("parentSectionGroup")]
        public object ParentSectionGroup { get; set; }
    }

    public partial class ParentNotebook
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("self")]
        public Uri Self { get; set; }
    }

    public partial class Page
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("self")]
        public Uri Self { get; set; }

        [JsonPropertyName("createdDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("createdByAppId")]
        public string CreatedByAppId { get; set; }

        [JsonPropertyName("contentUrl")]
        public Uri ContentUrl { get; set; }

        [JsonPropertyName("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }

        [JsonPropertyName("links")]
        public Links Links { get; set; }

        [JsonPropertyName("parentSection@odata.context")]
        public Uri ParentSectionOdataContext { get; set; }

        [JsonPropertyName("parentSection")]
        public ParentSection ParentSection { get; set; }
    }

    public partial class ParentSection
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("self")]
        public Uri Self { get; set; }
    }
}
