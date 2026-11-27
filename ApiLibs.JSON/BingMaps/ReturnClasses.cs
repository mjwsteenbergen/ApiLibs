using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Martijn.Extensions.JSON.Compat;

namespace ApiLibs.JSON.BingMaps
{
    public partial class LocationSearchResult
    {
        [JsonPropertyName("authenticationResultCode")]
        public string AuthenticationResultCode { get; set; }

        [JsonPropertyName("brandLogoUri")]
        public Uri BrandLogoUri { get; set; }

        [JsonPropertyName("copyright")]
        public string Copyright { get; set; }

        [JsonPropertyName("resourceSets")]
        public List<ResourceSet> ResourceSets { get; set; }

        [JsonPropertyName("statusCode")]
        public long StatusCode { get; set; }

        [JsonPropertyName("statusDescription")]
        public string StatusDescription { get; set; }

        [JsonPropertyName("traceId")]
        public string TraceId { get; set; }
    }

    public partial class ResourceSet
    {
        [JsonPropertyName("estimatedTotal")]
        public long EstimatedTotal { get; set; }

        [JsonPropertyName("resources")]
        public List<Resource> Resources { get; set; }
    }

    public partial class Resource
    {
        [JsonPropertyName("__type")]
        public string Type { get; set; }

        [JsonPropertyName("bbox")]
        public List<double> Bbox { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("point")]
        public Point Point { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }

        [JsonPropertyName("confidence")]
        public string Confidence { get; set; }

        [JsonPropertyName("entityType")]
        public string EntityType { get; set; }

        [JsonPropertyName("geocodePoints")]
        public List<GeocodePoint> GeocodePoints { get; set; }

        [JsonPropertyName("matchCodes")]
        public List<string> MatchCodes { get; set; }
    }

    public partial class Address
    {
        [JsonPropertyName("addressLine")]
        public string AddressLine { get; set; }

        [JsonPropertyName("adminDistrict")]
        public string AdminDistrict { get; set; }

        [JsonPropertyName("adminDistrict2")]
        public string AdminDistrict2 { get; set; }

        [JsonPropertyName("countryRegion")]
        public string CountryRegion { get; set; }

        [JsonPropertyName("formattedAddress")]
        public string FormattedAddress { get; set; }

        [JsonPropertyName("locality")]
        public string Locality { get; set; }

        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }
    }

    public partial class GeocodePoint
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("coordinates")]
        public List<double> Coordinates { get; set; }

        [JsonPropertyName("calculationMethod")]
        public string CalculationMethod { get; set; }

        [JsonPropertyName("usageTypes")]
        public List<string> UsageTypes { get; set; }
    }

    public partial class Point
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("coordinates")]
        public List<double> Coordinates { get; set; }
    }

    public partial class RouteResult
    {
        [JsonPropertyName("authenticationResultCode")]
        public string AuthenticationResultCode { get; set; }

        [JsonPropertyName("brandLogoUri")]
        public Uri BrandLogoUri { get; set; }

        [JsonPropertyName("copyright")]
        public string Copyright { get; set; }

        [JsonPropertyName("resourceSets")]
        public List<RouteResources> ResourceSets { get; set; }

        [JsonPropertyName("statusCode")]
        public long StatusCode { get; set; }

        [JsonPropertyName("statusDescription")]
        public string StatusDescription { get; set; }

        [JsonPropertyName("traceId")]
        public string TraceId { get; set; }
    }

    public partial class RouteResources
    {
        [JsonPropertyName("estimatedTotal")]
        public long EstimatedTotal { get; set; }

        [JsonPropertyName("resources")]
        public List<RouteResource> Resources { get; set; }
    }

    public partial class RouteResource
    {
        [JsonPropertyName("__type")]
        public string Type { get; set; }

        [JsonPropertyName("bbox")]
        public List<double> Bbox { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("distanceUnit")]
        public string DistanceUnit { get; set; }

        [JsonPropertyName("durationUnit")]
        public string DurationUnit { get; set; }

        [JsonPropertyName("routeLegs")]
        public List<RouteLeg> RouteLegs { get; set; }

        [JsonPropertyName("trafficCongestion")]
        public string TrafficCongestion { get; set; }

        [JsonPropertyName("trafficDataUsed")]
        public string TrafficDataUsed { get; set; }

        [JsonPropertyName("travelDistance")]
        public double TravelDistance { get; set; }

        [JsonPropertyName("travelDuration")]
        public long TravelDuration { get; set; }

        [JsonPropertyName("travelDurationTraffic")]
        public long TravelDurationTraffic { get; set; }
    }

    public partial class RouteLeg
    {
        [JsonPropertyName("actualEnd")]
        public ActualEnd ActualEnd { get; set; }

        [JsonPropertyName("actualStart")]
        public ActualEnd ActualStart { get; set; }

        [JsonPropertyName("alternateVias")]
        public List<object> AlternateVias { get; set; }

        [JsonPropertyName("cost")]
        public long Cost { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("endLocation")]
        public EndLocation EndLocation { get; set; }

        [JsonPropertyName("itineraryItems")]
        public List<ItineraryItem> ItineraryItems { get; set; }

        [JsonPropertyName("routeRegion")]
        public string RouteRegion { get; set; }

        [JsonPropertyName("routeSubLegs")]
        public List<RouteSubLeg> RouteSubLegs { get; set; }

        [JsonPropertyName("startLocation")]
        public StartLocation StartLocation { get; set; }

        [JsonPropertyName("travelDistance")]
        public double TravelDistance { get; set; }

        [JsonPropertyName("travelDuration")]
        public long TravelDuration { get; set; }

        [JsonPropertyName("endTime")]
        public string EndTime { get; set; }

        [JsonPropertyName("startTime")]
        public string StartTime { get; set; }

    }

    public partial class ActualEnd
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("coordinates")]
        public List<double> Coordinates { get; set; }
    }

    public partial class EndLocation
    {
        [JsonPropertyName("bbox")]
        public List<double> Bbox { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("point")]
        public ActualEnd Point { get; set; }

        [JsonPropertyName("address")]
        public EndLocationAddress Address { get; set; }

        [JsonPropertyName("confidence")]
        public string Confidence { get; set; }

        [JsonPropertyName("entityType")]
        public string EntityType { get; set; }

        [JsonPropertyName("geocodePoints")]
        public List<GeocodePoint> GeocodePoints { get; set; }

        [JsonPropertyName("matchCodes")]
        public List<string> MatchCodes { get; set; }
    }

    public partial class EndLocationAddress
    {
        [JsonPropertyName("adminDistrict")]
        public string AdminDistrict { get; set; }

        [JsonPropertyName("adminDistrict2")]
        public string AdminDistrict2 { get; set; }

        [JsonPropertyName("countryRegion")]
        public string CountryRegion { get; set; }

        [JsonPropertyName("formattedAddress")]
        public string FormattedAddress { get; set; }

        [JsonPropertyName("locality")]
        public string Locality { get; set; }
    }

    public partial class ItineraryItem
    {
        [JsonPropertyName("compassDirection")]
        public string CompassDirection { get; set; }

        [JsonPropertyName("details")]
        public List<Detail> Details { get; set; }

        [JsonPropertyName("exit")]
        public string Exit { get; set; }

        [JsonPropertyName("iconType")]
        public string IconType { get; set; }

        [JsonPropertyName("instruction")]
        public Instruction Instruction { get; set; }

        [JsonPropertyName("isRealTimeTransit")]
        public bool IsRealTimeTransit { get; set; }

        [JsonPropertyName("maneuverPoint")]
        public ActualEnd ManeuverPoint { get; set; }

        [JsonPropertyName("realTimeTransitDelay")]
        public long RealTimeTransitDelay { get; set; }

        [JsonPropertyName("sideOfStreet")]
        public string SideOfStreet { get; set; }

        [JsonPropertyName("tollZone")]
        public string TollZone { get; set; }

        [JsonPropertyName("towardsRoadName"), NullValueHandling(NullHandling.Ignore)]
        public string TowardsRoadName { get; set; }

        [JsonPropertyName("transitTerminus")]
        public string TransitTerminus { get; set; }

        [JsonPropertyName("travelDistance")]
        public double TravelDistance { get; set; }

        [JsonPropertyName("travelDuration")]
        public long TravelDuration { get; set; }

        [JsonPropertyName("travelMode")]
        public string TravelMode { get; set; }

        [JsonPropertyName("signs"), NullValueHandling(NullHandling.Ignore)]
        public List<string> Signs { get; set; }

        [JsonPropertyName("warnings"), NullValueHandling(NullHandling.Ignore)]
        public List<Warning> Warnings { get; set; }

        [JsonPropertyName("hints"), NullValueHandling(NullHandling.Ignore)]
        public List<Hint> Hints { get; set; }

        [JsonPropertyName("childItineraryItems"), NullValueHandling(NullHandling.Ignore)]
        public List<ChildItineraryItem> ChildItineraryItems { get; set; }

        [JsonPropertyName("transitLine"), NullValueHandling(NullHandling.Ignore)]
        public TransitLine TransitLine { get; set; }

        [JsonPropertyName("wheelchairAccessible"), NullValueHandling(NullHandling.Ignore)]
        public bool? WheelchairAccessible { get; set; }

        [JsonPropertyName("bicyclesAllowed"), NullValueHandling(NullHandling.Ignore)]
        public bool? BicyclesAllowed { get; set; }
    }

    public partial class Detail
    {
        [JsonPropertyName("compassDegrees")]
        public long CompassDegrees { get; set; }

        [JsonPropertyName("endPathIndices")]
        public List<long> EndPathIndices { get; set; }

        [JsonPropertyName("maneuverType")]
        public string ManeuverType { get; set; }

        [JsonPropertyName("mode")]
        public string Mode { get; set; }

        [JsonPropertyName("names"), NullValueHandling(NullHandling.Ignore)]
        public List<string> Names { get; set; }

        [JsonPropertyName("roadType")]
        public string RoadType { get; set; }

        [JsonPropertyName("startPathIndices")]
        public List<long> StartPathIndices { get; set; }

        [JsonPropertyName("locationCodes"), NullValueHandling(NullHandling.Ignore)]
        public List<string> LocationCodes { get; set; }

        [JsonPropertyName("roadShieldRequestParameters"), NullValueHandling(NullHandling.Ignore)]
        public RoadShieldRequestParameters RoadShieldRequestParameters { get; set; }
    }

    public partial class RoadShieldRequestParameters
    {
        [JsonPropertyName("bucket")]
        public long Bucket { get; set; }

        [JsonPropertyName("shields")]
        public List<Shield> Shields { get; set; }
    }

    public partial class Shield
    {
        [JsonPropertyName("labels")]
        public List<string> Labels { get; set; }

        [JsonPropertyName("roadShieldType")]
        public long RoadShieldType { get; set; }
    }

    public partial class Hint
    {
        [JsonPropertyName("hintType")]
        public string HintType { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public partial class Instruction
    {
        [JsonPropertyName("formattedText")]
        public object FormattedText { get; set; }

        [JsonPropertyName("maneuverType")]
        public string ManeuverType { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public partial class Warning
    {
        [JsonPropertyName("origin"), NullValueHandling(NullHandling.Ignore)]
        public string Origin { get; set; }

        [JsonPropertyName("severity")]
        public string Severity { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("to"), NullValueHandling(NullHandling.Ignore)]
        public string To { get; set; }

        [JsonPropertyName("warningType")]
        public string WarningType { get; set; }
    }

    public partial class RouteSubLeg
    {
        [JsonPropertyName("endWaypoint")]
        public Waypoint EndWaypoint { get; set; }

        [JsonPropertyName("startWaypoint")]
        public Waypoint StartWaypoint { get; set; }

        [JsonPropertyName("travelDistance")]
        public double TravelDistance { get; set; }

        [JsonPropertyName("travelDuration")]
        public long TravelDuration { get; set; }
    }

    public partial class Waypoint
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("coordinates")]
        public List<double> Coordinates { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("isVia")]
        public bool IsVia { get; set; }

        [JsonPropertyName("locationIdentifier")]
        public string LocationIdentifier { get; set; }

        [JsonPropertyName("routePathIndex")]
        public long RoutePathIndex { get; set; }
    }

    public partial class StartLocation
    {
        [JsonPropertyName("bbox")]
        public List<double> Bbox { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("point")]
        public ActualEnd Point { get; set; }

        [JsonPropertyName("address")]
        public StartLocationAddress Address { get; set; }

        [JsonPropertyName("confidence")]
        public string Confidence { get; set; }

        [JsonPropertyName("entityType")]
        public string EntityType { get; set; }

        [JsonPropertyName("geocodePoints")]
        public List<GeocodePoint> GeocodePoints { get; set; }

        [JsonPropertyName("matchCodes")]
        public List<string> MatchCodes { get; set; }
    }

    public partial class StartLocationAddress
    {
        [JsonPropertyName("addressLine")]
        public string AddressLine { get; set; }

        [JsonPropertyName("adminDistrict")]
        public string AdminDistrict { get; set; }

        [JsonPropertyName("adminDistrict2")]
        public string AdminDistrict2 { get; set; }

        [JsonPropertyName("countryRegion")]
        public string CountryRegion { get; set; }

        [JsonPropertyName("formattedAddress")]
        public string FormattedAddress { get; set; }

        [JsonPropertyName("locality")]
        public string Locality { get; set; }

        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }
    }

    public partial class ChildItineraryItem
    {
        [JsonPropertyName("compassDirection")]
        public string CompassDirection { get; set; }

        [JsonPropertyName("details")]
        public List<ChildItineraryItemDetail> Details { get; set; }

        [JsonPropertyName("exit")]
        public string Exit { get; set; }

        [JsonPropertyName("iconType")]
        public string IconType { get; set; }

        [JsonPropertyName("instruction")]
        public Instruction Instruction { get; set; }

        [JsonPropertyName("isRealTimeTransit")]
        public bool IsRealTimeTransit { get; set; }

        [JsonPropertyName("maneuverPoint")]
        public ActualEnd ManeuverPoint { get; set; }

        [JsonPropertyName("realTimeTransitDelay")]
        public long RealTimeTransitDelay { get; set; }

        [JsonPropertyName("sideOfStreet")]
        public string SideOfStreet { get; set; }

        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("tollZone")]
        public string TollZone { get; set; }

        [JsonPropertyName("transitStopId")]
        public long TransitStopId { get; set; }

        [JsonPropertyName("transitTerminus")]
        public string TransitTerminus { get; set; }

        [JsonPropertyName("travelDistance")]
        public long TravelDistance { get; set; }

        [JsonPropertyName("travelDuration")]
        public long TravelDuration { get; set; }

        [JsonPropertyName("travelMode")]
        public string TravelMode { get; set; }

        [JsonPropertyName("tripId")]
        public long TripId { get; set; }
    }

    public partial class ChildItineraryItemDetail
    {
        [JsonPropertyName("maneuverType")]
        public string ManeuverType { get; set; }

        [JsonPropertyName("mode")]
        public string Mode { get; set; }

        [JsonPropertyName("names")]
        public List<string> Names { get; set; }

        [JsonPropertyName("roadType")]
        public string RoadType { get; set; }
    }

    public partial class ItineraryItemDetail
    {
        [JsonPropertyName("endPathIndices")]
        public List<long> EndPathIndices { get; set; }

        [JsonPropertyName("maneuverType")]
        public string ManeuverType { get; set; }

        [JsonPropertyName("mode")]
        public string Mode { get; set; }

        [JsonPropertyName("roadType")]
        public string RoadType { get; set; }

        [JsonPropertyName("startPathIndices")]
        public List<long> StartPathIndices { get; set; }
    }

    public partial class TransitLine
    {
        [JsonPropertyName("abbreviatedName")]
        public string AbbreviatedName { get; set; }

        [JsonPropertyName("agencyId")]
        public long AgencyId { get; set; }

        [JsonPropertyName("agencyName")]
        public string AgencyName { get; set; }

        [JsonPropertyName("lineColor")]
        public long LineColor { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("providerInfo")]
        public string ProviderInfo { get; set; }

        [JsonPropertyName("transportType")]
        public long TransportType { get; set; }

        [JsonPropertyName("uri")]
        public Uri Uri { get; set; }

        [JsonPropertyName("verboseName")]
        public string VerboseName { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }

    

}
