namespace ChocoTrayNotify.Log
{
    public class CTLogEntry
    {
        public CTLogType Type    { get; }
        public string    Title   { get; }
        public string    Message { get; }

        public CTLogEntry(CTLogType t, string title, string msg)
        {
            Type    = t;
            Title   = title;
            Message = msg;
        }
    }
}
