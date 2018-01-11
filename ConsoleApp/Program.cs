using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeVes.Extensions;
using DeVes.Extensions.ParamDict;
using Newtonsoft.Json.Linq;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var _pd = new ParamDict();

            _pd["00.01.02"] = 1;
            _pd["k1"] = 1;
            _pd["k2"] = 2;
            _pd["k3"] = 3;
            _pd["k4"] = 4;
            _pd["k5"] = 5;

            var _element = _pd["00.01"];

            RuntimeCompiler();
        }



        private static void RuntimeCompiler()
        {
            string[] _code = {
            "using System;"+
            "namespace DynaCore"+
            "{"+
            "   public class DynaCore"+
            "   {"+
            "       static public string Run(string str, out int txtLength)"+
            "       {"+
            "           txtLength = str.Length;"+
            "           return \"Die länge ist: \" + txtLength.ToString();"+
            "       }"+
            "   }"+
            "}"};

            using (var _comp = new DeVes.Extensions.CodeCompiler.RuntimeCompiler())
            {
                var _compiled = _comp.CompileCode(_code);
                var _result = _comp.Invoke("DynaCore", "DynaCore", "Run", null, "here in dyna code", 0);
            }
        }
    }
}
