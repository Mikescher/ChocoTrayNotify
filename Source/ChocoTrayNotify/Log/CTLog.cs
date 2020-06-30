using ChocoTrayNotify.Powershell;
using System;
using System.Collections.ObjectModel;
using System.Text;

namespace ChocoTrayNotify.Log
{
    public class CTLog
    {
        public static readonly CTLog Inst = new CTLog();

        public ObservableCollection<CTLogEntry> Entries { get; } = new ObservableCollection<CTLogEntry>();

        private void Add(CTLogEntry e)
        {
            Entries.Add(e);
        }

        public static void AddInfo(string title, string msg = "")
        {
            Inst.Add(new CTLogEntry(CTLogType.Info, title, msg));
        }

        public static void AddCommand(string title, string command, string args, ProcessResult result)
        {
            var txt = new StringBuilder();

            txt.AppendLine("[Command] " + command);
            txt.AppendLine("[Arguments] " + args);
            txt.AppendLine("[Extitcode] " + result.ExitCode);
            txt.AppendLine("[stdout]").AppendLine(result.StandardOut).Append("\n\n\n");
            txt.AppendLine("[stderr]").AppendLine(result.StandardErr).Append("\n\n\n");

            Inst.Add(new CTLogEntry(CTLogType.Command, title, txt.ToString()));
        }

        public static void AddError(string msg, Exception e)
        {
            Inst.Add(new CTLogEntry(CTLogType.Error, msg, e.ToString()));
        }
    }
}
