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
                parser.AddCommand("cmd1", "command 1 with no sub commands");
                parser.AddCommand("cmd2", "command 2 with sub commands", new CmdlineParser("invoke cmd2 sub commands", (parser2) =>                
                {
                    parser2.AddShort("o2", "option on cmd2");
                    parser2.AddCommand("cmd-a", "command a");
                    parser2.AddCommand("cmd-b", "command b");

                    parser2.OnCmdlineMatch = () =>
                    {
                        System.Console.WriteLine(parser2);
                    };
                }));

                parser.OnCmdlineMatch = () =>
                {
                    System.Console.WriteLine(parser);
                };
            });

            cmdline.Run(args);
        }
    }
}
