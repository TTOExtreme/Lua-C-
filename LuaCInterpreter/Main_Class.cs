using LuaCInterpreter.Interpreter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaCInterpreter
{
    public class Lua
    {
        private Executor Ex = new Executor();
        private ExternalMethods ExMeth = new ExternalMethods();
        private Variables.Variables Vars = new Variables.Variables();
        private LuaReferences Refer = new LuaReferences();
        private LuaMethods LMeth = new LuaMethods();

        public void Init()
        {
            Ex.Init(ExMeth,Vars);
            LMeth.init(ExMeth,Vars,Ex);

            DoFile(File.ReadAllLines(Directory.GetCurrentDirectory() + "/Assets/_root/Bios.os").ToList());
        }

        //do command from string
        public void DoString(string line)
        {
            Ex.TermCall(line);
        }

        //execute an file
        public void DoFile(List<string> lines)
        {
            Ex.Execute(lines);
        }

        //add methods to lua interpreter
        public void AddMethod(Action<string> Method, string Name)
        {
            ExMeth.Add(Method, Name, new string[] { "string" });
        }

        public void AddMethod(Action<string> Method, string Name,string[] args)
        {
            ExMeth.Add(Method, Name, args);
        }

        //set the method to print in screen
        public void SetPrintMethod(Action<string> Method)
        {
            ExMeth.Add(Method, Refer.Print, new string[] { "string" });
        }

        //set the method to write in screen
        public void SetWriteMethod(Action<string> Method)
        {
            ExMeth.Add(Method, Refer.Write, new string[] { "string" });
        }

        //set the method to write in screen
        public void SetClearMethod(Action<string> Method)
        {
            ExMeth.Add(Method, Refer.Clear, new string[] { "string" });
        }

        //getValues from vars
        public string GetVar(string name)
        {
            name = Vars.Replace(name);
            return name;
        }

    }
}
