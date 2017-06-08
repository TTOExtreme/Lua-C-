using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaCInterpreter
{
    class ExternalMethods
    {
        private List<ExtMethods> ExternMethods = new List<ExtMethods>();

        public void Add(Action<string> Method, string Name, string[] args)
        {
            ExtMethods m = new ExtMethods();
            m.Method = Method;
            m.Name = Name;
            m.args = args;
            ExternMethods.Add(m);
        }

        public bool Exist(string Name)
        {
            foreach (ExtMethods e in ExternMethods)
            {
                if (e.Name == Name) { return true; }
            }
            return false;
        }

        public bool Exist(string Name, string[] args)
        {
            foreach (ExtMethods e in ExternMethods)
            {
                if (e.Name == Name && e.args == args) { return true; }
            }
            return false;
        }

        public Action<string> Get(string Name)
        {
            foreach (ExtMethods e in ExternMethods) { if (e.Name == Name) { return e.Method; } } 
            return null;
        }

        public Action<string> Get(string Name, string[] args)
        {
            foreach (ExtMethods e in ExternMethods) { if (e.Name == Name) { return e.Method; } } 
            return null;
        }

        public List<string> GetMethods()
        {
            List<string> ret = new List<string>();
            foreach (ExtMethods e in ExternMethods) { ret.Add(e.Name); }
            return ret;
        }
        //
    }

    class ExtMethods
    {
        public Action<string> Method;
        public string Name;
        public string[] args;
    }
}
