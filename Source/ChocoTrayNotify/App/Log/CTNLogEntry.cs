using System;

namespace ChocoTrayNotify.Log
{
    public class CTNLogEntry
    {
        public DateTime   Timestamp { get; }
        public CTNLogType Type      { get; }
        public string     Title     { get; }
        public string     Message   { get; }

        public CTNLogEntry(CTNLogType t, string title, string msg)
        {
            Timestamp = DateTime.Now;
            Type      = t;
            Title     = title;
            Message   = msg;
        }
    }
}
