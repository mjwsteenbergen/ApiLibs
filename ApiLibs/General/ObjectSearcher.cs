using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.General
{
    public abstract class ObjectSearcher
    {
        public Service service;

        public void Search(Service inputService)
        {
            if (service != null)
            {
                return;
            }

            service = inputService;

            foreach (var item in GetType().GetProperties())
            {
                if (item.GetMethod.ReturnType.BaseType == typeof(ObjectSearcher))
                {
                    ObjectSearcher objectSearcher = item.GetValue(this) as ObjectSearcher;
                    objectSearcher.Search(inputService);
                }
            }
        }
    }
}
