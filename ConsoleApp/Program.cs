using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
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
