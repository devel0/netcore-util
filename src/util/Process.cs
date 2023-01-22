namespace SearchAThing.Util;

public static partial class Toolkit
{

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
    // [SupportedOSPlatform("linux")]
    public static async Task<ExecResult> ExecBashRedirect(string script,
        CancellationToken ct, bool sudo = false, bool verbose = false) =>
        await Exec("bash", new[] { "-c", script }, ct, sudo, true, true, verbose);

}
