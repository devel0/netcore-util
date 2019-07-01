using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace SearchAThing.Util
{

    public static partial class Toolkit
    {

        /// <summary>
        /// deflate embedded resource
        /// </summary>
        public static Stream DeflateEmbeddedResource(Assembly assembly, string resource)
        {
            using (var s = assembly.GetManifestResourceStream(resource))
            {
                var ms = new MemoryStream();
                using (var ds = new DeflateStream(s, CompressionMode.Decompress, true))
                {
                    var buf = new byte[4096];
                    int read;
                    while ((read = ds.Read(buf, 0, buf.Length)) != 0)
                        ms.Write(buf, 0, read);
                }
                ms.Seek(0, SeekOrigin.Begin);
                return ms;
            }
        }

        /// <summary>
        /// save given embedded resource to file
        /// </summary>
        public static void SaveEmbeddedResourceToFile(string resource, string dstPathfilename, bool deflate = false)
        {
            var assembly = Assembly.GetCallingAssembly();
            if (deflate)
            {
                using (var fs = DeflateEmbeddedResource(assembly, resource))
                {
                    using (var dstfs = new FileStream(dstPathfilename, FileMode.Create))
                    {
                        fs.CopyTo(dstfs);
                    }
                }
            }
            else
            {
                using (var s = assembly.GetManifestResourceStream(resource))
                {
                    using (var dstfs = new FileStream(dstPathfilename, FileMode.Create))
                    {
                        s.CopyTo(dstfs);
                    }
                }
            }
        }

        /// <summary>
        /// retrieve current executing assembly resource names
        /// </summary>
        public static IEnumerable<string> GetEmbeddedResourceNames()
        {
            var assembly = Assembly.GetCallingAssembly();
            return assembly.GetManifestResourceNames();
        }

    }

}