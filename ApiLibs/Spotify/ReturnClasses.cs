using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace ApiLibs.Spotify
{

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
        public SearcTrackResults tracks { get; set; }
        public SearchArtistResults artists { get; set; }
        public SearchAlbumResults albums { get; set; }
        public PlaylistResults playlists { get; set; }

    }

    public class PagingObject
    {
        public string href { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }
    }

    public class SearcTrackResults : PagingObject
    {
        public List<Track> items { get; set; }
    }

    public class SearchArtistResults : PagingObject
    {
        public List<Artist> items { get; set; }
    }

    public class SearchAlbumResults : PagingObject
    {
        public Playlist[] items { get; set; }
    }

    public class PlaylistResults : PagingObject
    {
        public Playlist[] items { get; set; }
    }

    public class Track
    {
        public Album album { get; set; }
        public List<Artist> artists { get; set; }
        public List<string> available_markets { get; set; }
        public int disc_number { get; set; }
        public int duration_ms { get; set; }
        public bool _explicit { get; set; }
        public External_Ids external_ids { get; set; }
        public External_Urls external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public bool is_playable { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string preview_url { get; set; }
        public int track_number { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Album
    {
        public string album_type { get; set; }
        public List<Artist> artists { get; set; }
        public List<string> available_markets { get; set; }
        public External_Urls external_urls { get; set; }
        public List<string> genres { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<Image> images { get; set; }
        public string label { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string release_date { get; set; }
        public string release_date_precision { get; set; }
        public Tracks tracks { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class External_Urls
    {
        public string spotify { get; set; }
    }

    public class Artist
    {
        public External_Urls external_urls { get; set; }
        public Followers followers { get; set; }
        public List<string> genres { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<Image> images { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string type { get; set; }
        public string uri { get; set; }

        public override string ToString()
        {
            return name;
        }
    }

    public class Image
    {
        public int? height { get; set; }
        public string url { get; set; }
        public int? width { get; set; }
    }

    public class External_Ids
    {
        public string isrc { get; set; }
    }

    public class Followers
    {
        public object href { get; set; }
        public int total { get; set; }
    }

    public class Playlist
    {
        public bool collaborative { get; set; }
        public External_Urls external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<Image> images { get; set; }
        public string name { get; set; }
        public Owner owner { get; set; }
        public object _public { get; set; }
        public string snapshot_id { get; set; }
        public Tracks tracks { get; set; }
        public string type { get; set; }
        public string uri { get; set; }

        public override string ToString()
        {
            return name + " from " + owner.href;
        }
    }

    public class Owner
    {
        public External_Urls external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Tracks
    {
        public string href { get; set; }
        public int total { get; set; }
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
}
