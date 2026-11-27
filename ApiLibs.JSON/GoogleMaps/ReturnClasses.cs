using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiLibs.JSON.GoogleMaps
{
    public partial class PlaceSearchResult
    {
        [JsonPropertyName("candidates")]
        public List<Candidate> Candidates { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public partial class Candidate
    {
        [JsonPropertyName("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; }

        [JsonPropertyName("more_opening_hours")]
        public List<object> MoreOpeningHours { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public partial class Geometry
    {
        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("viewport")]
        public Viewport Viewport { get; set; }
    }

    public partial class Location
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }
    }

    public partial class Viewport
    {
        [JsonPropertyName("northeast")]
        public Location Northeast { get; set; }

        [JsonPropertyName("southwest")]
        public Location Southwest { get; set; }
    }
}
