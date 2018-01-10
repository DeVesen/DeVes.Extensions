namespace DeVes.Extensions.CodeCompiler
{
    public class RuntimeError : ComileAtRuntimeInfo
    {
        public RuntimeError(string errorText, bool isError = false, bool isWarning = false)
            : base(errorText, isError, isWarning)
        {
        }
    }
}
