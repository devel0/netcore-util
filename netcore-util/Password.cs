using System;
using System.Text;
using System.Linq;

namespace SearchAThing.Util
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
            /// <summary>
            /// avoid ambigous chars
            /// for example : new[] { 'l', 'I', 'O', '0' }
            /// </summary>            
            public char[] AvoidChars { get; set; } = null;
            public bool AtLeastOneNumber { get; set; } = true;
            public bool AtLeastOneUppercase { get; set; } = true;
            public bool AtLeastOneSpecial { get; set; } = false;
            public int LoopCount { get; internal set; } = 0;
        }

        /// <summary>
        /// Generate random password using defaults ( allow numbers, lowercase, uppercase, 12 of length, no special symbol )
        /// </summary>        
        public static string RandomPassword(RandomPasswordOptions opts = null)
        {
            if (opts == null) opts = new RandomPasswordOptions();

            var sb = new StringBuilder();

            var rnd = new Random();

            var loopCount = 0;

            while (true)
            {
                if (loopCount > 100) throw new Exception($"unable to find suitable password in {loopCount} step");

                ++opts.LoopCount;

                sb.Clear();

                var specialCount = 0;
                var numberCount = 0;
                var uppercaseCount = 0;

                while (sb.Length < opts.Length)
                {
                    var n = rnd.Next(48, 122);
                    var c = Convert.ToChar(n);

                    if (opts.AvoidChars != null && opts.AvoidChars.Any(a => c == a)) continue;

                    if (char.IsNumber(c) && (opts.AllowNumber || (opts.AtLeastOneNumber && numberCount == 0)))
                    {
                        sb.Append(c);
                        ++numberCount;
                    }
                    else if (char.IsLetter(c))
                    {
                        if (char.IsLower(c) && opts.AllowLowercase) sb.Append(c);
                        else if (opts.AllowUppercase || (opts.AtLeastOneUppercase && uppercaseCount == 0))
                        {
                            sb.Append(c);
                            ++uppercaseCount;
                        }
                    }
                    else if (specialCount < opts.MaxSpecial && (opts.AllowSpecial || (opts.AtLeastOneSpecial && specialCount == 0)))
                    {
                        sb.Append(c);
                        specialCount++;
                    }
                }

                if (opts.AtLeastOneNumber && numberCount == 0) continue;
                if (opts.AtLeastOneSpecial && specialCount == 0) continue;
                if (opts.AtLeastOneUppercase && uppercaseCount == 0) continue;

                break;
            }

            return sb.ToString();
        }

    }

}