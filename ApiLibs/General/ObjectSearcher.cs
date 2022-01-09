using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiLibs.General
{
    public abstract class ObjectSearcher
    {
        [JsonIgnore]
        internal Service _service { get; set; }

        public void Search(Service inputService)
        {
            _service = inputService;
            var isObjSearch = (Type o) => o.BaseType == typeof(ObjectSearcher) || o.BaseType?.BaseType == typeof(ObjectSearcher);

            foreach (var item in GetType().GetProperties())
            {
                if (item.GetValue(this) == null)
                {
                    continue;
                }
                if (isObjSearch(item.GetMethod.ReturnType))
                {
                    ObjectSearcher objectSearcher = item.GetValue(this) as ObjectSearcher;
                    objectSearcher.Search(inputService);
                }

                if (item.PropertyType.IsArray)
                {
                    Array a = (Array) item.GetValue(this);
                    foreach (object o in a)
                    {
                        if (isObjSearch(o.GetType()))
                        {
                            (o as ObjectSearcher)?.Search(inputService);
                        }
                    }
                }
                if (item.GetMethod.ReturnType.IsGenericType && item.GetMethod.ReturnType.GetGenericTypeDefinition()
        == typeof(List<>))
                {
                    Type itemType = item.GetMethod.ReturnType.GetGenericArguments()[0];
                    if(isObjSearch(itemType))
                    {
                        foreach (var o in (IEnumerable) item.GetValue(this))
                        {
                            (o as ObjectSearcher)?.Search(inputService);
                        }
                    }
                }
            }
        }
    }

    public abstract class ObjectSearcher<T> : ObjectSearcher where T : Service
    {
        [JsonIgnore]
        public T Service { get {
                if (_service == null || _service is not T)
                {
                    return null;
                }
                return _service as T;
            } 
        }
    }
}
