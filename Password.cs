
using System;
using System.Text;

namespace SearchAThing.NETCoreUtil
{

    public static partial class Util
    {

        public class RandomPasswordOptions
        {
            public bool AllowNumber { get; set; } = true;
            public bool AllowLowercase { get; set; } = true;
            public bool AllowUppercase { get; set; } = true;
            public bool AllowSpecial { get; set; } = false;
            public int MaxSpecial { get; set; } = 1;
            public int Length { get; set; } = 12;
        }

        public static string RandomPassword(RandomPasswordOptions opts = null)
        {
            if (opts == null) opts = new RandomPasswordOptions();

            var sb = new StringBuilder();

            var rnd = new Random();

            var specialCount = 0;

            while (sb.Length < opts.Length)
            {
                var n = rnd.Next(48, 122);
                var c = Convert.ToChar(n);

                if (opts.AllowNumber && char.IsNumber(c)) sb.Append(c);
                else if (char.IsLetter(c))
                {
                    if (char.IsLower(c) && opts.AllowLowercase) sb.Append(c);
                    else if (opts.AllowUppercase) sb.Append(c);
                }
                else if (specialCount < opts.MaxSpecial && opts.AllowSpecial)
                {
                    sb.Append(c);
                    specialCount++;
                }
            }

            return sb.ToString();
        }

    }

}