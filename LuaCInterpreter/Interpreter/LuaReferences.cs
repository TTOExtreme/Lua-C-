using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaCInterpreter
{
    class LuaReferences
    {
        public string If = "if";
        public string Then = "then";
        public string Else = "else";
        public string End = "end";

        public string While = "while";
        public string For = "for";
        public string Do = "do";

        public string Function = "function";
        public string FunctionEnd = "end";

        public string Comment = "--";

        public string Print = "print";
        public string Write = "write";
        public string Clear = "clear";

        public string Local = "local";
        public string Global = "Global";

        public string Components = "component";

        public string RootDir ="_root";

        public string[] Alpha = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "ç","₢" };
        public string[] Numeric = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public string[] Replaceble = { " ", ")", "(", "]", "[", "{", "}", "+", "-", "/", "#", "%", "!", "=", "*", ",", ".", ";", "&", "_" };
        //==================== ERRORS ====================\\
        public string ErrorHead = "[ERROR] ";
        public string ErrorNFC = "Command Not Found :( ";
        public string ErrorNFF = "File or Folder Not Found :( ";

        public string ErrorFor = " use For(<Variable>;<Initial Value>;<End Value>)";
    }
}
