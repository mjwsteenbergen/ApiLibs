using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs
{
    public class Param
    {
        private String _name;
        private String _value;

        public Param(String nm, String val)
        {
            _name = nm;
            _value = val;
        }

        public String name
        {
            get { return _name; }
        }

        public String value
        {
            get { return _value; }
            set { _value = value; }
        }

        public override string ToString()
        {
            return "{" + _name + ":" + _value + "}";
        }

    }
}
