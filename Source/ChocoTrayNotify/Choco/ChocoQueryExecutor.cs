using ChocoTrayNotify.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChocoTrayNotify.Choco
{
    public class ChocoQueryExecutor
    {
        private static async Task<string> Query(string cmd)
        {
            if (CTNSettings.Inst.PowershellElevate) return await QueryElevated(cmd); else return await QueryNormal(cmd);
        }

        private static async Task<string> QueryElevated(string cmd)
        {
            var tempFile = Path.Combine(Path.GetTempPath(), "chocotraynotify_" + Guid.NewGuid().ToString("N"));
            try
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = @"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe",
                        Verb = "runas",
                        Arguments = $"{cmd} 2>&1 > \"{tempFile}\"",
                        CreateNoWindow = true,
                    }
                };

                var timeout = CTNSettings.Inst.ChocoCommandTimeout;
                process.Start();
                var ok = await process.WaitForExitAsync(timeout);
                if (!ok) throw new ChocoCommandException($"Choco command timed out after {timeout}ms", $"[Command]\n{cmd}\n\n\n[Filepath]\n{tempFile}");

                if (!File.Exists(tempFile)) throw new ChocoCommandException($"Choco command did not write to output file", $"[Command]\n{cmd}\n\n\n[Filepath]\n{tempFile}");

                return File.ReadAllText(tempFile);
            }
            finally
            {
                if (File.Exists(tempFile)) File.Delete(tempFile);
            }
        }

        private static async Task<string> QueryNormal(string cmd)
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = @"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe",
                    Arguments = cmd,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                }
            };


            var builderOut = new StringBuilder();
            var builderErr = new StringBuilder();
            var builderBoth = new StringBuilder();

            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data == null) return;

                if (builderOut.Length == 0) builderOut.Append(args.Data);
                else builderOut.Append("\n" + args.Data);

                if (builderBoth.Length == 0) builderBoth.Append(args.Data);
                else builderBoth.Append("\n" + args.Data);
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data == null) return;

                if (builderErr.Length == 0) builderErr.Append(args.Data);
                else builderErr.Append("\n" + args.Data);

                if (builderBoth.Length == 0) builderBoth.Append(args.Data);
                else builderBoth.Append("\n" + args.Data);
            };

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();


            var timeout = CTNSettings.Inst.ChocoCommandTimeout;
            process.Start();
            var ok = await process.WaitForExitAsync(timeout);
            if (!ok) throw new ChocoCommandException($"Choco command timed out after {timeout}ms", $"[Command]\n{cmd}\n\n\n[stdout]\n{builderOut}\n\n\n[stderr]\n{builderErr}");


            if (process.ExitCode != 0) throw new ChocoCommandException($"Choco command returned with exitcode {process.ExitCode}", $"[Command]\n{cmd}\n\n\n[stdout]\n{builderOut}\n\n\n[stderr]\n{builderErr}\n\n\n[exitcode]\n{process.ExitCode}");

            return builderBoth.ToString();
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
            var resultOutdated = await Query("choco outdated --limit-output --no-color");
            
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

            var resultList = await Query("choco list --local-only --limit-output --no-color");
            var resultPin  = await Query("choco pin list --limit-output --no-color");

            var linesList = Regex.Split(resultList, @"\r?\n");
            for (int i = 0; i < linesList.Length; i++)
            {
                var line = linesList[i].Trim();
                if (string.IsNullOrWhiteSpace(line) && i == 0 || i == (linesList.Length - 1)) continue;

                var m = Regex.Match(line, @"^(?<name>[^\|\s]+)\|(?<v1>[0-9\.]+)$");

                if (!m.Success) throw new ChocoCommandException($"Could not parse 'choco list' command output (regex mismatch)", $"[choco list]\n{resultList}\n\n\n[Error-Line]\n{line}");

                results.Add(new ChocoPackage(m.Groups["name"].Value, m.Groups["v1"].Value, null, false));
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
