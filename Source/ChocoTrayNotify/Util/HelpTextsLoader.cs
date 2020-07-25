using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ChocoTrayNotify.Util
{
	public static class HelpTextsLoader
	{
		private static Dictionary<string, string> _texts = null;

		private static void Load()
		{
			_texts = new Dictionary<string, string>();

			LoadFromResources();
		}

		private static void LoadFromResources()
		{
			var lines = Regex.Split(Properties.Resources.HelpTexts, @"\r?\n");

			string curr = null;
			StringBuilder collector = new StringBuilder();
			for (int i = 0; i < lines.Length; i++)
			{
				string l = lines[i].TrimEnd();
				if (l.StartsWith("[") && l.EndsWith("]"))
				{
					if (curr != null) _texts[curr] = collector.ToString().Trim();
					curr = l.Substring(1, l.Length - 2);
					collector.Clear();
					continue;
				}
				else
				{
					collector.AppendLine(l);
				}
			}
			if (curr != null) _texts[curr] = collector.ToString().Trim();
		}

		public static string Get(string id)
		{
			if (_texts == null) Load();
			if (_texts == null) return "%ERROR%";

			if (_texts.TryGetValue(id, out var v)) return v.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);

			return "%ERROR%";
		}
	}
}
