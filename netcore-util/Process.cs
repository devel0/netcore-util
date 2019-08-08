using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SearchAThing
{

    namespace Util
    {

        public static partial class Toolkit
        {

            /// <summary>
            /// start a process in background redirecting standard output, error;
            /// a cancellation token can be supplied to cancel underlying process
            /// (this method will not redirect stdout and stderr)
            /// </summary>
            public static async Task<(int exitcode, string output, string error)> ExecNoRedirect(string cmd,
                IEnumerable<string> args, CancellationToken ct, bool sudo = false) =>
                await Exec(cmd, args, ct, sudo, false, false);

            /// <summary>
            /// start a process in background redirecting standard output, error;
            /// a cancellation token can be supplied to cancel underlying process
            /// </summary>        
            public static async Task<(int exitcode, string output, string error)> Exec(string cmd,
                IEnumerable<string> args, CancellationToken ct, bool sudo = false, bool redirectStdout = true, bool redirectStderr = true)
            {
                var task = Task<(int exitcode, string output, string error)>.Run(async () =>
                {
                    System.Console.WriteLine($"executing [{cmd} {string.Join(" ", args)}");
                    var p = new Process();
                    p.StartInfo.UseShellExecute = !redirectStdout && !redirectStderr;
                    p.StartInfo.RedirectStandardOutput = redirectStdout;
                    p.StartInfo.RedirectStandardError = redirectStderr;
                    if (sudo)
                    {                        
                        p.StartInfo.FileName = "sudo";                        
                        p.StartInfo.ArgumentList.Add(cmd);
                    }
                    else
                        p.StartInfo.FileName = cmd;
                    foreach (var a in args) p.StartInfo.ArgumentList.Add(a);

                    var sbOut = new StringBuilder();
                    var sbErr = new StringBuilder();

                    object lckstdout = new object();
                    object lckstderr = new object();

                    if (redirectStdout)
                    {
                        p.OutputDataReceived += async (s, e) =>
                        {
                            await Task.Delay(1000);

                            lock (lckstdout)
                            {
                                System.Console.WriteLine($"--> recv [{e.Data}]");
                                sbOut.AppendLine(e.Data);
                            }
                        };
                    }

                    if (redirectStderr)
                    {
                        p.ErrorDataReceived += (s, e) =>
                        {
                            lock (lckstderr)
                            {
                                sbErr.AppendLine(e.Data);
                            }
                        };
                    }

                    if (!p.Start()) throw new Exception($"can't run process");

                    if (redirectStdout) p.BeginOutputReadLine();
                    if (redirectStderr) p.BeginErrorReadLine();

                    while (!ct.IsCancellationRequested)
                    {
                        if (p.WaitForExit(200))
                        {
                            p.WaitForExit();
                            break;
                        }
                    }

                    if (ct.IsCancellationRequested)
                    {
                        if (!p.HasExited)
                        {
                            if (redirectStdout) p.CancelOutputRead();
                            if (redirectStderr) p.CancelErrorRead();

                            p.Kill();
                        }
                    }

                    var o = sbOut.ToString();
                    System.Console.WriteLine($"out=[{sbOut.ToString()}]");
                    if (string.IsNullOrEmpty(o))
                    {
                        System.Console.WriteLine($"empty out waiting");
                        await Task.Delay(1000);
                    }
                    System.Console.WriteLine($"now out [{sbOut.ToString()}]");

                    return (p.ExitCode, sbOut.ToString(), sbErr.ToString());
                });

                return await task;
            }

        }

    }

}