using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaCInterpreter.Variables
{
    class Variables
    {
        private Global GV = new Global();
        private Local LV = new Local();

        #region Replace
        public string Replace(string command)
        {
            #region Replaceble objects *********************************************************************************************

            #region Vars 
            LV.Get();
            foreach (VariablesStruct v in LV.vars)
            {
                if (command.IndexOf(v.Name) > -1)
                {
                    string[] st = new LuaReferences().Replaceble;

                    foreach (string s1 in st)
                    {
                        foreach (string s2 in st)
                        {
                            if (command.IndexOf(s2 + v.Name + s1) > -1) { command = command.Replace(s2 + v.Name + s1, s2 + v.Value.ToString() + s1); }
                            if (command.IndexOf(s1 + v.Name + s2) > -1) { command = command.Replace(s1 + v.Name + s2, s1 + v.Value.ToString() + s2); }
                            if (command.IndexOf(v.Name + s1) > -1) { command = command.Replace(v.Name + s1, v.Value.ToString() + s1); }
                            if (command.IndexOf(s1 + v.Name) > -1) { command = command.Replace(s1 + v.Name, s1 + v.Value.ToString()); }
                            if (command == v.Name) { command = command.Replace(v.Name, v.Value.ToString()); }
                        }
                    }
                }
            }

            GV.Get();
            foreach (VariablesStruct v in GV.vars)
            {
                if (command.IndexOf(v.Name) > -1)
                {
                    string[] st = { " ", ")", "(", "]", "[", "{", "}", "+", "-", "/", "#", "%", "!", "=", "*", ",", ".", ";", "&", "_" };

                    foreach (string s1 in st)
                    {
                        foreach (string s2 in st)
                        {
                            if (command.IndexOf(s2 + v.Name + s1) > -1) { command = command.Replace(s2 + v.Name + s1, s2 + v.Value.ToString() + s1); }
                            if (command.IndexOf(s1 + v.Name + s2) > -1) { command = command.Replace(s1 + v.Name + s2, s1 + v.Value.ToString() + s2); }
                            if (command.IndexOf(v.Name + s1) > -1) { command = command.Replace(v.Name + s1, v.Value.ToString() + s1); }
                            if (command.IndexOf(s1 + v.Name) > -1) { command = command.Replace(s1 + v.Name, s1 + v.Value.ToString()); }
                            if (command == v.Name) { command = command.Replace(v.Name, v.Value.ToString()); }
                        }
                    }
                }
            }
            #endregion

            #endregion

            return command;
        }
        #endregion

        #region ************ Variables add ************

        public void VarSum(string var, double val)
        {
            if (var.IndexOf(" ") > -1) { var = var.Replace(" ", ""); }
            LV.Get();
            foreach (VariablesStruct v in LV.vars)
            {
                if (v.Name == var)
                {
                    //if (v.Type == "int")
                    try
                    {
                        v.Value = (Convert.ToInt64(v.Value) + val).ToString();
                        VarAdd(v);
                    }
                    catch
                    {

                    }
                }
            }
        }
        public void VarSub(string var, double val)
        {
            if (var.IndexOf(" ") > -1) { var = var.Replace(" ", ""); }
            LV.Get();
            foreach (VariablesStruct v in LV.vars)
            {
                if (v.Name == var)
                {
                    //if (v.Type == "int")
                    try
                    {
                        v.Value = (Convert.ToInt64(v.Value) - val).ToString();
                    }
                    catch
                    {

                    }
                }
            }
            LV.Set();
        }


        //insert new variable or rewrite
        public void VarAdd(VariablesStruct v)
        {
            LV.Get();
            if (v.Type == "Local")
            {
                for (int i = 0; i < LV.vars.LongCount(); i++)
                {
                    VariablesStruct va = LV.vars[i];
                    if (va.Name == v.Name) { va = v; LV.vars[i] = va; LV.Set(); return; }
                    LV.vars[i] = va;
                }
                LV.vars.Add(v);
                LV.Set();
            }
            else
            {
                GV.Get();
                for (int i = 0; i < GV.vars.LongCount(); i++)
                {
                    VariablesStruct va = GV.vars[i];
                    if (va.Name == v.Name)
                    {
                        va = v;
                        GV.vars[i] = va;
                        GV.Set();
                        return;
                    }
                    GV.vars[i] = va;
                }
                GV.vars.Add(v);
                GV.Set();
            }
        }

        //variable replace
        public void VarSet(string name, string type, object value)
        {
            LV.Get();
            if (type == "Local")
            {
                for (int i = 0; i < LV.vars.LongCount(); i++)
                {
                    VariablesStruct va = LV.vars[i];
                    if (va.Name == name) { va = new VariablesStruct(name,type,value); LV.Set(); return; }
                    LV.vars[i] = va;
                }
                LV.vars.Add(new VariablesStruct(name, type, value));
                LV.Set();
            }
            else
            {
                GV.Get();
                for (int i = 0; i < GV.vars.LongCount(); i++)
                {
                    VariablesStruct va = GV.vars[i];
                    if (va.Name == name) { va = new VariablesStruct(name, type, value); GV.Set(); return; }
                    GV.vars[i] = va;
                }
                GV.vars.Add(new VariablesStruct(name, type, value));
                GV.Set();
            }
        }

        //delete variable
        public void VarRem(string name)
        {
            LV.Get();
            for (int i = 0; i < LV.vars.LongCount(); i++)
            {
                VariablesStruct va = LV.vars[i];
                if (va.Name == name) { LV.vars.RemoveAt(i); LV.Set(); return; }
            }
            GV.Get();
            for (int i = 0; i < GV.vars.LongCount(); i++)
            {
                VariablesStruct va = GV.vars[i];
                if (va.Name == name) { GV.vars.RemoveAt(i); GV.Set(); return; }
            }
        }

        #endregion

    }
}
