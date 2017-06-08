using LuaCInterpreter.Variables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaCInterpreter
{
    class Executor
    {
        private LuaReferences Refer = new LuaReferences();
        private IfStatement IF = new IfStatement();
        private ForStatement FOR = new ForStatement();
        private WhileStatement WHILE = new WhileStatement();
        private ExternalMethods Ex;
        private Variables.Variables Vars;
        private Functions.Functions Func = new Functions.Functions();
        
        internal void Init(ExternalMethods Exm, Variables.Variables vars)
        {
            Ex = Exm;

            Ex.Add(Start, "start", new string[] { "string" });

            Vars = vars;

            IF.Init(this, Exm, Vars);
            FOR.Init(this, Exm, Vars);
            WHILE.Init(this, Exm, Vars);

            IF.Init1(IF, FOR, WHILE);
            FOR.Init1(IF, FOR, WHILE);
            WHILE.Init1(IF, FOR, WHILE);

        }

        public void TermCall(string Line)
        {

            foreach (string f in Directory.GetFiles((Directory.GetCurrentDirectory() + "\\Assets" + Vars.Replace("CurrentLocation")).Replace(" ", "")))
            {
                string c = f.Replace((Directory.GetCurrentDirectory() + "\\Assets" + Vars.Replace("CurrentLocation")).Replace(" ", ""), "");
                if (Line == c) {
                    Execute(File.ReadAllLines(f).ToList());
                }
            }
            foreach (string met in Ex.GetMethods())
            {
                if (Line.IndexOf(met) > -1)
                {
                    Action<string> DO = Ex.Get(met);
                    try
                    {
                        Line = Vars.Replace(Line);
                        if (Line.IndexOf("(") > -1 && Line.IndexOf(")") > -1)
                        {
                            DO.DynamicInvoke(Line.Substring(Line.IndexOf("(") + 1, Line.Substring(Line.IndexOf("(") + 1).LastIndexOf(")")).Split(','));
                        }
                        else
                        {
                            DO.DynamicInvoke(Line.Replace(met, "").Replace(";", ""));
                        }
                        return;
                    }
                    catch (Exception e)
                    {
                        Ex.Get(Refer.Print).DynamicInvoke(Refer.ErrorHead + e.ToString());
                        return;
                    }
                }
            }
        }

        public void Execute(string Line)
        {
            if (Line.IndexOf(Refer.Comment) > -1) { Line = Line.Substring(0, Line.IndexOf(Refer.Comment)); }
            if (Line.IndexOf(Refer.Function) > -1) { return; }
            Line =Line.Replace(Refer.Else, "").Replace(Refer.If, "").Replace(Refer.For, "").Replace(Refer.While, "").Replace(Refer.Then, "").Replace(Refer.Do, "").Replace(Refer.End,"");
            if (Line == "") { return; }
            /*//load file
            foreach(string f in Directory.GetFiles((Directory.GetCurrentDirectory() + "\\Assets" + Vars.Replace("CurrentLocation")).Replace(" ", "")))
            {
                if (Line + ".lua" == f) { Execute(File.ReadAllLines((Directory.GetCurrentDirectory() + "\\Assets" + Vars.Replace("CurrentLocation")).Replace(" ", "") + "\\" + f).ToList()); }
            }
            //end */

            //Components
            string lineaux = Vars.Replace(Line);
            if (lineaux.IndexOf(Refer.Components) > -1)
            {

            }

            if (Line.IndexOf("++;") > -1)
            {
                Vars.VarSum(Line.Replace("++;", "").Replace(" ", "").Replace("\t",""), 1);
                return;
            }
            if (Line.IndexOf("--;") > -1)
            {
                Vars.VarSub(Line.Replace("--;", "").Replace(" ", ""), 1);
                return;
            }

            foreach (string met in Ex.GetMethods())
            {
                if (Line.IndexOf(met) > -1)
                {
                    Action<string> DO = Ex.Get(met);
                    try
                    {
                        Line = Vars.Replace(Line);
                        if (Line.IndexOf("(") > -1 && Line.IndexOf(")") > -1)
                        {
                            DO.DynamicInvoke(Line.Substring(Line.IndexOf("(") + 1, Line.Substring(Line.IndexOf("(") + 1).LastIndexOf(")")).Split(','));
                        }else
                        {
                            //DO.DynamicInvoke(Line.Replace(met,"").Replace(";",""));
                        }
                        return;
                    }
                    catch(Exception e)
                    {
                        Ex.Get(Refer.Print).DynamicInvoke(Refer.ErrorHead + e.ToString());
                        return;
                    }
                }
            }
            if(Line.IndexOf("(")>-1 && Line.IndexOf(")") > -1)
            {
                string rf = Line;
                if (Line.IndexOf("=") > -1) { rf = Line.Substring(Line.IndexOf("=") + 1); }
                string arg = Line.Substring(Line.IndexOf("("));
                arg.Substring(0, arg.IndexOf(")"));
                string[] args = arg.Substring(arg.IndexOf("(") + 1, arg.LastIndexOf(")") - 1 - arg.IndexOf("(")).Split(';');
                bool brea = false;
                foreach (string s in Func.GetAll())
                {
                    if (rf.IndexOf(s) > -1)
                    {
                        foreach (string s1 in Refer.Replaceble)
                        {
                            foreach (string s2 in Refer.Replaceble)
                            {
                                if (rf.IndexOf(s2 + s + s1 + arg) > -1) { rf = rf.Replace(s2 + s + s1 + arg, s2 + ExecuteFunction(Func.Get(s),args.ToList()) + s1); brea = true; break; }
                                if (rf.IndexOf(s1 + s + s2 + arg) > -1) { rf = rf.Replace(s1 + s + s2 + arg, s1 + ExecuteFunction(Func.Get(s), args.ToList()) + s2); brea = true; break; }
                                if (rf == s + arg) { rf = rf.Replace(s + arg, ExecuteFunction(Func.Get(s), args.ToList())); brea = true; break; }

                                if (rf.IndexOf(s2 + s + s1) > -1) { rf = rf.Replace(s2 + s + s1, s2 + ExecuteFunction(Func.Get(s), args.ToList()) + s1); brea = true; break; }
                                if (rf.IndexOf(s1 + s + s2) > -1) { rf = rf.Replace(s1 + s + s2, s1 + ExecuteFunction(Func.Get(s), args.ToList()) + s2); brea = true; break; }
                                if (rf == s) { rf = rf.Replace(s, ExecuteFunction(Func.Get(s), args.ToList())); brea = true; break; }
                            }
                            if (brea) { break; }
                        }
                    }
                    if (brea) { break; }
                }
                if (Line.IndexOf("=") > -1)
                {
                    Line = Line.Substring(0,Line.IndexOf("=")+1)+rf.Replace("\t", "").Replace(" ", "");
                }
                else
                {
                    Line = rf;
                }
            }

            if (!LineInter(Line))
            {
                if (Line.Replace("\t","") == "") { return; }
                Ex.Get(Refer.Print).DynamicInvoke(Refer.ErrorHead + Refer.ErrorNFC + " {" + Line + "}");
            }
        }

        #region Execute File
        public void Execute(List<string> _Lines)
        {
            //compile
            List<string> Lines = new List<string>();
            foreach (string s in _Lines) { Lines.Add(s); }
            for (int line = 0; line < Lines.LongCount(); line++)
            {
                if (Lines[line].IndexOf(Refer.Function) > -1)
                {
                    string[] args = Lines[line].Substring(Lines[line].IndexOf("(") + 1, Lines[line].LastIndexOf(")") - 1 - Lines[line].IndexOf("(")).Split(',');
                    string name = Lines[line].Substring(0, Lines[line].IndexOf("(")).Replace(Refer.Function, "").Replace(" ", "");
                    bool ret = false;
                    List<string> l = new List<string>();
                    string openclose = "";
                    while (Lines[line].IndexOf(Refer.FunctionEnd) < 0 || openclose != "")
                    {
                        if (Lines[line].IndexOf(Refer.End) > -1) { Lines[line] = Lines[line].Replace(Refer.End, " "); openclose += "}"; openclose = openclose.Replace("{}", ""); if (openclose == "") { break; } }
                        if (Lines[line].IndexOf(Refer.Return) > -1) { ret = true; }
                        l.Add(Lines[line]);
                        Lines.RemoveAt(line);
                    }
                    //l.Add(Lines[line]);//add the end line
                    Lines.RemoveAt(line);
                    line--;
                    Func.Set(name, l);
                }
            }
            //end */
            //Lines = new List<string>();
            //foreach(string s in _Lines) { Lines.Add(s); }
            for (int line = 0; line < Lines.LongCount(); line++)
            {
                if (Lines[line].IndexOf(Refer.If) > -1 && Lines[line].IndexOf(Refer.Then) > -1)
                {
                    List<string> _prog = new List<string>();
                    for (int i = line; i < Lines.LongCount(); i++) { _prog.Add(Lines[i]); }
                    Lines = IF.IF(_prog);
                }
                else
                {
                    if (Lines[line].IndexOf(Refer.For) > -1 && Lines[line].IndexOf(Refer.Do) > -1)
                    {
                        List<string> _prog = new List<string>();
                        for (int i = line; i < Lines.LongCount(); i++) { _prog.Add(Lines[i]); }
                        Lines = FOR.FOR(_prog);
                    }
                    else
                    {
                        if (Lines[line].IndexOf(Refer.While) > -1 && Lines[line].IndexOf(Refer.Do) > -1)
                        {
                            List<string> _prog = new List<string>();
                            for (int i = line; i < Lines.LongCount(); i++) { _prog.Add(Lines[i]); }
                            Lines = WHILE.WHILE(_prog);
                        }
                        else
                        {
                            Execute(Lines[line]);
                        }
                    }
                }
            }
        }
        #endregion

        #region Execute function
        public string ExecuteFunction(List<string> _Lines,List<string> Args)
        {
            string[] args = _Lines[0].Substring(_Lines[0].IndexOf("(") + 1, _Lines[0].LastIndexOf(")") - 1 - _Lines[0].IndexOf("(")).Split(';');
            if(args.LongCount()!= Args.LongCount()) { Ex.Get(Refer.Print).DynamicInvoke(Refer.ErrorHead +Refer.ErrorWA+args.LongCount()+"\n"); return ""; }

            for (int i=0;i<args.LongCount();i++)
            {
                Execute(args[i] + " = " + Args[i] + ";");
            }

            List<string> Lines = new List<string>();
            foreach(string s in _Lines) { Lines.Add(s); }
            for (int line = 0; line < Lines.LongCount(); line++)
            {
                if (Lines[line].IndexOf(Refer.If) > -1 && Lines[line].IndexOf(Refer.Then) > -1)
                {
                    List<string> _prog = new List<string>();
                    for (int i = line; i < Lines.LongCount(); i++) { _prog.Add(Lines[i]); }
                    Lines = IF.IF(_prog);
                    if (Lines[line].IndexOf(Refer.Return) > -1) { return Vars.Replace(Lines[line]).Replace(Refer.Return, "").Replace(";", ""); }
                }
                else
                {
                    if (Lines[line].IndexOf(Refer.For) > -1 && Lines[line].IndexOf(Refer.Do) > -1)
                    {
                        List<string> _prog = new List<string>();
                        for (int i = line; i < Lines.LongCount(); i++) { _prog.Add(Lines[i]); }
                        Lines = FOR.FOR(_prog);
                        if (Lines[line].IndexOf(Refer.Return) > -1) { return Vars.Replace(Lines[line]).Replace(Refer.Return, "").Replace(";", ""); }
                    }
                    else
                    {
                        if (Lines[line].IndexOf(Refer.While) > -1 && Lines[line].IndexOf(Refer.Do) > -1)
                        {
                            List<string> _prog = new List<string>();
                            for (int i = line; i < Lines.LongCount(); i++) { _prog.Add(Lines[i]); }
                            Lines = WHILE.WHILE(_prog);
                            if (Lines[line].IndexOf(Refer.Return) > -1) { return Vars.Replace(Lines[line]).Replace(Refer.Return, "").Replace(";", ""); }
                        }
                        else
                        {
                            if (Lines[line].IndexOf(Refer.Return) > -1) { return Vars.Replace(Lines[line]).Replace(Refer.Return, "").Replace(";", ""); }
                            Execute(Lines[line]);
                        }
                    }
                }
            }
            return "";
        }
        #endregion

        #region line interpreter
        private bool LineInter(string Line)
        {
            #region set Local var
            if (Line.IndexOf(Refer.Local) > -1 && Line.IndexOf("=") > -1)
            {
                Variables.VariablesStruct vs = new Variables.VariablesStruct();
                vs.Name = Line.Substring(Line.Substring(Line.IndexOf(Refer.Local)).IndexOf(" "), Line.IndexOf("=") - (Line.Substring(Line.IndexOf(Refer.Local)).IndexOf(" "))).Replace(" ", "");
                Line = Vars.Replace(Line);
                vs.Value = Line.Substring(Line.IndexOf("=") + 1).Replace(";", "");
                vs.Type = "Local";
                if (isNumeric((string)vs.Value))
                {
                    try
                    {
                        vs.Value = Evaluate((string)vs.Value);
                    }
                    catch (Exception e)
                    {
                        Ex.Get(Refer.Print).DynamicInvoke(Refer.ErrorHead + e.ToString());
                    }
                }
                Vars.VarAdd(vs);
                return true;
            }
            #endregion
            #region set Global var
            if (Line.IndexOf(Refer.Global) > -1 && Line.IndexOf("=") > -1)
            {
                Variables.VariablesStruct vs = new Variables.VariablesStruct();
                vs.Name = Line.Substring(Line.Substring(Line.IndexOf(Refer.Global)).IndexOf(" "), Line.IndexOf("=") - (Line.Substring(Line.IndexOf(Refer.Global)).IndexOf(" "))).Replace(" ", "");
                Line = Vars.Replace(Line);
                vs.Value = Line.Substring(Line.IndexOf("=") + 1).Replace(";", "");
                vs.Type = "Global";
                if (isNumeric((string)vs.Value))
                {
                    try
                    {
                        vs.Value = Evaluate((string)vs.Value);
                    }
                    catch (Exception e)
                    {
                        Ex.Get(Refer.Print).DynamicInvoke(Refer.ErrorHead + e.ToString());
                    }
                }
                Vars.VarAdd(vs);
                return true;
            }
            #endregion
            #region set var
            if (Line.IndexOf("=") > -1)
            {
                Variables.VariablesStruct vs = new Variables.VariablesStruct();
                vs.Name = Line.Substring(0, Line.IndexOf("=")).Replace(" ", "");
                Line = Vars.Replace(Line);
                vs.Value = Line.Substring(Line.IndexOf("=") + 1).Replace(";", "");
                vs.Type = "Local";
                if (isNumeric((string)vs.Value))
                {
                    try
                    {
                        vs.Value = Evaluate((string)vs.Value);
                    }
                    catch (Exception e)
                    {
                        Ex.Get(Refer.Print).DynamicInvoke(Refer.ErrorHead + e.ToString());
                    }
                }
                Vars.VarAdd(vs);
                return true;
            }
            #endregion
            return false;
        }
        #endregion

        #region Aux Func

        #region Type of var
        private bool isNumeric(string Line)
        {
            foreach(string s in Refer.Alpha)
            {
                if (Line.IndexOf(s) > -1) { return false; }
            }
            return true;
        }
        #endregion

        #region ************ Evaluate math ************

        // do math functions
        private static double Evaluate(string expression)
        {
            return (double)new System.Xml.XPath.XPathDocument
            (new StringReader("<r/>")).CreateNavigator().Evaluate
            (string.Format("number({0})", new
            System.Text.RegularExpressions.Regex(@"([\+\-\*])")
            .Replace(expression, " ${1} ")
            .Replace("/", " div ")
            .Replace("%", " mod ")));
        }

        #endregion

        #endregion

        #region SubRotines
        private void Start(string path)
        {
            try
            {
                if (path.IndexOf(Refer.RootDir) > -1)
                {
                    List<string> s = File.ReadAllLines((Directory.GetCurrentDirectory() + "\\Assets").Replace(" ", "") + "\\" + path.Replace(" ", "")).ToList();
                    Execute(s);
                }
                else
                {
                    Execute(File.ReadAllLines((Directory.GetCurrentDirectory() + "\\Assets" + Vars.Replace("CurrentLocation")).Replace(" ", "") + "\\" + path.Replace(" ", "")).ToList());
                }
            }
            catch (Exception e)
            {
                Ex.Get(Refer.Print).DynamicInvoke(Refer.ErrorHead + e.ToString());
            }
        }
        #endregion
    }
}
