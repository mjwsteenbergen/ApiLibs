using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ApiLibs.BingMaps
{
    public partial class LocationSearchResult
    {
        [JsonProperty("authenticationResultCode")]
        public string AuthenticationResultCode { get; set; }

        [JsonProperty("brandLogoUri")]
        public Uri BrandLogoUri { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("resourceSets")]
        public List<ResourceSet> ResourceSets { get; set; }

        [JsonProperty("statusCode")]
        public long StatusCode { get; set; }

        [JsonProperty("statusDescription")]
        public string StatusDescription { get; set; }

        [JsonProperty("traceId")]
        public string TraceId { get; set; }
    }

    public partial class ResourceSet
    {
        [JsonProperty("estimatedTotal")]
        public long EstimatedTotal { get; set; }

        [JsonProperty("resources")]
        public List<Resource> Resources { get; set; }
    }

    public partial class Resource
    {
        [JsonProperty("__type")]
        public string Type { get; set; }

        [JsonProperty("bbox")]
        public List<double> Bbox { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("point")]
        public Point Point { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("confidence")]
        public string Confidence { get; set; }

        [JsonProperty("entityType")]
        public string EntityType { get; set; }

        [JsonProperty("geocodePoints")]
        public List<GeocodePoint> GeocodePoints { get; set; }

        [JsonProperty("matchCodes")]
        public List<string> MatchCodes { get; set; }
    }

    public partial class Address
    {
        [JsonProperty("addressLine")]
        public string AddressLine { get; set; }

        [JsonProperty("adminDistrict")]
        public string AdminDistrict { get; set; }

        [JsonProperty("adminDistrict2")]
        public string AdminDistrict2 { get; set; }

        [JsonProperty("countryRegion")]
        public string CountryRegion { get; set; }

        [JsonProperty("formattedAddress")]
        public string FormattedAddress { get; set; }

        [JsonProperty("locality")]
        public string Locality { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }
    }

    public partial class GeocodePoint
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public List<double> Coordinates { get; set; }

        [JsonProperty("calculationMethod")]
        public string CalculationMethod { get; set; }

        [JsonProperty("usageTypes")]
        public List<string> UsageTypes { get; set; }
    }

    public partial class Point
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public List<double> Coordinates { get; set; }
    }

    public partial class RouteResult
    {
        [JsonProperty("authenticationResultCode")]
        public string AuthenticationResultCode { get; set; }

        [JsonProperty("brandLogoUri")]
        public Uri BrandLogoUri { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("resourceSets")]
        public List<RouteResources> ResourceSets { get; set; }

        [JsonProperty("statusCode")]
        public long StatusCode { get; set; }

        [JsonProperty("statusDescription")]
        public string StatusDescription { get; set; }

        [JsonProperty("traceId")]
        public string TraceId { get; set; }
    }

    public partial class RouteResources
    {
        [JsonProperty("estimatedTotal")]
        public long EstimatedTotal { get; set; }

        [JsonProperty("resources")]
        public List<RouteResource> Resources { get; set; }
    }

    public partial class RouteResource
    {
        [JsonProperty("__type")]
        public string Type { get; set; }

        [JsonProperty("bbox")]
        public List<double> Bbox { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("distanceUnit")]
        public string DistanceUnit { get; set; }

        [JsonProperty("durationUnit")]
        public string DurationUnit { get; set; }

        [JsonProperty("routeLegs")]
        public List<RouteLeg> RouteLegs { get; set; }

        [JsonProperty("trafficCongestion")]
        public string TrafficCongestion { get; set; }

        [JsonProperty("trafficDataUsed")]
        public string TrafficDataUsed { get; set; }

        [JsonProperty("travelDistance")]
        public double TravelDistance { get; set; }

        [JsonProperty("travelDuration")]
        public long TravelDuration { get; set; }

        [JsonProperty("travelDurationTraffic")]
        public long TravelDurationTraffic { get; set; }
    }

    public partial class RouteLeg
    {
        [JsonProperty("actualEnd")]
        public ActualEnd ActualEnd { get; set; }

        [JsonProperty("actualStart")]
        public ActualEnd ActualStart { get; set; }

        [JsonProperty("alternateVias")]
        public List<object> AlternateVias { get; set; }

        [JsonProperty("cost")]
        public long Cost { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("endLocation")]
        public EndLocation EndLocation { get; set; }

        [JsonProperty("itineraryItems")]
        public List<ItineraryItem> ItineraryItems { get; set; }

        [JsonProperty("routeRegion")]
        public string RouteRegion { get; set; }

        [JsonProperty("routeSubLegs")]
        public List<RouteSubLeg> RouteSubLegs { get; set; }

        [JsonProperty("startLocation")]
        public StartLocation StartLocation { get; set; }

        [JsonProperty("travelDistance")]
        public double TravelDistance { get; set; }

        [JsonProperty("travelDuration")]
        public long TravelDuration { get; set; }

        [JsonProperty("endTime")]
        public string EndTime { get; set; }

        [JsonProperty("startTime")]
        public string StartTime { get; set; }

    }

    public partial class ActualEnd
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public List<double> Coordinates { get; set; }
    }

    public partial class EndLocation
    {
        [JsonProperty("bbox")]
        public List<double> Bbox { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("point")]
        public ActualEnd Point { get; set; }

        [JsonProperty("address")]
        public EndLocationAddress Address { get; set; }

        [JsonProperty("confidence")]
        public string Confidence { get; set; }

        [JsonProperty("entityType")]
        public string EntityType { get; set; }

        [JsonProperty("geocodePoints")]
        public List<GeocodePoint> GeocodePoints { get; set; }

        [JsonProperty("matchCodes")]
        public List<string> MatchCodes { get; set; }
    }

    public partial class EndLocationAddress
    {
        [JsonProperty("adminDistrict")]
        public string AdminDistrict { get; set; }

        [JsonProperty("adminDistrict2")]
        public string AdminDistrict2 { get; set; }

        [JsonProperty("countryRegion")]
        public string CountryRegion { get; set; }

        [JsonProperty("formattedAddress")]
        public string FormattedAddress { get; set; }

        [JsonProperty("locality")]
        public string Locality { get; set; }
    }

    public partial class ItineraryItem
    {
        [JsonProperty("compassDirection")]
        public string CompassDirection { get; set; }

        [JsonProperty("details")]
        public List<Detail> Details { get; set; }

        [JsonProperty("exit")]
        public string Exit { get; set; }

        [JsonProperty("iconType")]
        public string IconType { get; set; }

        [JsonProperty("instruction")]
        public Instruction Instruction { get; set; }

        [JsonProperty("isRealTimeTransit")]
        public bool IsRealTimeTransit { get; set; }

        [JsonProperty("maneuverPoint")]
        public ActualEnd ManeuverPoint { get; set; }

        [JsonProperty("realTimeTransitDelay")]
        public long RealTimeTransitDelay { get; set; }

        [JsonProperty("sideOfStreet")]
        public string SideOfStreet { get; set; }

        [JsonProperty("tollZone")]
        public string TollZone { get; set; }

        [JsonProperty("towardsRoadName", NullValueHandling = NullValueHandling.Ignore)]
        public string TowardsRoadName { get; set; }

        [JsonProperty("transitTerminus")]
        public string TransitTerminus { get; set; }

        [JsonProperty("travelDistance")]
        public double TravelDistance { get; set; }

        [JsonProperty("travelDuration")]
        public long TravelDuration { get; set; }

        [JsonProperty("travelMode")]
        public string TravelMode { get; set; }

        [JsonProperty("signs", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Signs { get; set; }

        [JsonProperty("warnings", NullValueHandling = NullValueHandling.Ignore)]
        public List<Warning> Warnings { get; set; }

        [JsonProperty("hints", NullValueHandling = NullValueHandling.Ignore)]
        public List<Hint> Hints { get; set; }

        [JsonProperty("childItineraryItems", NullValueHandling = NullValueHandling.Ignore)]
        public List<ChildItineraryItem> ChildItineraryItems { get; set; }

        [JsonProperty("transitLine", NullValueHandling = NullValueHandling.Ignore)]
        public TransitLine TransitLine { get; set; }

        [JsonProperty("wheelchairAccessible", NullValueHandling = NullValueHandling.Ignore)]
        public bool? WheelchairAccessible { get; set; }

        [JsonProperty("bicyclesAllowed", NullValueHandling = NullValueHandling.Ignore)]
        public bool? BicyclesAllowed { get; set; }
    }

    public partial class Detail
    {
        [JsonProperty("compassDegrees")]
        public long CompassDegrees { get; set; }

        [JsonProperty("endPathIndices")]
        public List<long> EndPathIndices { get; set; }

        [JsonProperty("maneuverType")]
        public string ManeuverType { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("names", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Names { get; set; }

        [JsonProperty("roadType")]
        public string RoadType { get; set; }

        [JsonProperty("startPathIndices")]
        public List<long> StartPathIndices { get; set; }

        [JsonProperty("locationCodes", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> LocationCodes { get; set; }

        [JsonProperty("roadShieldRequestParameters", NullValueHandling = NullValueHandling.Ignore)]
        public RoadShieldRequestParameters RoadShieldRequestParameters { get; set; }
    }

    public partial class RoadShieldRequestParameters
    {
        [JsonProperty("bucket")]
        public long Bucket { get; set; }

        [JsonProperty("shields")]
        public List<Shield> Shields { get; set; }
    }

    public partial class Shield
    {
        [JsonProperty("labels")]
        public List<string> Labels { get; set; }

        [JsonProperty("roadShieldType")]
        public long RoadShieldType { get; set; }
    }

    public partial class Hint
    {
        [JsonProperty("hintType")]
        public string HintType { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public partial class Instruction
    {
        [JsonProperty("formattedText")]
        public object FormattedText { get; set; }

        [JsonProperty("maneuverType")]
        public string ManeuverType { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public partial class Warning
    {
        [JsonProperty("origin", NullValueHandling = NullValueHandling.Ignore)]
        public string Origin { get; set; }

        [JsonProperty("severity")]
        public string Severity { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("to", NullValueHandling = NullValueHandling.Ignore)]
        public string To { get; set; }

        [JsonProperty("warningType")]
        public string WarningType { get; set; }
    }

    public partial class RouteSubLeg
    {
        [JsonProperty("endWaypoint")]
        public Waypoint EndWaypoint { get; set; }

        [JsonProperty("startWaypoint")]
        public Waypoint StartWaypoint { get; set; }

        [JsonProperty("travelDistance")]
        public double TravelDistance { get; set; }

        [JsonProperty("travelDuration")]
        public long TravelDuration { get; set; }
    }

    public partial class Waypoint
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public List<double> Coordinates { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("isVia")]
        public bool IsVia { get; set; }

        [JsonProperty("locationIdentifier")]
        public string LocationIdentifier { get; set; }

        [JsonProperty("routePathIndex")]
        public long RoutePathIndex { get; set; }
    }

    public partial class StartLocation
    {
        [JsonProperty("bbox")]
        public List<double> Bbox { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("point")]
        public ActualEnd Point { get; set; }

        [JsonProperty("address")]
        public StartLocationAddress Address { get; set; }

        [JsonProperty("confidence")]
        public string Confidence { get; set; }

        [JsonProperty("entityType")]
        public string EntityType { get; set; }

        [JsonProperty("geocodePoints")]
        public List<GeocodePoint> GeocodePoints { get; set; }

        [JsonProperty("matchCodes")]
        public List<string> MatchCodes { get; set; }
    }

    public partial class StartLocationAddress
    {
        [JsonProperty("addressLine")]
        public string AddressLine { get; set; }

        [JsonProperty("adminDistrict")]
        public string AdminDistrict { get; set; }

        [JsonProperty("adminDistrict2")]
        public string AdminDistrict2 { get; set; }

        [JsonProperty("countryRegion")]
        public string CountryRegion { get; set; }

        [JsonProperty("formattedAddress")]
        public string FormattedAddress { get; set; }

        [JsonProperty("locality")]
        public string Locality { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }
    }

    public partial class ChildItineraryItem
    {
        [JsonProperty("compassDirection")]
        public string CompassDirection { get; set; }

        [JsonProperty("details")]
        public List<ChildItineraryItemDetail> Details { get; set; }

        [JsonProperty("exit")]
        public string Exit { get; set; }

        [JsonProperty("iconType")]
        public string IconType { get; set; }

        [JsonProperty("instruction")]
        public Instruction Instruction { get; set; }

        [JsonProperty("isRealTimeTransit")]
        public bool IsRealTimeTransit { get; set; }

        [JsonProperty("maneuverPoint")]
        public ActualEnd ManeuverPoint { get; set; }

        [JsonProperty("realTimeTransitDelay")]
        public long RealTimeTransitDelay { get; set; }

        [JsonProperty("sideOfStreet")]
        public string SideOfStreet { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("tollZone")]
        public string TollZone { get; set; }

        [JsonProperty("transitStopId")]
        public long TransitStopId { get; set; }

        [JsonProperty("transitTerminus")]
        public string TransitTerminus { get; set; }

        [JsonProperty("travelDistance")]
        public long TravelDistance { get; set; }

        [JsonProperty("travelDuration")]
        public long TravelDuration { get; set; }

        [JsonProperty("travelMode")]
        public string TravelMode { get; set; }

        [JsonProperty("tripId")]
        public long TripId { get; set; }
    }

    public partial class ChildItineraryItemDetail
    {
        [JsonProperty("maneuverType")]
        public string ManeuverType { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("names")]
        public List<string> Names { get; set; }

        [JsonProperty("roadType")]
        public string RoadType { get; set; }
    }

    public partial class ItineraryItemDetail
    {
        [JsonProperty("endPathIndices")]
        public List<long> EndPathIndices { get; set; }

        [JsonProperty("maneuverType")]
        public string ManeuverType { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("roadType")]
        public string RoadType { get; set; }

        [JsonProperty("startPathIndices")]
        public List<long> StartPathIndices { get; set; }
    }

    public partial class TransitLine
    {
        [JsonProperty("abbreviatedName")]
        public string AbbreviatedName { get; set; }

        [JsonProperty("agencyId")]
        public long AgencyId { get; set; }

        [JsonProperty("agencyName")]
        public string AgencyName { get; set; }

        [JsonProperty("lineColor")]
        public long LineColor { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("providerInfo")]
        public string ProviderInfo { get; set; }

        [JsonProperty("transportType")]
        public long TransportType { get; set; }

        [JsonProperty("uri")]
        public Uri Uri { get; set; }

        [JsonProperty("verboseName")]
        public string VerboseName { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }

    

}
