using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaCInterpreter.Variables
{
    class VariablesStruct
    {
        public string Name = "";
        public string Type = "";
        public string Content = "Normal";
        public object Value;

        public VariablesStruct() { }

        public VariablesStruct(string name, string type, object value)
        {
            this.Name = name;
            this.Type = type;
            this.Value = value;
        }
    }
}
