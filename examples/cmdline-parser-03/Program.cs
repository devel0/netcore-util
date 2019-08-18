using System;
using System.IO;
using SearchAThing;

namespace cmdline_parser_03
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmdline = new CmdlineParser("sample program", (parser) =>
            {
                var flag_short = parser.AddShort("fs", "flag short");
                var flag_short_val = parser.AddShort("fsv", "flag short with value", "VAL");
                var flag_short_val_mandatory = parser.AddMandatoryShort("mfsv", "mandatory flag short with value", "VAL");

                var flag_long = parser.AddLong("fl", "flag long");
                var flag_short_long = parser.AddShortLong("fsl", "flag-short-long", "flag short long");
                var flag_short_long_val = parser.AddShortLong("fslv", "flag-short-long-val", "flag short long", "VAL");
                var flag_short_long_val_mandatory = parser.AddMandatoryShortLong("mfslv", "mflag-short-long-val", "flag short long", "VAL");

                var files = parser.AddParameterArray("files", "file to test", mandatory: false);

                parser.OnCmdlineMatch = () =>
                {
                    System.Console.WriteLine(parser);
                };
            });

            cmdline.Run(args);
        }
    }
}
