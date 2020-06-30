using System;

namespace ChocoTrayNotify.Powershell
{
    public class ProcessException : Exception
    {
        public string Caption { get; } 
        public string Content { get; }

        public ProcessException(string messageCaption, string longerMessage) : base(messageCaption)
        {
            Caption = messageCaption;
            Content = longerMessage;
        }

        public override string ToString()
        {
            return Caption + "\n\n" + Content + "\n\n" + this.StackTrace;
        }
    }
}
