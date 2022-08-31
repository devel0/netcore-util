using System;
using System.Text;
using System.Linq;

namespace SearchAThing
{

    /// <summary>
    /// to generate a pin use follow config ( AllowLetter = false, AtLeastOneUppercase = false, Length = 4 )
    /// </summary>
    public class RandomPasswordOptions
    {
        /// <summary>
        /// default: true
        /// </summary>            
        public bool AllowNumber { get; set; } = true;

        /// <summary>
        /// default: true
        /// </summary>            
        public bool AllowLetter { get; set; } = true;

        /// <summary>
        /// default: true
        /// </summary>            
        public bool AllowLowercase { get; set; } = true;

        /// <summary>
        /// default: true
        /// </summary>            
        public bool AllowUppercase { get; set; } = true;

        /// <summary>
        /// default: false
        /// </summary>            
        public bool AllowSpecial { get; set; } = false;

        /// <summary>
        /// default: 1
        /// </summary>            
        public int MaxSpecial { get; set; } = 1;

        /// <summary>
        /// default: 12
        /// </summary>            
        public int Length { get; set; } = 12;
        /// <summary>
        /// avoid ambigous chars
        /// for example : new[] { 'l', 'I', 'O', '0' }
        /// default: null
        /// </summary>            
        public char[] AvoidChars { get; set; } = { };

        /// <summary>
        /// default: true
        /// </summary>            
        public bool AtLeastOneNumber { get; set; } = true;

        /// <summary>
        /// default: true
        /// </summary>            
        public bool AtLeastOneUppercase { get; set; } = true;

        /// <summary>
        /// default: false
        /// </summary>            
        public bool AtLeastOneSpecial { get; set; } = false;

        /// <summary>
        /// default: 0
        /// </summary>            
        public int LoopCount { get; internal set; } = 0;
    }

    public static partial class UtilToolkit
    {

        /// <summary>
        /// Generate random password using defaults ( allow numbers, lowercase, uppercase, 12 of length, no special symbol )
        /// </summary>        
        public static string RandomPassword(RandomPasswordOptions? opts = null)
        {
            if (opts is null) opts = new RandomPasswordOptions();

            var sb = new StringBuilder();

            var rnd = new Random();

            var loopCount = 0;

            {
                var cntRequired = 0;
                if (opts.AtLeastOneNumber) ++cntRequired;
                if (opts.AtLeastOneSpecial) ++cntRequired;
                if (opts.AtLeastOneUppercase) ++cntRequired;
                if (cntRequired > opts.Length) throw new ArgumentException($"invalid length not enough for required at least {cntRequired} characters");
                if (opts.Length == 0) throw new ArgumentException($"invalid 0 length");
            }

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
                    else if (char.IsLetter(c) && opts.AllowLetter)
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