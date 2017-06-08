using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaCInterpreter
{
    class IfStatement
    {
        private LuaReferences Refer = new LuaReferences();
        //private IfStatement IF = new IfStatement();
        private Executor Ex;
        private ForStatement FOR;
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
            //IF = i;
            FOR = f;
            WHILE = w;

            Cond.Init(Vars);
        }

        #region ************ If ************

        // recursive if func
        public List<string> IF(List<string> prog)
        {
            string openclose = "";
            if (Cond.IsTrue(prog[0]))
            {
                if (prog[0].IndexOf(Refer.Then) > -1 || prog[0].IndexOf(Refer.Else) > -1 || prog[0].IndexOf(Refer.Do) > -1) { openclose += "{"; }

                prog.RemoveAt(0);
                if (prog[0].IndexOf(Refer.Then) > -1 || prog[0].IndexOf(Refer.Else) > -1 || prog[0].IndexOf(Refer.Do) > -1) { openclose += "{"; }
                while (prog[0] != Refer.Else && (prog[0].IndexOf(Refer.If) == -1 || prog[0].IndexOf(Refer.While) == -1 || prog[0].IndexOf(Refer.For) == -1) && openclose != "")
                {
                    if (prog[0].IndexOf(Refer.End) > -1) { openclose += "}"; openclose = openclose.Replace("{}", ""); if (openclose == "") { break; } }
                    Ex.Execute(prog[0]);
                    prog.RemoveAt(0);
                    if (prog.LongCount() == 0) { return prog; }
                }
                if (prog[0].IndexOf(Refer.Else) > -1)
                {
                    while (prog[0].IndexOf(Refer.End) < 0 && prog[0].IndexOf(Refer.Then) < 0 && prog[0].IndexOf(Refer.Do) < 0)
                    {
                        prog.RemoveAt(0);
                        if (prog.LongCount() == 0) { return prog; }
                    }
                }
                if (prog[0].IndexOf(Refer.If) > -1 && prog[0].IndexOf(Refer.Then) > -1)
                {
                    prog = IF(prog);
                }
                if (prog[0].IndexOf(Refer.For) > -1 && prog[0].IndexOf(Refer.Do) > -1)
                {
                    prog = FOR.FOR(prog);
                }
                if (prog[0].IndexOf(Refer.While) > -1 && prog[0].IndexOf(Refer.Do) > -1)
                {
                    prog = WHILE.WHILE(prog);
                }
            }
            else
            {
                prog.RemoveAt(0);
                while (prog[0] != Refer.Else && prog[0].IndexOf(Refer.Then) < 0 && prog[0].IndexOf(Refer.Do) < 0 )
                {
                    prog.RemoveAt(0);
                    if (prog.LongCount() == 0) { return prog; }
                }
                if (prog[0].IndexOf(Refer.Else) > -1)
                {
                    while (prog[0].IndexOf(Refer.End) < 0 && prog[0].IndexOf(Refer.Then) < 0 && prog[0].IndexOf(Refer.Do) < 0)
                    {
                        Ex.Execute(prog[0]);
                        prog.RemoveAt(0);
                        if (prog.LongCount() == 0) { return prog; }
                    }
                }
                if (prog[0].IndexOf(Refer.If) > -1 && prog[0].IndexOf(Refer.Then) > -1)
                {
                    prog = IF(prog);
                }
                if (prog[0].IndexOf(Refer.For) > -1 && prog[0].IndexOf(Refer.Do) > -1)
                {
                    prog = FOR.FOR(prog);
                }
                if (prog[0].IndexOf(Refer.While) > -1 && prog[0].IndexOf(Refer.Do) > -1)
                {
                    prog = WHILE.WHILE(prog);
                }
            }
            return prog;

        }

        #endregion


    }
}
