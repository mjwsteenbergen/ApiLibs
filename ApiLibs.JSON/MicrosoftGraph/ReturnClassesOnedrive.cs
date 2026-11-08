using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Martijn.Extensions.JSON.Compat;

namespace ApiLibs.JSON.MicrosoftGraph
{
    public partial class FolderChildrenRoot
    {
        [JsonPropertyName("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonPropertyName("value")]
        public List<DriveItem> Value { get; set; }
    }

    public partial class DriveItem
    {
        [JsonPropertyName("createdDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonPropertyName("cTag")]
        public string CTag { get; set; }

        [JsonPropertyName("eTag")]
        public string ETag { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("webUrl")]
        public Uri WebUrl { get; set; }

        [JsonPropertyName("createdBy")]
        public EdBy CreatedBy { get; set; }

        [JsonPropertyName("lastModifiedBy")]
        public EdBy LastModifiedBy { get; set; }

        [JsonPropertyName("parentReference")]
        public ParentReference ParentReference { get; set; }

        [JsonPropertyName("fileSystemInfo")]
        public FileSystemInfo FileSystemInfo { get; set; }

        [JsonPropertyName("folder"), NullValueHandling(NullHandling.Ignore)]
        public ChildFolder ChildFolder { get; set; }

        [JsonPropertyName("specialFolder"), NullValueHandling(NullHandling.Ignore)]
        public SpecialFolder SpecialFolder { get; set; }

        [JsonPropertyName("shared"), NullValueHandling(NullHandling.Ignore)]
        public Shared Shared { get; set; }

        [JsonPropertyName("@microsoft.graph.downloadUrl"), NullValueHandling(NullHandling.Ignore)]
        public Uri MicrosoftGraphDownloadUrl { get; set; }

        [JsonPropertyName("file"), NullValueHandling(NullHandling.Ignore)]
        public File File { get; set; }

        [JsonPropertyName("image"), NullValueHandling(NullHandling.Ignore)]
        public Image Image { get; set; }

        [JsonPropertyName("photo"), NullValueHandling(NullHandling.Ignore)]
        public Photo Photo { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public partial class EdBy
    {
        [JsonPropertyName("user")]
        public User User { get; set; }

        [JsonPropertyName("application"), NullValueHandling(NullHandling.Ignore)]
        public User Application { get; set; }

        [JsonPropertyName("device"), NullValueHandling(NullHandling.Ignore)]
        public Device Device { get; set; }

        [JsonPropertyName("oneDriveSync"), NullValueHandling(NullHandling.Ignore)]
        public OneDriveSync OneDriveSync { get; set; }

        public override string ToString()
        {
            return User?.DisplayName ?? "Has no username";
        }
    }

    public partial class User
    {
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public partial class Device
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public partial class OneDriveSync
    {
        [JsonPropertyName("@odata.type")]
        public string OdataType { get; set; }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }

    public partial class File
    {
        [JsonPropertyName("mimeType")]
        public string MimeType { get; set; }

        [JsonPropertyName("hashes")]
        public Hashes Hashes { get; set; }
    }

    public partial class Hashes
    {
        [JsonPropertyName("crc32Hash")]
        public string Crc32Hash { get; set; }

        [JsonPropertyName("sha1Hash")]
        public string Sha1Hash { get; set; }
    }

    public partial class FileSystemInfo
    {
        [JsonPropertyName("createdDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonPropertyName("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }
    }

    public partial class ChildFolder
    {
        [JsonPropertyName("childCount")]
        public long ChildCount { get; set; }

        [JsonPropertyName("view")]
        public View View { get; set; }
    }

    public partial class View
    {
        [JsonPropertyName("viewType")]
        public string ViewType { get; set; }

        [JsonPropertyName("sortBy")]
        public string SortBy { get; set; }

        [JsonPropertyName("sortOrder")]
        public string SortOrder { get; set; }
    }

    public partial class Image
    {
        [JsonPropertyName("height")]
        public long Height { get; set; }

        [JsonPropertyName("width")]
        public long Width { get; set; }
    }

    public partial class ParentReference
    {
        [JsonPropertyName("driveId")]
        public string DriveId { get; set; }

        [JsonPropertyName("driveType")]
        public string DriveType { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("path")]
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
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("owner")]
        public Owner Owner { get; set; }
    }

    public partial class Owner
    {
        [JsonPropertyName("user")]
        public User User { get; set; }
    }

    public partial class SpecialFolder
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
