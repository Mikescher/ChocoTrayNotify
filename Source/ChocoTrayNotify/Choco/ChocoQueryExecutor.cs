using ChocoTrayNotify.Powershell;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChocoTrayNotify.Choco
{
    public class ChocoQueryExecutor
    {
        private static async Task<string> Run(string cmd)
        {
            var r = await ProcessRunner.RunPowershell(cmd, 
                GAS.Settings.PowershellElevate, 
                GAS.Settings.ChocoCommandTimeout, 
                GAS.Settings.ShowPowershellWindow);

            if (r.ExitCode    != 0)  throw new ProcessException($"Command returned with exitcode {r.ExitCode}", $"[Command]\n{cmd}\n\n\n[stdout]\n{r.StandardOut}\n\n\n[stderr]\n{r.StandardErr}\n\n\n[exitcode]\n{r.ExitCode}");
            if (r.StandardErr != "") throw new ProcessException($"Command returned with stderr", $"[Command]\n{cmd}\n\n\n[stdout]\n{r.StandardOut}\n\n\n[stderr]\n{r.StandardErr}\n\n\n[exitcode]\n{r.ExitCode}");

            return r.StandardOut;
        }

        public static async Task<List<ChocoPackage>> QueryFull()
        {
            var results = await QueryInstalled();

            foreach (var outdated in await QueryOutdated())
            {
                var dst = results.FirstOrDefault(p => p.PackageName == outdated.PackageName);

                if (dst == null) throw new ChocoCommandException($"Command 'choco outdated' returned invalid package (not in 'choco list')'", $"Package '{outdated.PackageName}' not found in List:\n{string.Join("\n", results.Select(p => " - " + p.PackageName))}");

                results.Remove(dst);
                results.Add(outdated);
            }

            return results;
        }

        public static async Task<List<ChocoPackage>> QueryOutdated()
        {
            var resultOutdated = await Run(GAS.Settings.ChocoCommand + " outdated --limit-output --no-color");
            
            var lines = Regex.Split(resultOutdated, @"\r?\n");

            var results = new List<ChocoPackage>();

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line) && i == 0 || i == (lines.Length-1)) continue;

                var m = Regex.Match(line, @"^(?<name>[^\|\s]+)\|(?<v1>[0-9\.]+)\|(?<v2>[0-9\.]+)\|(?<pinned>true|false)$");

                if (!m.Success) throw new ChocoCommandException($"Could not parse 'choco outdated' command output (regex mismatch)", $"[choco outdated]\n{resultOutdated}\n\n\n[Error-Line]\n{line}");

                results.Add(new ChocoPackage(m.Groups["name"].Value, m.Groups["v1"].Value, m.Groups["v2"].Value, ParseChocoBool(m.Groups["pinned"].Value, line)));
            }

            return results.OrderBy(p => p.PackageName.ToLower()).ToList();
        }

        public static async Task<List<ChocoPackage>> QueryInstalled()
        {
            var results = new List<ChocoPackage>();

            var resultList = await Run(GAS.Settings.ChocoCommand + " list --local-only --limit-output --no-color");
            var resultPin  = await Run(GAS.Settings.ChocoCommand + " pin list --limit-output --no-color");

            var linesList = Regex.Split(resultList, @"\r?\n");
            for (int i = 0; i < linesList.Length; i++)
            {
                var line = linesList[i].Trim();
                if (string.IsNullOrWhiteSpace(line) && i == 0 || i == (linesList.Length - 1)) continue;

                var m = Regex.Match(line, @"^(?<name>[^\|\s]+)\|(?<v1>[0-9\.]+)$");

                if (!m.Success) throw new ChocoCommandException($"Could not parse 'choco list' command output (regex mismatch)", $"[choco list]\n{resultList}\n\n\n[Error-Line]\n{line}");

                results.Add(new ChocoPackage(m.Groups["name"].Value, m.Groups["v1"].Value, m.Groups["v1"].Value, false));
            }

            var linesPin = Regex.Split(resultPin, @"\r?\n");
            for (int i = 0; i < linesPin.Length; i++)
            {
                var line = linesPin[i].Trim();
                if (string.IsNullOrWhiteSpace(line) && i == 0 || i == (linesPin.Length - 1)) continue;

                var m = Regex.Match(line, @"^(?<name>[^\|\s]+)\|(?<v1>[0-9\.]+)$");

                if (!m.Success) throw new ChocoCommandException($"Could not parse 'choco pin list' command output (regex mismatch)", $"[choco pin list]\n{resultPin}\n\n\n[Error-Line]\n{line}");

                var pname = m.Groups["name"].Value;
                var dst = results.FirstOrDefault(p => p.PackageName == pname);

                if (dst == null) throw new ChocoCommandException($"Command 'choco pin list' returned invalid package", $"[choco list]\n{resultList}\n\n\n[choco pin list]\n{resultPin}\n\n\n[Error]\nPackage '{pname}' not found");

                results.Remove(dst);
                results.Add(new ChocoPackage(dst.PackageName, dst.CurrentVersion, dst.AvailableVersion, dst.Pinned));
            }

            return results.OrderBy(p => p.PackageName.ToLower()).ToList();
        }

        private static bool ParseChocoBool(string value, string line)
        {
            if (value == "true") return true;
            if (value == "false") return false;
            throw new ChocoCommandException($"Could not parse command (not a boolean value)", $"[Value]\n{value}\n\n\n[line]\n{line}");
        }
    }
}
