using Newtonsoft.Json;

namespace ApiLibs
{
    public class Param
    {
        public Param(string nm, string val)
        {
            Name = nm;
            Value = val;
        }

        public Param(string nm, object val)
        {
            Name = nm;
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            Value = JsonConvert.SerializeObject(val, settings);
        }

        public string Name { get; }

        public string Value { get; set; }

        public override string ToString()
        {
            return "{" + Name + ":" + Value + "}";
        }

    }
}
