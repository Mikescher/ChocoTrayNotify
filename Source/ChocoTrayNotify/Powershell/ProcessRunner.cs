using ChocoTrayNotify.Log;
using MSHC.Lang.Other;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ChocoTrayNotify.Powershell
{
    public static class ProcessRunner
    {
        public static async Task<ProcessResult> RunPowershell(string args, bool elevated, int timeout, bool showWindow)
        {
            GAS.Log.AddInfo($"Powershell command '{args}' started");

            ProcessResult r;
            if (elevated)
                r = await RunElevated(args, GAS.Settings.PowershellPath, "runas", args, timeout, showWindow);
            else
                r = await RunNormal(args, GAS.Settings.PowershellPath, "", args, timeout, showWindow);

            GAS.Log.AddInfo($"Powershell command '{args}' finished");

            return r;
        }

        public static async Task<ProcessResult> RunNormal(string title, string command, string verb, string args, int timeout, bool showWindow)
        {
            try
            {
                if (!File.Exists(command)) throw new ProcessException("Command not found", $"The command '{command}' was not found");

                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = command,
                        Verb = verb,
                        Arguments = args,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = !showWindow,
                    }
                };


                var builderOut  = new StringBuilder();
                var builderErr  = new StringBuilder();
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


                process.Start();
                var ok = await process.WaitForExitAsync(timeout);
                if (!ok) 
                {
                    try { process.Kill(); } catch { }
                    throw new ProcessException($"Command timed out after {timeout}ms", $"[Command]\n{command} {args}\n\n\n[stdout]\n{builderOut}\n\n\n[stderr]\n{builderErr}");
                }

                var result = new ProcessResult(builderOut.ToString(), builderErr.ToString(), builderBoth.ToString(), process.ExitCode);

                GAS.Log.AddCommand(title, command, args, result);

                return result;
            }
            catch (Exception e)
            {
                GAS.Log.AddError($"Command '{title}' failed", e);

                throw;
            }

        }

        public static async Task<ProcessResult> RunElevated(string title, string command, string verb, string args, int timeout, bool showWindow)
        {
            var tempFileOut = Path.Combine(Path.GetTempPath(), "chocotraynotify_" + Guid.NewGuid().ToString("N"));
            var tempFileErr = Path.Combine(Path.GetTempPath(), "chocotraynotify_" + Guid.NewGuid().ToString("N"));
            try
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = command,
                        Verb = verb,
                        Arguments = $"{args} 1> \"{tempFileOut}\" 2> \"{tempFileErr}\"",
                        CreateNoWindow = !showWindow,
                    }
                };

                process.Start();
                var ok = await process.WaitForExitAsync(timeout);
                if (!ok)
                {
                    try { process.Kill(); } catch { }

                    var msg = $"[Command]\n{command} {args}\n\n\n[Filepath-Out]\n{tempFileOut}\n\n\n[Filepath-Err]\n{tempFileErr}\n";

                    if (File.Exists(tempFileOut)) 
                        msg += $"\n\n[stdout]\n{File.ReadAllText(tempFileOut)}\n";
                    else
                        msg += $"\n\n[stdout]\n(FileMotFound)\n";

                    if (File.Exists(tempFileErr)) 
                        msg += $"\n\n[stderr]\n{File.ReadAllText(tempFileErr)}\n";
                    else
                        msg += $"\n\n[stderr]\n(FileMotFound)\n";


                    throw new ProcessException($"Command timed out after {timeout}ms", msg);
                }

                if (!File.Exists(tempFileOut)) throw new ProcessException($"Command did not write to output file (stdout)", $"[Command]\n{command} {args}\n\n\n[Filepath-Out]\n{tempFileOut}\n\n\n[Filepath-Err]\n{tempFileErr}");
                if (!File.Exists(tempFileErr)) throw new ProcessException($"Command did not write to output file (stderr)", $"[Command]\n{command} {args}\n\n\n[Filepath-Out]\n{tempFileOut}\n\n\n[Filepath-Err]\n{tempFileErr}");

                var stdout = File.ReadAllText(tempFileOut);
                var stderr = File.ReadAllText(tempFileErr);

                var result = new ProcessResult(stdout, stderr, stdout + "\n" + stderr, process.ExitCode);

                GAS.Log.AddCommand(title, command, args, result);

                return result;
            }
            catch (Exception e)
            {
                GAS.Log.AddError($"Command '{title}' failed", e);

                throw;
            }
            finally
            {
                if (File.Exists(tempFileOut)) File.Delete(tempFileOut);
                if (File.Exists(tempFileErr)) File.Delete(tempFileErr);
            }
        }
    }
}
