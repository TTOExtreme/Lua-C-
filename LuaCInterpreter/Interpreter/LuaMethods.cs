using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaCInterpreter.Variables;
using System.IO;

namespace LuaCInterpreter.Interpreter
{
    class LuaMethods
    {
        private ExternalMethods ExMeth;
        private Variables.Variables Vars;
        private Executor Ex;
        private LuaReferences Refer = new LuaReferences();

        public void init(ExternalMethods exMeth, Variables.Variables vars, Executor ex)
        {
            ExMeth = exMeth;
            Vars = vars;
            Ex = ex;

            //add the methods
            ExMeth.Add(Ls, "ls", new string[] { "String" });
            ExMeth.Add(Cd, "cd", new string[] { "String" });
            ExMeth.Add(Start, "start", new string[] { "String" });
        }

        #region LS
        private void Ls(string mess)
        {
            Ex.Execute("print(Current At:" + (Directory.GetCurrentDirectory() + "/Assets/" + Vars.Replace("CurrentLocation")).Replace(" ", "") + ");");//debug only
            Ex.Execute("print(Current At:" + Vars.Replace("CurrentLocation") + ");");
            foreach (string s in Directory.GetDirectories((Directory.GetCurrentDirectory() + "\\Assets" + Vars.Replace("CurrentLocation")).Replace(" ","")))
            {
                string str = (Directory.GetCurrentDirectory() + "\\Assets" + Vars.Replace("CurrentLocation")).Replace(" ", "");
                Ex.Execute("print(" + s.Replace(str.Substring(0,str.LastIndexOf("\\")+1), "") + "\\);");
            }
            foreach (string s in Directory.GetFiles((Directory.GetCurrentDirectory() + "\\Assets" + Vars.Replace("CurrentLocation")).Replace(" ", "")))
            {
                if (s.IndexOf(".ocult") ==-1) { Ex.Execute("print(" + s.Replace((Directory.GetCurrentDirectory() + "\\Assets" + Vars.Replace("CurrentLocation")).Replace(" ", ""), "") + ");"); } 
                //dont show files with .ocult extension
            }
        }
        #endregion

        #region CD
        private void Cd(string loc)
        {
            if (loc.Replace(" ","") == "..")
            {
                string at = Vars.Replace("CurrentLocation");
                at = at.Substring(0, at.LastIndexOf("\\"));
                at = at.Substring(0, at.LastIndexOf("\\") + 1);
                Vars.VarAdd(new VariablesStruct("CurrentLocation", "Global", at));
            }else
            {
                string at = Vars.Replace("CurrentLocation");
                if (Directory.Exists(at + "/" + loc))
                {
                    Vars.VarAdd(new VariablesStruct("CurrentLocation", "Global", at + "/" + loc));
                }
                else
                {
                    Ex.Execute("print(" + Refer.ErrorHead + Refer.ErrorNFF + ");");
                }
            }
        }
        #endregion

        #region Start
        private void Start(string filename)
        {
            string fileloc = (Directory.GetCurrentDirectory() + "\\Assets" + Vars.Replace("CurrentLocation")).Replace(" ", "") + filename +".lua";
            if (File.Exists(fileloc))
            {
                Ex.Execute(File.ReadAllLines(fileloc).ToList());
            }else
            {
                Ex.Execute("print(" + Refer.ErrorHead + Refer.ErrorNFF + ");");
            }
        }
        #endregion
    }
}
