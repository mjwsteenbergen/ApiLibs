using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs._9292
{
    public class _9292Service : Service
    {
        public _9292Service()
        { 
            SetUp("https://api.9292.nl/0.1/");
            AddStandardParameter("lang", "nl-NL");
        }

        public async Task<Status> GetStatus()
        {
            return await MakeRequest<Status>("status");
        }

        public async Task GetJourney(string from, string to, int before = 0, int after = 0, int sequence = 1, bool byBus = false, bool byFerry = false, bool bySubway = false, bool byTram = false, bool byTrain = false, DateTime time = default(DateTime), SearchType searchType = SearchType.departure)
        {
            List<Param> parameters = new List<Param>
            {
                new Param("before", before.ToString()),
                new Param("after", after.ToString()),
                new Param("sequence", sequence.ToString()),
                new Param("from", from),
                new Param("to", to)
            };
            if (byBus)
            {
                parameters.Add(new Param("byBus", "true"));
            }
            if (byFerry)
            {
                parameters.Add(new Param("byFerry","true"));
            }
            if (bySubway)
            {
                parameters.Add(new Param("bySubway", "true"));
            }
            if (byTram)
            {
                parameters.Add(new Param("byTram", "true"));
            }
            if (byTrain)
            {
                parameters.Add(new Param("byTrain", "true"));
            }
            if (time == default(DateTime))
            {
                time = DateTime.Now;
            }
            if (searchType == SearchType.arrival)
            {
                parameters.Add(new Param("searchType", SearchType.arrival.ToString()));
            }
            string timeString = time.ToString("s");
            string minuteTimeString = timeString.Remove(timeString.Length - 3);
            parameters.Add(new Param("dateTime", minuteTimeString.Replace(":", "")));
            await HandleRequest("journeys", parameters: parameters);
        }

        public async Task<List<Location>>  GetLocation(string searchQuery)
        {
            return (await MakeRequest<LocationRoot>("locations", parameters: new List<Param> {new Param("q", searchQuery)})).locations.ToList();
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum SearchType
    {  
        departure, arrival
    }
}
