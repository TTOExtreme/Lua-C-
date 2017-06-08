using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaCInterpreter.Variables;
using System.IO;

namespace LuaCInterpreter.Conditionals
{
    class Condition
    {
        private Variables.Variables Vars;
        
        public void Init(Variables.Variables vars)
        {
            Vars = vars;
        }

        public bool IsTrue(string prog)
        {
            if (prog.LongCount() < 1) { return false; }
            string str = Vars.Replace(prog);
            string[] args;

            #region +++++++++++++++++++++ "Exists" ++++++++++++++++++++++++
            if (str.IndexOf("exist") > -1)
            {
                str = Vars.Replace(str);

                args = str.Substring(str.IndexOf("(") + 1, (str.IndexOf(")") - (str.IndexOf("(") + 1))).Replace("exist", "#").Split('#');
                string comp1 = args[1];

                if (comp1.IndexOf(new LuaReferences().RootDir) > -1)
                {
                    string s = (Directory.GetCurrentDirectory() + "\\Assets").Replace(" ", "") + comp1.Replace(" ", "");
                    if (File.Exists((Directory.GetCurrentDirectory() + "\\Assets").Replace(" ", "") + comp1.Replace(" ", ""))) { return true; } else { return false; }
                }
                else
                {
                    if (File.Exists((Directory.GetCurrentDirectory() + "\\Assets" + Vars.Replace("CurrentLocation")).Replace(" ", "") + "\\" + comp1.Replace(" ", ""))) { return true; } else { return false; }
                }
            }
            #endregion

            #region +++++++++++++++++++++ "==" ++++++++++++++++++++++++
            if (str.IndexOf("==") > -1)
            {
                str = Vars.Replace(str);

                args = str.Substring(str.IndexOf("(") + 1, (str.IndexOf(")") - (str.IndexOf("(") + 1))).Replace("==", "#").Split('#');
                string comp1 = args[0];
                string comp2 = args[1];
                //bool isTrue = false;

                if (comp1 == comp2 && (comp1 != "" && comp2 != "")) { return true; } else { return false; }

            }
            #endregion

            #region +++++++++++++++++++++ "!=" ++++++++++++++++++++++++
            if (str.IndexOf("!=") > -1)
            {
                str = Vars.Replace(str);

                args = str.Substring(str.IndexOf("(") + 1, (str.IndexOf(")") - (str.IndexOf("(") + 1))).Replace("!=", "#").Split('#');
                string comp1 = args[0];
                string comp2 = args[1];
                bool isTrue = false;

                if (comp1 != comp2 && (comp1 != "" && comp2 != "")) { return true; } else { return false; }

            }
            #endregion

            #region +++++++++++++++++++++ ">" ++++++++++++++++++++++++
            if (str.IndexOf(">") > -1)
            {
                str = Vars.Replace(str);

                args = str.Substring(str.IndexOf("(") + 1, (str.IndexOf(")") - (str.IndexOf("(") + 1))).Replace(">", "#").Split('#');
                string comp1 = args[0];
                string comp2 = args[1];
                bool isTrue = false;

                if (Convert.ToDouble(comp1) > Convert.ToDouble(comp2) && (comp1 != "" && comp2 != "")) { return true; } else { return false; }

            }
            #endregion

            #region +++++++++++++++++++++ "<" ++++++++++++++++++++++++
            if (str.IndexOf("<") > -1)
            {
                str = Vars.Replace(str);

                args = str.Substring(str.IndexOf("(") + 1, (str.IndexOf(")") - (str.IndexOf("(") + 1))).Replace("<", "#").Split('#');
                string comp1 = args[0];
                string comp2 = args[1];
                bool isTrue = false;

                if (Convert.ToDouble(comp1) < Convert.ToDouble(comp2) && (comp1 != "" && comp2 != "")) { return true; } else { return false; }

            }
            #endregion

            #region +++++++++++++++++++++ "<=" ++++++++++++++++++++++++
            if (str.IndexOf("<=") > -1)
            {
                str = Vars.Replace(str);

                args = str.Substring(str.IndexOf("(") + 1, (str.IndexOf(")") - (str.IndexOf("(") + 1))).Replace("<=", "#").Split('#');
                string comp1 = args[0];
                string comp2 = args[1];
                bool isTrue = false;

                if (Convert.ToDouble(comp1) <= Convert.ToDouble(comp2) && (comp1 != "" && comp2 != "")) { return true; } else { return false; }

            }
            #endregion

            #region +++++++++++++++++++++ ">=" ++++++++++++++++++++++++
            if (str.IndexOf(">=") > -1)
            {
                str = Vars.Replace(str);

                args = str.Substring(str.IndexOf("(") + 1, (str.IndexOf(")") - (str.IndexOf("(") + 1))).Replace(">=", "#").Split('#');
                string comp1 = args[0];
                string comp2 = args[1];
                bool isTrue = false;

                //Console.WriteLine("c1: " + comp1 + " c2: " + comp2);

                if (Convert.ToDouble(comp1) >= Convert.ToDouble(comp2) && (comp1 != "" && comp2 != ""))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            #endregion


            #region +++++++++++++++++++++ "direct bool" ++++++++++++++++++++++++
            if (str.IndexOf("true") > -1)
            {
                return true;
            }
            #endregion

            return false;
        }
    }
}
