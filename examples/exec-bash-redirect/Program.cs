using System.Threading;
using System.Threading.Tasks;
using static SearchAThing.UtilToolkit;

namespace exec
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var q = await ExecBashRedirect("i=0; while (($i < 5)); do echo $i; let i=$i+1; done",
                    CancellationToken.None,
                    sudo: false,                    
                    verbose: false);

                if (q.ExitCode == 0)
                {
                    System.Console.WriteLine($"output[{q.Output}]");                    
                }

                // RESULT:
                //
                // output[0
                // 1
                // 2
                // 3
                // 4

                // ]
                
            }).Wait();
        }
    }
}
