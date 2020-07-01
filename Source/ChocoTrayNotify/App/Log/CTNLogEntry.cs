namespace ChocoTrayNotify.Log
{
    public class CTNLogEntry
    {
        public CTNLogType Type    { get; }
        public string     Title   { get; }
        public string     Message { get; }

        public CTNLogEntry(CTNLogType t, string title, string msg)
        {
            Type    = t;
            Title   = title;
            Message = msg;
        }
    }
}
