using System;
using System.IO;
using SearchAThing;

namespace cmdline_parser_04
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmdline = new CmdlineParser("sample program", (parser) =>
            {
                parser.AddShort("b", "base option");
                var qvaltest = parser.AddShortLong("i", "qval-test", "value test", "VAL");
                parser.AddCommand("cmd1", "command 1 with no sub commands");
                parser.AddCommand("cmd2", "command 2 with sub commands", new CmdlineParser("invoke cmd2 sub commands", (parser2) =>
                {
                    parser2.AddShort("o2", "option on cmd2");
                    var qvaltest2 = parser2.AddShortLong("i2", "qval-test2", "value test2", "VAL2");
                    parser2.AddCommand("cmd-a", "command a");
                    parser2.AddCommand("cmd-b", "command b");

                    parser2.OnCmdlineMatch = () =>
                    {
                        var q = qvaltest ? qvaltest.Value : "";
                        var q2 = qvaltest2 ? qvaltest2.Value : "";
                        System.Console.WriteLine($"---> parser2 OnCmdlineMatch (val={q}) (val2={q2})");
                        System.Console.WriteLine(parser2);
                    };
                }));

                parser.OnCmdlineMatch = () =>
                {
                    var q = qvaltest ? qvaltest.Value : "";                    
                    System.Console.WriteLine($"---> parser OnCmdlineMatch (val={q})");
                    System.Console.WriteLine(parser);
                };
            });

            cmdline.Run(args);
        }
    }
}
