using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.BingMaps
{
    public class BingMapsService : Service
    {
        public BingMapsService(string key) : base("https://dev.virtualearth.net/REST/v1/")
        {
            AddStandardParameter(new Param("key", key));
        }

        public async Task<LocationSearchResult> Location(string query)
        {
            return await MakeRequest<LocationSearchResult>("Locations", parameters: new List<Param>
            {
                new Param("query", query)
            });
        }

        public async Task<RouteResult> Route(List<string> waypoints, DateTime? time = null, TravelMode ? mode = TravelMode.Driving, TimeType? ttype = null)
        {
            return await MakeRequest<RouteResult>("Routes/" + mode.ToString(), parameters: new List<Param>
            {
                new OParam("dateTime", time?.ToString("G", CultureInfo.InvariantCulture)),
                new OParam("timeType", ttype?.ToString())
            }.Union(waypoints.Select((waypoint, index) => new Param($"wp.{index}", waypoint))).ToList());
        }

        public enum TravelMode { Driving, Walking, Transit }
        public enum TimeType { Arrival, Departure, LastAvailable }

        public async Task<RouteResult> TransitRoute(List<string> waypoints, DateTime? time = null, TimeType ttype = TimeType.Departure)
        {
            return await Route(waypoints, time ?? DateTime.Now, TravelMode.Transit, ttype);
        }
    }
}
