using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ApiLibs.MicrosoftGraph
{
    public partial class FolderChildrenRoot
    {
        [JsonProperty("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonProperty("value")]
        public List<DriveItem> Value { get; set; }
    }

    public partial class DriveItem
    {
        [JsonProperty("createdDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonProperty("cTag")]
        public string CTag { get; set; }

        [JsonProperty("eTag")]
        public string ETag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("webUrl")]
        public Uri WebUrl { get; set; }

        [JsonProperty("createdBy")]
        public EdBy CreatedBy { get; set; }

        [JsonProperty("lastModifiedBy")]
        public EdBy LastModifiedBy { get; set; }

        [JsonProperty("parentReference")]
        public ParentReference ParentReference { get; set; }

        [JsonProperty("fileSystemInfo")]
        public FileSystemInfo FileSystemInfo { get; set; }

        [JsonProperty("folder", NullValueHandling = NullValueHandling.Ignore)]
        public ChildFolder ChildFolder { get; set; }

        [JsonProperty("specialFolder", NullValueHandling = NullValueHandling.Ignore)]
        public SpecialFolder SpecialFolder { get; set; }

        [JsonProperty("shared", NullValueHandling = NullValueHandling.Ignore)]
        public Shared Shared { get; set; }

        [JsonProperty("@microsoft.graph.downloadUrl", NullValueHandling = NullValueHandling.Ignore)]
        public Uri MicrosoftGraphDownloadUrl { get; set; }

        [JsonProperty("file", NullValueHandling = NullValueHandling.Ignore)]
        public File File { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public Image Image { get; set; }

        [JsonProperty("photo", NullValueHandling = NullValueHandling.Ignore)]
        public Photo Photo { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public partial class EdBy
    {
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("application", NullValueHandling = NullValueHandling.Ignore)]
        public User Application { get; set; }

        [JsonProperty("device", NullValueHandling = NullValueHandling.Ignore)]
        public Device Device { get; set; }

        [JsonProperty("oneDriveSync", NullValueHandling = NullValueHandling.Ignore)]
        public OneDriveSync OneDriveSync { get; set; }

        public override string ToString()
        {
            return User?.DisplayName ?? "Has no username";
        }
    }

    public partial class User
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class Device
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class OneDriveSync
    {
        [JsonProperty("@odata.type")]
        public string OdataType { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }
    }

    public partial class File
    {
        [JsonProperty("mimeType")]
        public string MimeType { get; set; }

        [JsonProperty("hashes")]
        public Hashes Hashes { get; set; }
    }

    public partial class Hashes
    {
        [JsonProperty("crc32Hash")]
        public string Crc32Hash { get; set; }

        [JsonProperty("sha1Hash")]
        public string Sha1Hash { get; set; }
    }

    public partial class FileSystemInfo
    {
        [JsonProperty("createdDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }
    }

    public partial class ChildFolder
    {
        [JsonProperty("childCount")]
        public long ChildCount { get; set; }

        [JsonProperty("view")]
        public View View { get; set; }
    }

    public partial class View
    {
        [JsonProperty("viewType")]
        public string ViewType { get; set; }

        [JsonProperty("sortBy")]
        public string SortBy { get; set; }

        [JsonProperty("sortOrder")]
        public string SortOrder { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }
    }

    public partial class ParentReference
    {
        [JsonProperty("driveId")]
        public string DriveId { get; set; }

        [JsonProperty("driveType")]
        public string DriveType { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public partial class Photo
    {
    }

    public partial class Shared
    {
        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }
    }

    public partial class Owner
    {
        [JsonProperty("user")]
        public User User { get; set; }
    }

    public partial class SpecialFolder
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
