using System;
using System.IO;
using SearchAThing;

namespace cmdline_parser_02
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmdline = new CmdlineParser("sample program", (parser) =>
            {
                var flag_i = parser.AddShort("i", "print file size");
                var files = parser.AddParameterArray("files", "file to test", mandatory: false);

                parser.OnCmdlineMatch = () =>
                {
                    foreach (var file in files)
                    {
                        if (flag_i)
                        {
                            System.Console.WriteLine($"Filesize [{(string)file}] = {new FileInfo((string)file).Length}");                            
                        }
                    }
                };
            });

            cmdline.Run(args);
        }
    }
}
