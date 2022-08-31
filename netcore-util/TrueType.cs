using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace SearchAThing
{

    /// <summary>
    /// extract some info from ttx file
    /// ( use `fonttools ttx file.ttf` to generate xml representation )
    /// </summary>
    public class TrueTypeNfo
    {

        public string TTXPathfilename { get; private set; }

        XElement xml;

        List<int>? widths = null;

        public IReadOnlyList<int>? Widths => widths;

        public int Ascent { get; private set; }
        public int Descent { get; private set; }
        public int ItalicAngle { get; private set; }
        public string Name { get; private set; } = "";
        public int MaxWidth { get; private set; }
        public int AvgWidth { get; private set; }

        public TrueTypeNfo(string ttxPathfilename)
        {
            TTXPathfilename = ttxPathfilename;

            var sr = new StreamReader(ttxPathfilename);

            xml = XElement.Load(sr);

            var hhea = xml.Descendants("hhea");

            if (hhea != null)
            {

                var ascent = hhea.Elements("ascent");
                var descent = hhea.Elements("descent");

                if (ascent != null && descent != null)
                {
                    var qascent = ascent.Select(x => x.Attribute("value")).Where(r => r != null).Select(r => r!);
                    var qdescent = descent.Select(x => x.Attribute("value")).Where(r => r != null).Select(r => r!);

                    if (qascent != null && qdescent != null)
                    {
                        Ascent = (int)qascent.First();
                        Descent = (int)qdescent.First();
                    }
                }

                var post = xml.Descendants("post");
                if (post != null)
                {
                    var qitalic = post.Elements("italicAngle").Select(x => x.Attribute("value")).Where(r => r != null).Select(r => r!);
                    ItalicAngle = (int)(double)qitalic.First();
                }

                var name = xml.Descendants("name");
                if (name != null)
                {
                    var qname = name.Elements("namerecord")?.Select(x => x.Attribute("nameID"))
                        .Where(r => r != null).Select(r => r!)
                        .FirstOrDefault(r => (int)r == 1);

                    if (qname != null)
                        Name = (string)qname.Value.Trim();
                }

                var hmtx = xml.Descendants("hmtx");

                widths = new List<int>();
                MaxWidth = 0;
                int wSum = 0;
                foreach (var mtx in hmtx.Elements("mtx"))
                {
                    var qw = mtx.Attribute("width");

                    if (qw != null)
                    {
                        var w = (int)qw;
                        if (w > MaxWidth) MaxWidth = w;
                        wSum += w;
                        widths.Add(w);
                    }
                }

                AvgWidth = wSum / widths.Count;
            }
        }

    };

}