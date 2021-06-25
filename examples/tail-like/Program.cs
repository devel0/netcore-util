using static SearchAThing.UtilToolkit;

namespace mytest
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var x in TailLike("/var/log/syslog"))
            {
                System.Console.WriteLine(x);
            }
        }
    }
}
