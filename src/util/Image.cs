using ImageMagick;

namespace SearchAThing.Util;

public static partial class Toolkit
{

    /// <summary>
    /// retrieve image nfo
    /// </summary>
    public static MagickImageInfo GetImageNfo(string pathfilename) => new MagickImageInfo(pathfilename);

    /// <summary>
    /// retrieve image nfo
    /// </summary>
    public static MagickImageInfo GetImageNfoFromStream(Stream stream) => new MagickImageInfo(stream);

    /// <summary>
    /// retrieve image size width (mm) x height (mm);
    /// if no density info present in image defaultDpi argument will used
    /// </summary>
    /// <param name="nfo">input magick image nfo</param>        
    /// <param name="defaultDpi">used if image density info missing</param>              
    public static (double widthMM, double heightMM) ImageSizeMM(this MagickImageInfo nfo, double defaultDpi = 96)
    {
        var width = (double)nfo.Width;
        var height = (double)nfo.Height;

        var dpix = defaultDpi;
        var dpiy = defaultDpi;

        if (nfo.Density is not null)
        {
            switch (nfo.Density.Units)
            {
                case DensityUnit.PixelsPerCentimeter:
                    {
                        dpix = nfo.Density.X * 2.54;
                        dpiy = nfo.Density.Y * 2.54;
                    }
                    break;
            }
        }

        return (width / dpix * 25.4, height / dpiy * 25.4);
    }

}