using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace SearchAThing
{

    public static partial class UtilExt
    {

        /// <summary>
        /// retrieve embedded resource file content and read into a string
        /// </summary>
        /// <param name="resourceName">name of resource (eg. namespace.filename.ext)</param>
        /// <typeparam name="T">Type for which lookup assembly (eg. namespace.classname)</typeparam>
        public static string GetEmbeddedFileContent<T>(this string resourceName) where T : class
        {
            var assembly = typeof(T).GetTypeInfo().Assembly;
            string res = "";
            using (var resource = assembly.GetManifestResourceStream(resourceName))
            {
                if (resource != null)
                    using (var sr = new StreamReader(resource))
                    {
                        res = sr.ReadToEnd();
                    }
            }
            return res;
        }

    }

    public static partial class UtilToolkit
    {

        /// <summary>
        /// deflate embedded resource
        /// </summary>
        public static Stream? DeflateEmbeddedResource(Assembly assembly, string resource)
        {
            using (var s = assembly.GetManifestResourceStream(resource))
            {
                if (s != null)
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

            return null;
        }

        /// <summary>
        /// save given embedded resource to file
        /// </summary>
        public static bool SaveEmbeddedResourceToFile(string resource, string dstPathfilename, bool deflate = false)
        {
            var assembly = Assembly.GetCallingAssembly();
            if (deflate)
            {
                using (var fs = DeflateEmbeddedResource(assembly, resource))
                {
                    if (fs == null) return false;

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
                    if (s == null) return false;

                    using (var dstfs = new FileStream(dstPathfilename, FileMode.Create))
                    {
                        s.CopyTo(dstfs);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// retrieve current executing assembly resource names
        /// </summary>
        public static IEnumerable<string> GetEmbeddedResourceNames()
        {
            var assembly = Assembly.GetCallingAssembly();
            return assembly.GetManifestResourceNames();
        }

        /// <summary>
        /// retrieve the list of embedded resource names from given Type
        /// </summary>
        /// <typeparam name="T">Type for which lookup assembly (eg. namespace.classname)</typeparam>
        public static string[] GetEmbeddedResourcesList<T>() where T : class
        {
            var assembly = typeof(T).GetTypeInfo().Assembly;
            return assembly.GetManifestResourceNames();
        }

    }

}