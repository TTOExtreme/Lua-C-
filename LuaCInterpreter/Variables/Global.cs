using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace LuaCInterpreter.Variables
{
    class Global
    {
        public List<VariablesStruct> vars = new List<VariablesStruct>()
        {
        };

        public void Set()
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/_vars/")) { Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/_vars/"); }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/_vars/global.bin", JsonConvert.SerializeObject(vars));
        }
        public void Get()
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/_vars/")) { return; }
            vars = JsonConvert.DeserializeObject<List<VariablesStruct>>(File.ReadAllText(Directory.GetCurrentDirectory() + "/_vars/global.bin"));
        }
    }
    class Local
    {
        public List<VariablesStruct> vars = new List<VariablesStruct>()
        {
        };

        public void Set()
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/_vars/")) { Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/_vars/"); }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/_vars/local.bin", JsonConvert.SerializeObject(vars));
        }
        public void Get()
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/_vars/")) { return; }
            if(!File.Exists(Directory.GetCurrentDirectory() + "/_vars/local.bin")) { return; }
            vars = JsonConvert.DeserializeObject<List<VariablesStruct>>(File.ReadAllText(Directory.GetCurrentDirectory() + "/_vars/local.bin"));
        }
    }
}
