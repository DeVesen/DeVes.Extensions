namespace DeVes.Extensions.CodeCompiler
{
    public abstract class ComileAtRuntimeInfo
    {
        public bool IsError { get; set; }
        public bool IsWarning { get; set; }
        public string ErrorText { get; set; }

        protected ComileAtRuntimeInfo(string errorText, bool isError, bool isWarning)
        {
            IsWarning = isWarning;
            IsError = isError;
            ErrorText = errorText;
        }
    }
}
