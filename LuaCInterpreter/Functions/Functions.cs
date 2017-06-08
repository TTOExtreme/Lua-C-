using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaCInterpreter.Functions
{
    class Functions
    {
        private List<FunctionsStruct> Fs = new List<FunctionsStruct>();

        private Executor Ex;
        private ExternalMethods ExM;
        private Variables.Variables Vars;

        internal void Init(Executor ex, ExternalMethods exm, Variables.Variables vars)
        {
            Ex = ex;
            ExM = exm;
            Vars = vars;
        }

        public List<string> Get(string name)
        {
            foreach (FunctionsStruct f in Fs)
            {
                if (f.Name == name) { return f.Lines; }
            }
            return new List<string>();
        }
        public List<string> GetAll()
        {
            List<string> l = new List<string>();
            foreach (FunctionsStruct f in Fs)
            {
                l.Add(f.Name);
            }
            return l;
        }

        public bool Exist(string name)
        {
            foreach (FunctionsStruct f in Fs)
            {
                if (f.Name == name) { return true; }
            }
            return false;
        }

        public void Set(string name, List<string> Lines)
        {
            FunctionsStruct fu = new FunctionsStruct();
            fu.Name = name;
            fu.Lines = Lines;
            for (int i = 0; i < Fs.LongCount(); i++)
            {
                if (Fs[i].Name == name) { Fs[i] = fu; return; }
            }
            Fs.Add(fu);
        }

        public void Set(string name, List<string> Lines,bool Return)
        {
            FunctionsStruct fu = new FunctionsStruct();
            fu.Name = name;
            fu.Lines = Lines;
            fu.Return = Return;
            for (int i = 0; i < Fs.LongCount(); i++)
            {
                if (Fs[i].Name == name) { Fs[i] = fu; return; }
            }
            Fs.Add(fu);
        }
    }
}
