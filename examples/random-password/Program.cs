using System;
using SearchAThing;
using static SearchAThing.UtilToolkit;

namespace mytest
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                var opts = new RandomPasswordOptions()
                {
                    AtLeastOneNumber = true,
                    AtLeastOneSpecial = false,
                    AtLeastOneUppercase = true,
                    Length = 8,
                    AvoidChars = new[] { 'l', 'I', 'O', '0' }
                };
                var pass = RandomPassword(opts);
                System.Console.WriteLine($"pass [{pass}] in {opts.LoopCount} loop count");
            }

            {
                var opts = new RandomPasswordOptions()
                {
                    AllowLetter = false,
                    AtLeastOneUppercase = false,
                    Length = 4
                };
                var pass = RandomPassword(opts);
                System.Console.WriteLine($"pin [{pass}] in {opts.LoopCount} loop count");
            }
        }
    }
}
