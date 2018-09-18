using System;
using SearchAThing.Util;

namespace mytest
{
    class Program
    {
        static void Main(string[] args)
        {
            var opts = new Util.RandomPasswordOptions()
            {
                AtLeastOneNumber = true,
                AtLeastOneSpecial = false,
                AtLeastOneUppercase = true,
                Length = 8,
                AvoidChars = new[] { 'l', 'I', 'O', '0' }
            };
            var pass = Util.RandomPassword(opts);
            System.Console.WriteLine($"pass [{pass}] in {opts.LoopCount} loop count");
        }
    }
}
