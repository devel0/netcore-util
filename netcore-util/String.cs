using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.FormattableString;
using static System.Math;

namespace SearchAThing
{

	public static partial class UtilToolkit
	{
		public static string ToJson(object o) => JsonConvert.SerializeObject(o);
	}

}
