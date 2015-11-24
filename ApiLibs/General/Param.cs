using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs
{
    public class Param
    {
        public Param(String nm, String val)
        {
            Name = nm;
            Value = val;
        }

        public string Name { get; }

        public string Value { get; set; }

        public override string ToString()
        {
            return "{" + Name + ":" + Value + "}";
        }

    }
}
