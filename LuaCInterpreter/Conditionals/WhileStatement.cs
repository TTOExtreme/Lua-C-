using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaCInterpreter
{
    class WhileStatement
    {
        private Executor Ex;
        private LuaReferences Refer = new LuaReferences();
        private IfStatement IF;
        private ForStatement FOR;
        //private WhileStatement WHILE = new WhileStatement();
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
            FOR = f;
            //WHILE = w;

            Cond.Init(Vars);
        }

        public List<string> WHILE(List<string> Lines)
        {
            List<string> prog = new List<string>(Lines);

            string openclose = "";
            int count = 0;
            string[] args = Lines[0].Substring(Lines[0].IndexOf("(") + 1, Lines[0].LastIndexOf(")") - 1 - Lines[0].IndexOf("(")).Split(';');
            while (Cond.IsTrue("("+args[0]+")"))
            {
                //Task.Delay(10);
                if (prog[count].IndexOf(Refer.Do) > -1) { openclose += "{"; }

                //prog.RemoveAt(0);
                count++;
                if (prog[count].IndexOf(Refer.Do) > -1) { openclose += "{"; }
                List<string> _prog = new List<string>();
                foreach (string s in prog) { _prog.Add(s); }
                while ((prog[count].IndexOf(Refer.Then) == -1 && prog[count].IndexOf(Refer.Do) == -1) && openclose != "{")
                {
                    if (_prog[count].IndexOf(Refer.End) > -1) { _prog[count] = _prog[count].Replace(Refer.End, " "); openclose += "}"; openclose = openclose.Replace("{}", ""); if (openclose == "") { break; } }
                    Ex.Execute(_prog[count]);
                    _prog.RemoveAt(count);
                    if (_prog.LongCount() - count == 1) { break; }
                }


                if (prog[count].IndexOf(Refer.If) > -1 && prog[count].IndexOf(Refer.Then) > -1)
                {
                    List<string> pro = new List<string>();
                    for (int i = 0; count + i < prog.LongCount(); i++) { pro.Add(prog[i + count]); }

                    pro = IF.IF(pro);

                    for (int j = 0; j < pro.LongCount(); j++) { prog.RemoveAt(count); prog.Add(pro[j]); }
                }
                if (prog[count].IndexOf(Refer.For) > -1 && prog[count].IndexOf(Refer.Do) > -1)
                {
                    List<string> pro = new List<string>();
                    for (int i = 0; count + i < prog.LongCount(); i++) { pro.Add(prog[i + count]); }

                    pro = FOR.FOR(pro);

                    for (int j = 0; j < pro.LongCount(); j++) { prog.RemoveAt(count); prog.Add(pro[j]); }
                }
                if (prog[count].IndexOf(Refer.While) > -1 && prog[count].IndexOf(Refer.Do) > -1)
                {
                    List<string> pro = new List<string>();
                    for (int i = 0; count + i < prog.LongCount(); i++) { pro.Add(prog[i + count]); }

                    pro = WHILE(pro);

                    for (int j = 0; j < pro.LongCount(); j++) { prog.RemoveAt(count); prog.Add(pro[j]); }
                }
                count = 0;

            }
            for (; count > 0; count--)
            {
                prog.RemoveAt(0);
            }
            return prog;
        }
    }
}
