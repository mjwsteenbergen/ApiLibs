using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

// ReSharper disable InconsistentNaming

namespace ApiLibs.JSON.Spotify
{


    public partial class Playlist
    {
        [JsonPropertyName("collaborative")]
        public bool Collaborative { get; set; }

        [JsonPropertyName("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("images")]
        public List<Image> Images { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("owner")]
        public Owner Owner { get; set; }

        [JsonPropertyName("primary_color")]
        public object PrimaryColor { get; set; }

        [JsonPropertyName("public")]
        public bool Public { get; set; }

        [JsonPropertyName("snapshot_id")]
        public string SnapshotId { get; set; }

        [JsonPropertyName("tracks")]
        public Tracks Tracks { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        public override string ToString()
        {
            return Name + " from " + Owner.Href;
        }
    }

    public partial class Owner
    {
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        [JsonPropertyName("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }

    public partial class Tracks
    {
        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("total")]
        public float Total { get; set; }
    }
    public class AudioFeatureList
    {
        [JsonPropertyName("audio_features")]
        public List<AudioFeatures> AudioFeatures { get; set; }
    }

    public partial class SavedTrack
    {
        [JsonPropertyName("added_at")]
        public DateTimeOffset AddedAt { get; set; }

        [JsonPropertyName("track")]
        public Track Track { get; set; }
    }

    public partial class Track
    {
        [JsonPropertyName("album")]
        public Album Album { get; set; }

        [JsonPropertyName("artists")]
        public List<Artist> Artists { get; set; }

        [JsonPropertyName("available_markets")]
        public List<string> AvailableMarkets { get; set; }

        [JsonPropertyName("disc_number")]
        public long DiscNumber { get; set; }

        [JsonPropertyName("duration_ms")]
        public long DurationMs { get; set; }

        [JsonPropertyName("explicit")]
        public bool Explicit { get; set; }

        [JsonPropertyName("external_ids")]
        public External_Ids ExternalIds { get; set; }

        [JsonPropertyName("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("is_local")]
        public bool IsLocal { get; set; }

        public bool is_playable { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("popularity")]
        public float Popularity { get; set; }

        [JsonPropertyName("preview_url")]
        public string PreviewUrl { get; set; }

        [JsonPropertyName("track_number")]
        public long TrackNumber { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }

    public partial class Album
    {
        [JsonPropertyName("album_type")]
        public AlbumType AlbumType { get; set; }

        [JsonPropertyName("artists")]
        public List<Artist> Artists { get; set; }

        [JsonPropertyName("available_markets")]
        public List<string> AvailableMarkets { get; set; }

        [JsonPropertyName("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        public List<string> genres { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("images")]
        public List<Image> Images { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        public string label { get; set; }

        public float popularity { get; set; }

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; }

        [JsonPropertyName("release_date_precision")]
        public string ReleaseDatePrecision { get; set; }

        public Tracks tracks { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }

    public partial class Artist
    {
        [JsonPropertyName("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonPropertyName("followers")]
        public Followers Followers { get; set; }
        
        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("images")]
        public List<Image> Images { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("popularity")]
        public float Popularity { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public partial class ExternalUrls
    {
        [JsonPropertyName("spotify")]
        public string Spotify { get; set; }
    }

    public partial class Image
    {
        [JsonPropertyName("height")]
        public long? Height { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("width")]
        public long? Width { get; set; }
    }

    public class AccessTokenObject
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
    }


    public class SearchObject
    {
        public TrackResultsResponse tracks { get; set; }
        public ArtistResultsResponse artists { get; set; }
        public AlbumResultsResponse albums { get; set; }
        public PlaylistResultsResponse playlists { get; set; }

    }

    public class PagingObject<T>
    {
        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("limit")]
        public float Limit { get; set; }

        [JsonPropertyName("next")]
        public string Next { get; set; }

        [JsonPropertyName("offset")]
        public long Offset { get; set; }

        [JsonPropertyName("previous")]
        public object Previous { get; set; }

        [JsonPropertyName("total")]
        public float Total { get; set; }

        [JsonPropertyName("items")]
        public List<T> Items { get; set; }
    }

    public partial class PlaylistResultsResponse : PagingObject<Playlist>
    {

    }

    public partial class PlaylistTrackResponse : PagingObject<PlayListTrack>
    {
    }

    public partial class SavedTrackResponse : PagingObject<SavedTrack>
    {
    }

    public partial class PlayListTrack
    {
        [JsonPropertyName("added_at")]
        public DateTimeOffset AddedAt { get; set; }

        [JsonPropertyName("added_by")]
        public Owner AddedBy { get; set; }

        [JsonPropertyName("is_local")]
        public bool IsLocal { get; set; }

        [JsonPropertyName("primary_color")]
        public object PrimaryColor { get; set; }

        [JsonPropertyName("track")]
        public Track Track { get; set; }
    }

    public class TrackResponse
    {
        [JsonPropertyName("tracks")]
        public List<Track> Tracks { get; set; }
    }


    public class TrackResultsResponse : PagingObject<Track>
    {
    }

    public class ArtistResultsResponse : PagingObject<Artist>
    {
    }

    public class AlbumResultsResponse : PagingObject<Album>
    {
    }

    public class External_Urls
    {
        public string spotify { get; set; }
    }

    public class External_Ids
    {
        public string isrc { get; set; }
    }

    public class Followers
    {
        public string href { get; set; }
        public double total { get; set; }
    }

    public class AudioAnalysis
    {
        public Meta meta { get; set; }
        public Track track { get; set; }
        public Bar[] bars { get; set; }
        public Beat[] beats { get; set; }
        public Tatum[] tatums { get; set; }
        public Section[] sections { get; set; }
        public Segment[] segments { get; set; }
    }

    public class Meta
    {
        public string analyzer_version { get; set; }
        public string platform { get; set; }
        public string detailed_status { get; set; }
        public int status_code { get; set; }
        public int timestamp { get; set; }
        public float analysis_time { get; set; }
        public string input_process { get; set; }
    }

    public class TrackAnalysis
    {
        public int num_samples { get; set; }
        public float duration { get; set; }
        public string sample_md5 { get; set; }
        public int offset_seconds { get; set; }
        public int window_seconds { get; set; }
        public int analysis_sample_rate { get; set; }
        public int analysis_channels { get; set; }
        public float end_of_fade_in { get; set; }
        public float start_of_fade_out { get; set; }
        public float loudness { get; set; }
        public float tempo { get; set; }
        public float tempo_confidence { get; set; }
        public int time_signature { get; set; }
        public float time_signature_confidence { get; set; }
        public int key { get; set; }
        public float key_confidence { get; set; }
        public int mode { get; set; }
        public float mode_confidence { get; set; }
        public string codestring { get; set; }
        public float code_version { get; set; }
        public string echoprintstring { get; set; }
        public float echoprint_version { get; set; }
        public string synchstring { get; set; }
        public float synch_version { get; set; }
        public string rhythmstring { get; set; }
        public float rhythm_version { get; set; }
    }

    public class Bar
    {
        public float start { get; set; }
        public float duration { get; set; }
        public float confidence { get; set; }
    }

    public class Beat
    {
        public float start { get; set; }
        public float duration { get; set; }
        public float confidence { get; set; }
    }

    public class Tatum
    {
        public float start { get; set; }
        public float duration { get; set; }
        public float confidence { get; set; }
    }

    public class Section
    {
        public float start { get; set; }
        public float duration { get; set; }
        public float confidence { get; set; }
        public float loudness { get; set; }
        public float tempo { get; set; }
        public float tempo_confidence { get; set; }
        public int key { get; set; }
        public float key_confidence { get; set; }
        public int mode { get; set; }
        public float mode_confidence { get; set; }
        public int time_signature { get; set; }
        public float time_signature_confidence { get; set; }
    }

    public class Segment
    {
        public float start { get; set; }
        public float duration { get; set; }
        public float confidence { get; set; }
        public float loudness_start { get; set; }
        public float loudness_max_time { get; set; }
        public float loudness_max { get; set; }
        public float[] pitches { get; set; }
        public float[] timbre { get; set; }
        public float loudness_end { get; set; }
    }


    public class AudioFeatures
    {
        public float danceability { get; set; }
        public float energy { get; set; }
        public int key { get; set; }
        public float loudness { get; set; }
        public int mode { get; set; }
        public float speechiness { get; set; }
        public float acousticness { get; set; }
        public float instrumentalness { get; set; }
        public float liveness { get; set; }
        public float valence { get; set; }
        public float tempo { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public string uri { get; set; }
        public string track_href { get; set; }
        public string analysis_url { get; set; }
        public int duration_ms { get; set; }
        public int time_signature { get; set; }
    }


    public class DeviceList
    {
        public List<Device> devices { get; set; }
    }

    public class Device
    {
        public string id { get; set; }
        public bool is_active { get; set; }
        public bool is_restricted { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int? volume_percent { get; set; }
    }


    public class PlaybackState
    {
        public long timestamp { get; set; }
        public int progress_ms { get; set; }
        public bool is_playing { get; set; }
        public Track item { get; set; }
        public Context context { get; set; }
        public Device device { get; set; }
        public string repeat_state { get; set; }
        public bool shuffle_state { get; set; }
    }

    public class Context
    {
        public External_Urls external_urls { get; set; }
        public string href { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }


    public class CurrentlyPlayingState
    {
        public long timestamp { get; set; }
        public int progress_ms { get; set; }
        public bool is_playing { get; set; }
        public Track item { get; set; }
        public Context context { get; set; }
    }


    public class MeObject
    {
        public string country { get; set; }
        public string display_name { get; set; }
        public External_Urls external_urls { get; set; }
        public Followers followers { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public object[] images { get; set; }
        public string product { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }
}
