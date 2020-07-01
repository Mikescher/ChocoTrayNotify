using ChocoTrayNotify.Powershell;
using System;
using System.Collections.ObjectModel;
using System.Text;

namespace ChocoTrayNotify.Log
{
    public class CTNLog
    {
        public ObservableCollection<CTNLogEntry> Entries { get; } = new ObservableCollection<CTNLogEntry>();

        private void Add(CTNLogEntry e)
        {
            Entries.Add(e);
        }

        public void AddInfo(string title, string msg = "")
        {
            Add(new CTNLogEntry(CTNLogType.Info, title, msg));
        }

        public void AddCommand(string title, string command, string args, ProcessResult result)
        {
            var txt = new StringBuilder();

            txt.AppendLine("[Command] " + command);
            txt.AppendLine("[Arguments] " + args);
            txt.AppendLine("[Extitcode] " + result.ExitCode);
            txt.AppendLine("[stdout]").AppendLine(result.StandardOut).Append("\n\n\n");
            txt.AppendLine("[stderr]").AppendLine(result.StandardErr).Append("\n\n\n");

            Add(new CTNLogEntry(CTNLogType.Command, title, txt.ToString()));
        }

        public void AddDebug(string txt)
        {
            Add(new CTNLogEntry(CTNLogType.Debug, txt, ""));
        }

        public void AddError(string msg, Exception e)
        {
            Add(new CTNLogEntry(CTNLogType.Error, msg, e.ToString()));
        }
    }
}
