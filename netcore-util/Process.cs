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

            // TODO: enable NETSTANDARD2_0 too but requires an extension to convert argument to single string with appropriate escaping if any

#if NETSTANDARD2_1

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
                            lock (lckstdout)
                            {                                
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

                    p.WaitForExit(); // flush async                                        

                    return (p.ExitCode, sbOut.ToString(), sbErr.ToString());
                });

                return await task;
            }

#endif

        }

    }

}