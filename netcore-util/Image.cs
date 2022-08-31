using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ImageMagick;

namespace SearchAThing
{


    public static partial class UtilToolkit
    {
        
        /// <summary>
        /// retrieve image nfo
        /// </summary>
        public static MagickImageInfo GetImageNfo(string pathfilename) => new MagickImageInfo(pathfilename);
        
    }

}