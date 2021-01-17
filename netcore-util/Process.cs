using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;

namespace SearchAThing
{

    /// <summary>
    /// results of process executio
    /// </summary>
    public class ExecResult
    {
        public ExecResult(int exitCode, string output, string error)
        {
            ExitCode = exitCode;
            Output = output;
            Error = error;
        }
        /// <summary>
        /// exitcode of process execuition
        /// </summary>        
        public int ExitCode { get; private set; }

        /// <summary>
        /// stdout of process execution if redirection was used
        /// </summary>    
        public string Output { get; private set; }

        /// <summary>
        /// stderr of process execution if redirection was used
        /// </summary>        
        public string Error { get; private set; }
    }

    public static partial class UtilToolkit
    {


        /// <summary>
        /// start a process in background without redirecting standard output, error;
        /// a cancellation token can be supplied to cancel underlying process            
        /// </summary>
        public static async Task<ExecResult> ExecNoRedirect(string cmd,
            IEnumerable<string> args, CancellationToken ct, bool sudo = false, bool verbose = false) =>
            await Exec(cmd, args, ct, sudo, false, false, verbose);

        /// <summary>
        /// start a process in background redirecting standard output, error;
        /// a cancellation token can be supplied to cancel underlying process            
        /// </summary>
        public static async Task<ExecResult> ExecRedirect(string cmd,
            IEnumerable<string> args, CancellationToken ct, bool sudo = false, bool verbose = false) =>
            await Exec(cmd, args, ct, sudo, true, true, verbose);

        /// <summary>
        /// start a process in background redirecting standard error;
        /// a cancellation token can be supplied to cancel underlying process            
        /// </summary>
        public static async Task<ExecResult> ExecRedirectError(string cmd,
            IEnumerable<string> args, CancellationToken ct, bool sudo = false, bool verbose = false) =>
            await Exec(cmd, args, ct, sudo, false, true, verbose);

        /// <summary>
        /// start a bash process in background redirecting standard output, error;
        /// given script can contains pipe and other shell related redirections.
        /// a cancellation token can be supplied to cancel underlying process    
        /// </summary>
        /// <param name="script">bash script to execute</param>
        /// <param name="ct">cancellation token</param>
        /// <param name="sudo">true if sudo required</param>
        /// <param name="verbose">if true prints command and args used</param>
        /// <returns></returns>
        public static async Task<ExecResult> ExecBashRedirect(string script,
            CancellationToken ct, bool sudo = false, bool verbose = false) =>
            await Exec("bash", new[] { "-c", script }, ct, sudo, true, true, verbose);

        /// <summary>
        /// start a process in background redirecting standard output, error;
        /// a cancellation token can be supplied to cancel underlying process
        /// </summary>
        /// <param name="cmd">cmd to execute</param>
        /// <param name="args">cmd arguments ( array of strings )</param>
        /// <param name="ct">cancellation token</param>
        /// <param name="sudo">true if sudo required</param>
        /// <param name="redirectStdout">redirect process stdout and grab into output</param>
        /// <param name="redirectStderr">redirect process stderr and grab into error</param>
        /// <param name="verbose">if true prints command and args used</param>                  
        public static async Task<ExecResult> Exec(string cmd,
            IEnumerable<string> args, CancellationToken ct, bool sudo = false,
            bool redirectStdout = true, bool redirectStderr = true,
            bool verbose = false)
        {
            var task = Task<ExecResult>.Run(() =>
            {
                var p = new Process();
                p.StartInfo.UseShellExecute = !redirectStdout && !redirectStderr;
                p.StartInfo.RedirectStandardOutput = redirectStdout;
                p.StartInfo.RedirectStandardError = redirectStderr;

                if (sudo)
                {
                    p.StartInfo.FileName = "sudo";
                    var lst = new List<string>(args);
                    lst.Insert(0, cmd);
                    args = lst;
                }
                else
                    p.StartInfo.FileName = cmd;

                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    p.StartInfo.Arguments = SystemWrap.PasteArguments.PasteUnix(args, pasteFirstArgumentUsingArgV0Rules: true);
                }
                else
                {
                    p.StartInfo.Arguments = SystemWrap.PasteArguments.PasteWindows(args, pasteFirstArgumentUsingArgV0Rules: true);
                }

                var sbOut = new StringBuilder();
                var sbErr = new StringBuilder();

                object lckstdout = new object();
                object lckstderr = new object();

                if (redirectStdout)
                {
                    p.OutputDataReceived += (s, e) =>
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

                if (verbose)
                    System.Console.WriteLine($"{p.StartInfo.FileName} {p.StartInfo.Arguments}");

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

                return new ExecResult(p.ExitCode, sbOut.ToString(), sbErr.ToString());
            });

            return await task;
        }

    }

}