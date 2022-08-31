using System;
using static System.Math;

namespace SearchAThing
{

    /// <summary>
    /// standard paper sizes
    /// </summary>
    public enum PaperSizeEnum
    {

        /// <summary>
        /// 841 x 1189 mm
        /// </summary>
        A0,

        /// <summary>
        /// 594 x 841 mm
        /// </summary>
        A1,

        /// <summary>
        /// 420 x 594 mm
        /// </summary>
        A2,

        /// <summary>
        /// 297 x 420 mm
        /// </summary>
        A3,

        /// <summary>
        /// 210 x 297 mm
        /// </summary>
        A4,

        /// <summary>
        /// 148.5 x 210 mm
        /// </summary>
        A5,

        /// <summary>
        /// 105 x 148.5 mm
        /// </summary>
        A6,

        /// <summary>
        /// 74 x 105 mm
        /// </summary>
        A7,

        /// <summary>
        /// 52 x 74 mm
        /// </summary>
        A8,

        /// <summary>
        /// 37 x 52 mm
        /// </summary>
        A9,

        /// <summary>
        /// 26 x 37 mm
        /// </summary>
        A10,

        /// <summary>
        /// custom width x height mm
        /// </summary>
        Custom

    }

    /// <summary>
    /// hold paper size info
    /// </summary>
    public class PaperSize
    {

        #region predefined paper size objects

        /// <summary>
        /// 841 x 1189 mm
        /// </summary>
        public static readonly PaperSize A0 = new PaperSize(PaperSizeEnum.A0);

        /// <summary>
        /// 594 x 841 mm
        /// </summary>
        public static readonly PaperSize A1 = new PaperSize(PaperSizeEnum.A1);

        /// <summary>
        /// 420 x 594 mm
        /// </summary>
        public static readonly PaperSize A2 = new PaperSize(PaperSizeEnum.A2);

        /// <summary>
        /// 297 x 420 mm
        /// </summary>
        public static readonly PaperSize A3 = new PaperSize(PaperSizeEnum.A3);

        /// <summary>
        /// 210 x 297 mm
        /// </summary>
        public static readonly PaperSize A4 = new PaperSize(PaperSizeEnum.A4);

        /// <summary>
        /// 148.5 x 210 mm
        /// </summary>
        public static readonly PaperSize A5 = new PaperSize(PaperSizeEnum.A5);

        /// <summary>
        /// 105 x 148.5 mm
        /// </summary>
        public static readonly PaperSize A6 = new PaperSize(PaperSizeEnum.A6);

        /// <summary>
        /// 74 x 105 mm
        /// </summary>
        public static readonly PaperSize A7 = new PaperSize(PaperSizeEnum.A7);

        /// <summary>
        /// 52 x 74 mm
        /// </summary>
        public static readonly PaperSize A8 = new PaperSize(PaperSizeEnum.A8);

        /// <summary>
        /// 37 x 52 mm
        /// </summary>
        public static readonly PaperSize A9 = new PaperSize(PaperSizeEnum.A9);

        /// <summary>
        /// 26 x 37 mm
        /// </summary>
        public static readonly PaperSize A10 = new PaperSize(PaperSizeEnum.A10);

        #endregion

        /// <summary>
        /// paper width [mm]
        /// </summary>        
        public double WidthMM { get; private set; }

        /// <summary>
        /// paper height[mm]
        /// </summary>
        public double HeightMM { get; private set; }

        /// <summary>
        /// peper type
        /// </summary>        
        public PaperSizeEnum Type { get; private set; }

        /// <summary>
        /// paper size by given type
        /// </summary>
        public PaperSize(PaperSizeEnum type)
        {
            Type = type;

            switch (type)
            {
                case PaperSizeEnum.A0: WidthMM = 841; HeightMM = 1189; break;
                case PaperSizeEnum.A1: WidthMM = 594; HeightMM = 841; break;
                case PaperSizeEnum.A2: WidthMM = 420; HeightMM = 594; break;
                case PaperSizeEnum.A3: WidthMM = 297; HeightMM = 420; break;
                case PaperSizeEnum.A4: WidthMM = 210; HeightMM = 297; break;
                case PaperSizeEnum.A5: WidthMM = 148.5; HeightMM = 210; break;
                case PaperSizeEnum.A6: WidthMM = 105; HeightMM = 148.5; break;
                case PaperSizeEnum.A7: WidthMM = 74; HeightMM = 105; break;
                case PaperSizeEnum.A8: WidthMM = 52; HeightMM = 74; break;
                case PaperSizeEnum.A9: WidthMM = 37; HeightMM = 52; break;
                case PaperSizeEnum.A10: WidthMM = 26; HeightMM = 37; break;
                case PaperSizeEnum.Custom: throw new ArgumentException($"use (width, height) constructor for custom type");

                default: throw new ArgumentException($"unknown paper type [{type}]");
            }
        }

        /// <summary>
        /// custom paper size
        /// </summary>
        public PaperSize(double widthMM, double heightMM)
        {
            Type = PaperSizeEnum.Custom;

            WidthMM = widthMM;
            HeightMM = heightMM;
        }

    }

    public static partial class UtilExt
    {

        /// <summary>
        /// font size<br/>
        /// half_point = hp = 1/144 inch = 25.4/144 mm<br/>
        /// pt = hp/2<br/>
        /// pt = 2*25.4/144*mm
        /// </summary>        
        public static double MMToPt(this double mm) => 2 * 25.4 / 144 * mm;

        /// <summary>
        /// font size<br/>
        /// half_point = hp = 1/144 inch = 25.4/144 mm<br/>
        /// pt = hp/2<br/>        
        /// mm = 144/(2*25.4) pt<br/>        
        /// </summary>        
        public static double PtToMM(this double pt) => 144 / (2 * 25.4) * pt;

        /// <summary>
        /// font size<br/>
        /// hp = 2*pt
        /// </summary>        
        public static double PtToHalfPoint(this double pt) => 2 * pt;

        /// <summary>
        /// convert mm to twip<br/>
        /// twip = 1/20 pp = 1/20 * 1/72 in = 1/1440 in = 1/1440 * 25.4 mm<br/>
        /// mm = 1440/25.4 twip
        /// </summary>        
        public static int MMToTwip(this double mm)
        {
            return (int)Round(1440d / 25.4 * mm);
        }

        /// <summary>
        /// convert mm to twip<br/>
        /// twip = 1/20 pp = 1/20 * 1/72 in = 1/1440 in = 1/1440 * 25.4 mm<br/>
        /// mm = 1440/25.4 twip
        /// </summary>        
        public static int MMToTwip(this int mm)
        {
            return ((double)mm).MMToTwip();
        }

        /// <summary>        
        /// 10mm = 360000 EMU<br/>
        /// mm = 36000 EMU
        /// </summary>
        public static int MMToEMU(this int mm)
        {
            return mm * 36000;
        }

        /// <summary>        
        /// 10mm = 360000 EMU<br/>
        /// mm = 36000 EMU
        /// </summary>
        public static int MMToEMU(this double mm)
        {
            return (int)Round(mm * 36000, 0);
        }

        /// <summary>
        /// convert twip to mm<br/>
        /// mm = 1440/25.4 twip<br/>
        /// twip = 25.4/1440 mm
        /// </summary>
        public static double TwipToMM(this uint twip)
        {
            return 25.4 / 1440 * twip;
        }

        /// <summary>
        /// convert twip to mm<br/>
        /// mm = 1440/25.4 twip<br/>
        /// twip = 25.4/1440 mm
        /// </summary>
        public static double TwipToMM(this int twip)
        {
            return ((uint)twip).TwipToMM();
        }

        /// <summary>
        /// convert given percent 0..100 to fiftieths of a Percent
        /// </summary>
        public static int Pct(this double percent)
        {            
            return (int)(percent * 50);
        }

    }

}