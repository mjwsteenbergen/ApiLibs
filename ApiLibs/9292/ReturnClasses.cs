using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs._9292
{
    class ReturnClasses
    {
    }


    public class Status
    {
        public Daterange dateRange { get; set; }
        public string version { get; set; }
        public string servicesVersion { get; set; }
        public string plannerVersion { get; set; }
    }

    public class Daterange
    {
        public string from { get; set; }
        public string to { get; set; }
    }


    public class Rootobject
    {
        public Journey[] journeys { get; set; }
        public string earlier { get; set; }
        public string later { get; set; }
        public object[] disturbances { get; set; }
        public object[] serviceMessages { get; set; }
        public object[] nearbyAddresses { get; set; }
    }

    public class Journey
    {
        public string id { get; set; }
        public object[] ludMessages { get; set; }
        public object fasterJourneyId { get; set; }
        public string departure { get; set; }
        public string arrival { get; set; }
        public int numberOfChanges { get; set; }
        public Leg1[] legs { get; set; }
        public Fareinfo fareInfo { get; set; }
    }

    public class Fareinfo
    {
        public bool complete { get; set; }
        public int fullPriceCents { get; set; }
        public int reducedPriceCents { get; set; }
        public Leg[] legs { get; set; }
    }

    public class Leg
    {
        public From from { get; set; }
        public To to { get; set; }
        public string locationsString { get; set; }
        public string modeTypeString { get; set; }
        public string operatorString { get; set; }
        public Fare[] fares { get; set; }
    }

    public class Leg1
    {
        public string type { get; set; }
        public Mode mode { get; set; }
        public string destination { get; set; }
        public Operator _operator { get; set; }
        public string service { get; set; }
        public object[] attributes { get; set; }
        public object[] disturbancePlannerIds { get; set; }
        public object[] serviceMessageIds { get; set; }
        public Stop[] stops { get; set; }
    }

    public class From
    {
        public string id { get; set; }
        public string type { get; set; }
        public string stationId { get; set; }
        public string stationType { get; set; }
        public string name { get; set; }
        public Place place { get; set; }
        public Latlong latLong { get; set; }
        public Urls urls { get; set; }
    }

    public class Place
    {
        public string name { get; set; }
        public string regionCode { get; set; }
        public string regionName { get; set; }
        public bool showRegion { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public bool showCountry { get; set; }
    }

    public class Latlong
    {
        public float lat { get; set; }
        public float _long { get; set; }
    }

    public class Urls
    {
        public string nlNL { get; set; }
        public string enGB { get; set; }
    }

    public class To
    {
        public string id { get; set; }
        public string type { get; set; }
        public string stationId { get; set; }
        public string stationType { get; set; }
        public string name { get; set; }
        public Place place { get; set; }
        public Latlong latLong { get; set; }
        public Urls urls { get; set; }
    }


    public class Fare
    {
        public string _class { get; set; }
        public bool reduced { get; set; }
        public int eurocents { get; set; }
    }



    public class Mode
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Operator
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Stop
    {
        public string arrival { get; set; }
        public string departure { get; set; }
        public string platform { get; set; }
        public Location location { get; set; }
        public object fallbackName { get; set; }
    }

    public class Location
    {
        public string id { get; set; }
        public string type { get; set; }
        public string stationId { get; set; }
        public string stationType { get; set; }
        public string name { get; set; }
        public Place place { get; set; }
        public Latlong latLong { get; set; }
        public Urls urls { get; set; }
        public string regionCode { get; set; }
        public string regionName { get; set; }
        public bool showRegion { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public bool showCountry { get; set; }
        public string poiType { get; set; }
    }

    public class LocationRoot
    {
        public Location[] locations { get; set; }
    }
}
