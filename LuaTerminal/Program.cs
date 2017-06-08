using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaCInterpreter;
using System.IO;

namespace LuaTerminal
{

    class Program
    {
        public Lua lua = new Lua();

        public static void Main(string[] args)
        {
            var instance = new Program();
            instance.LoadMethods();
            instance.lua.Init();
            while (true)
            {
                string s = Console.ReadLine();
                instance.lua.DoString(s);
            }
        }

        private void LoadMethods()
        {
            lua.SetPrintMethod(Print);
            lua.SetWriteMethod(Write);
            lua.SetClearMethod(Clear);
        }

        #region Methods
        #region Print
        private void Print(string mess)
        {
            Console.WriteLine(mess);
        }
        #endregion
        #region Write
        private void Write(string mess)
        {
            Console.Write(mess);
        }
        #endregion
        #region Clear
        private void Clear(string mess)
        {
            Console.Clear();
        }
        #endregion

        #endregion
    }
}
