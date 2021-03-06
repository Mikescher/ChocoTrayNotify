﻿using System;

namespace ChocoTrayNotify.Choco
{
    public class ChocoCommandException : Exception
    {
        public string Caption { get; } 
        public string Content { get; }

        public ChocoCommandException(string messageCaption, string longerMessage) : base(messageCaption)
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
