using System;
using System.IO;
using System.Reflection;

namespace SearchAThing
{
    public static partial class UtilToolkit
    {

        /// <summary>
        /// {AppData}/{namespace}/{assembly_name}
        /// </summary>
        public static string AppDataFolder(string ns) => EnsureFolder(System.IO.Path.Combine(
            System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ns),
            Assembly.GetCallingAssembly().GetName().Name));            
            
        /// <summary>
        /// Ensure given folder path exists.
        /// </summary>        
        public static string EnsureFolder(this string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// Search given filename in the PATH
        /// </summary>
        /// <returns>null if not found</returns>
        public static string? SearchInPath(this string filename)
        {
            if (File.Exists(filename)) return System.IO.Path.GetFullPath(filename);

            var paths = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in paths.Split(System.IO.Path.PathSeparator))
            {
                var pathname = System.IO.Path.Combine(path, filename);
                if (File.Exists(pathname)) return pathname;
            }
            return null;
        }

    }

}