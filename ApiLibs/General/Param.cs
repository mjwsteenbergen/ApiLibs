using Newtonsoft.Json;

namespace ApiLibs
{
    public class Param
    {
        public Param(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public Param(string name, object value)
        {
            Name = name;
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            Value = JsonConvert.SerializeObject(value, settings);
        }

        public string Name { get; }

        public string Value { get; set; }

        public override string ToString()
        {
            return "{" + Name + ":" + Value + "}";
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class OParam : Param {
        public OParam(string name, string value) : base(name, value)
        {
        }

        public OParam(string name, object value) : base(name, value)
        {
        }
    }
}
