using System;
using Newtonsoft.Json;

namespace ApiLibs.General
{
    public abstract class ObjectSearcher
    {
        [JsonIgnore]
        public Service service {get; set;}

        public void Search(Service inputService)
        {
            if (service != null)
            {
                return;
            }

            service = inputService;

            foreach (var item in GetType().GetProperties())
            {
                if (item.GetValue(this) == null)
                {
                    continue;
                }
                if (item.GetMethod.ReturnType.BaseType == typeof(ObjectSearcher))
                {
                    ObjectSearcher objectSearcher = item.GetValue(this) as ObjectSearcher;
                    objectSearcher.Search(inputService);
                }

                if (item.PropertyType.IsArray)
                {
                    Array a = (Array) item.GetValue(this);
                    foreach (object o in a)
                    {
                        if (o.GetType().BaseType == typeof(ObjectSearcher))
                        {
                            (o as ObjectSearcher)?.Search(inputService);
                        }
                    }
                }
            }
        }
    }

    public abstract class ObjectSearcher<T> where T:Service
    {
        [JsonIgnore]
        public T service { get; set; }

        public void Search(T inputService)
        {
            if (service != null)
            {
                return;
            }

            service = inputService;

            foreach (var item in GetType().GetProperties())
            {
                if (item.GetValue(this) == null)
                {
                    continue;
                }
                if (item.GetMethod.ReturnType.BaseType == typeof(ObjectSearcher<T>))
                {
                    ObjectSearcher<T> objectSearcher = item.GetValue(this) as ObjectSearcher<T>;
                    objectSearcher.Search(inputService);
                }

                if (item.PropertyType.IsArray)
                {
                    Array a = (Array)item.GetValue(this);
                    foreach (object o in a)
                    {
                        if (o.GetType().BaseType == typeof(ObjectSearcher<T>))
                        {
                            (o as ObjectSearcher<T>)?.Search(inputService);
                        }
                    }
                }
            }
        }
    }
}
