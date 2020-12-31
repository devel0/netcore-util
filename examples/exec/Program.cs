using System.Threading;
using System.Threading.Tasks;
using static SearchAThing.UtilToolkit;
using SearchAThing;
using System.Linq;

namespace exec
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var q = await Exec("ls", new[] { "-la", "/etc/hosts" }, CancellationToken.None,
                    sudo: false,
                    redirectStdout: true,
                    redirectStderr: false,
                    verbose: false);

                if (q.ExitCode == 0)
                {
                    var line = q.Output.Lines().First();
                    System.Console.WriteLine(line);

                    var ss = line.Split(' ');

                    System.Console.WriteLine($"perm: {ss[0]}");
                    System.Console.WriteLine($"owner: {ss[2]}");
                    System.Console.WriteLine($"group: {ss[3]}");
                    System.Console.WriteLine($"size: {ss[4]}");
                }

                // RESULT:
                //
                // -rw-r--r-- 1 root root 218 May 11  2020 /etc/hosts
                // perm: -rw-r--r--
                // owner: root
                // group: root
                // size: 218

            }).Wait();
        }
    }
}
