namespace ChocoTrayNotify.Powershell
{
    public class ProcessResult
    {
        public readonly string StandardOut;
        public readonly string StandardErr;
        public readonly string StandardCombined;
        public readonly int    ExitCode;

        public ProcessResult(string standardOut, string standardErr, string standardCombined, int returnCode)
        {
            StandardOut      = standardOut;
            StandardErr      = standardErr;
            StandardCombined = standardCombined;
            ExitCode         = returnCode;
        }
    }
}
