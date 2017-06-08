using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaCInterpreter
{
    class ForStatement
    {
        private Executor Ex;
        private LuaReferences Refer = new LuaReferences();
        private IfStatement IF;
        //private ForStatement FOR;
        private WhileStatement WHILE;
        private Conditionals.Condition Cond = new Conditionals.Condition();

        private ExternalMethods ExM;
        private Variables.Variables Vars;

        internal void Init(Executor ex, ExternalMethods exm, Variables.Variables vars)
        {
            Ex = ex;
            ExM = exm;
            Vars = vars;
        }

        internal void Init1(IfStatement i, ForStatement f, WhileStatement w)
        {
            IF = i;
            //FOR = f;
            WHILE = w;

            Cond.Init(Vars);
        }

        public List<string> FOR(List<string> Lines)
        {
            List<string> _prog = new List<string>();

            string openclose = "";
            int count = 0;
            while (!(Lines[0].IndexOf(Refer.For) > -1) && (Lines[0].IndexOf(Refer.Do) > -1) && Lines.LongCount() > 0) { Lines.RemoveAt(0); }
            string[] args = Lines[0].Substring(Lines[0].IndexOf("(") + 1, Lines[0].LastIndexOf(")") - 1 - Lines[0].IndexOf("(")).Split(';');
            if ((int)(args.LongCount()) != 3) { ExM.Get(Refer.Print).DynamicInvoke(Refer.ErrorHead+Refer.ErrorFor + "" + Refer.Do); return null; }
            //prog.RemoveAt(0);
            if (Vars.Replace(args[0]) == args[0])
            {
                Variables.VariablesStruct v = new Variables.VariablesStruct();
                v.Type = "Local";
                v.Name = args[0];
                v.Value = args[1];
                Vars.VarAdd(v);
            }

            for (; Cond.IsTrue("(" + args[0].Replace(" ", "") + " != " + args[2].Replace(" ", "") + ")");)
            {
                Task.Delay(10);
                if (Lines[count].IndexOf(Refer.End) > -1) { openclose += "{"; }

                //prog.RemoveAt(0);
                count++;
                if (Lines[count].IndexOf(Refer.Do) > -1 && Lines[count].IndexOf(Refer.Then) > -1) { openclose += "{"; }
                _prog = new List<string>();
                foreach (string s in Lines) { _prog.Add(s); }
                while ((_prog[count].IndexOf(Refer.Then) == -1 && _prog[count].IndexOf(Refer.Do) == -1) && openclose != "{")
                {
                    if (_prog[count].IndexOf(Refer.End) > -1) { _prog[count] = _prog[count].Replace(Refer.End, " "); openclose += "}"; openclose = openclose.Replace("{}", ""); if (openclose == "") { break; } }
                    _prog[count] = _prog[count].Replace(Refer.Else, "").Replace(Refer.If, "").Replace(Refer.For, "").Replace(Refer.While, "").Replace(Refer.Then, "").Replace(Refer.Do, "");

                    if (_prog[0].IndexOf(Refer.Return) > -1) { return new List<string> { _prog[0] }; }
                    Ex.Execute(_prog[count]);
                    count++;
                    //_prog.RemoveAt(count);
                    if (_prog.LongCount() - count == 1)// 1 for the "end" line
                    {
                        //Vars.VarRem(args[0]);
                        //count = 0;
                        break;
                    }
                }


                if (_prog[count].IndexOf(Refer.If) > -1 && _prog[count].IndexOf(Refer.Then) > -1)
                {
                    List<string> pro = new List<string>();
                    for (int i = 0; count + i < _prog.LongCount(); i++) { pro.Add(_prog[i + count]); }

                    pro = IF.IF(pro);
                    if (pro[0].IndexOf(Refer.Return) > -1) { return new List<string> { pro[0] }; }

                    for (int j = 0; j < pro.LongCount(); j++) { count++; }//prog.RemoveAt(count); prog.Add(pro[j]);
                }
                if (_prog[count].IndexOf(Refer.For) > -1 && _prog[count].IndexOf(Refer.Do) > -1)
                {
                    List<string> pro = new List<string>();
                    for (int i = 0; count + i < _prog.LongCount(); i++) { pro.Add(Lines[i + count]); }

                    pro = FOR(pro);
                    if (pro[0].IndexOf(Refer.Return) > -1) { return new List<string> { pro[0] }; }

                    for (int j = 0; j < pro.LongCount(); j++) { count++; }
                }
                if (_prog[count].IndexOf(Refer.While) > -1 && _prog[count].IndexOf(Refer.Do) > -1)
                {
                    List<string> pro = new List<string>();
                    for (int i = 0; count + i < _prog.LongCount(); i++) { pro.Add(Lines[i + count]); }

                    pro = WHILE.WHILE(pro);
                    if (pro[0].IndexOf(Refer.Return) > -1) { return new List<string> { pro[0] }; }

                    for (int j = 0; j < pro.LongCount(); j++) { count++; }
                }
                count = 0;
                //Ex.Execute(_prog[count]);
                if (Convert.ToInt64(args[1]) < Convert.ToInt64(args[2])) { Ex.Execute(args[0] + "++;"); } else { if (Convert.ToInt64(args[1]) > Convert.ToInt64(args[2])) { Ex.Execute(args[0] + "--;"); } }

            }
            if (_prog.LongCount() > 0) { _prog.RemoveAt(0); }
            Vars.VarRem(args[0]);
            return _prog;
        }
    }
}
