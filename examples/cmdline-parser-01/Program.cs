using System;
using System.IO;
using SearchAThing;

namespace cmdline_parser_01
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmdline = new CmdlineParser("sample program", (parser) =>
            {
                var flag_i = parser.AddShort("i", "print filename stats");                
                var file1 = parser.AddParameter("file1", "1th pathfilename to test", mandatory: false);
                var file2 = parser.AddParameter("file2", "2th pathfilename to test", mandatory: false);

                parser.OnCmdlineMatch = () =>
                {
                    System.Console.WriteLine(parser);

                    if (flag_i)
                    {
                        if (file1) System.Console.WriteLine($"Filesize [{(string)file1}] = {new FileInfo((string)file1).Length}");
                        if (file2) System.Console.WriteLine($"Filesize [{(string)file2}] = {new FileInfo((string)file2).Length}");
                    }
                };
            });

            cmdline.Run(args);
        }
    }
}
