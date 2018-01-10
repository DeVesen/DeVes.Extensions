namespace DeVes.Extensions.CodeCompiler
{
    public class CompilerError : ComileAtRuntimeInfo
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public string ErrorNumber { get; set; }
        public string FileName { get; set; }

        public CompilerError(string fileName, int line, int column, string errorNumber, string errorText, bool isWarning = false, bool isError = false)
            : base(errorText, isError, isWarning)
        {
            FileName = fileName;
            Line = line;
            Column = column;
            ErrorNumber = errorNumber;
        }

        internal CompilerError(System.CodeDom.Compiler.CompilerError compilerError)
            : base(compilerError.ErrorText, !compilerError.IsWarning, compilerError.IsWarning)
        {
            Line = compilerError.Line;
            Column = compilerError.Column;
            ErrorNumber = compilerError.ErrorNumber;
            FileName = compilerError.FileName;
        }
    }
}
