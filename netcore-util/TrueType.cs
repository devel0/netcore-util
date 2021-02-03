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

        List<int> widths = null;

        public IReadOnlyList<int> Widths => widths;

        public int Ascent { get; private set; }
        public int Descent { get; private set; }
        public int ItalicAngle { get; private set; }
        public string Name { get; private set; }
        public int MaxWidth { get; private set; }
        public int AvgWidth { get; private set; }

        public TrueTypeNfo(string ttxPathfilename)
        {
            TTXPathfilename = ttxPathfilename;

            var sr = new StreamReader(ttxPathfilename);

            xml = XElement.Load(sr);

            var hhea = xml.Descendants("hhea");

            Ascent = (int)hhea.Elements("ascent").Select(x => x.Attribute("value")).First();
            Descent = (int)hhea.Elements("descent").Select(x => x.Attribute("value")).First();

            var post = xml.Descendants("post");
            ItalicAngle = (int)(double)post.Elements("italicAngle").Select(x => x.Attribute("value")).First();

            var name = xml.Descendants("name");
            Name = (string)name.Elements("namerecord").First(x => (int)x.Attribute("nameID") == 1).Value.Trim();

            var hmtx = xml.Descendants("hmtx");

            widths = new List<int>();
            MaxWidth = 0;
            int wSum = 0;
            foreach (var mtx in hmtx.Elements("mtx"))
            {
                var w = (int)mtx.Attribute("width");
                if (w > MaxWidth) MaxWidth = w;
                wSum += w;
                widths.Add(w);
            }

            AvgWidth = wSum / widths.Count;
        }

    };

}