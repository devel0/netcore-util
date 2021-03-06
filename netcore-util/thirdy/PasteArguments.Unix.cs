// https://github.com/dotnet/corefx/blob/da1f8120aeec867bbd81829358ed348d323b1d14/src/Common/src/CoreLib/System/PasteArguments.Unix.cs

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SystemWrap
{
    internal static partial class PasteArguments
    {
        /// <summary>
        /// Repastes a set of arguments into a linear string that parses back into the originals under pre- or post-2008 VC parsing rules.
        /// On Unix: the rules for parsing the executable name (argv[0]) are ignored.
        /// </summary>
        internal static string PasteUnix(IEnumerable<string> arguments, bool pasteFirstArgumentUsingArgV0Rules)
        {
            var stringBuilder = new StringBuilder();
            foreach (string argument in arguments)
            {
                AppendArgument(stringBuilder, argument);
            }
            return stringBuilder.ToString();
        }

    }
}