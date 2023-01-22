using UnitsNet;

namespace SearchAThing.Util;

public static partial class Ext
{

    /// <summary>
    /// true if ( |x-y| LTE tol )
    /// </summary>        
    public static bool EqualsTol(this IQuantity x, IQuantity tol, IQuantity y)
    {
        var bu = x.QuantityInfo.BaseUnitInfo.Value;

        return ((double)x.ToUnit(bu).Value).EqualsTol((double)tol.ToUnit(bu).Value, (double)y.ToUnit(bu).Value);
    }

    /// <summary>
    /// true if (x GT y) AND NOT ( |x-y| LTE tol )
    /// </summary>        
    public static bool GreatThanTol(this IQuantity x, IQuantity tol, IQuantity y)
    {
        var bu = x.QuantityInfo.BaseUnitInfo.Value;

        return ((double)x.ToUnit(bu).Value).GreatThanTol((double)tol.ToUnit(bu).Value, (double)y.ToUnit(bu).Value);
    }

    /// <summary>
    /// true if (x GT y) AND ( |x-y| LTE tol )
    /// </summary>     
    public static bool GreatThanOrEqualsTol(this IQuantity x, IQuantity tol, IQuantity y)
    {
        var bu = x.QuantityInfo.BaseUnitInfo.Value;

        return ((double)x.ToUnit(bu).Value).GreatThanOrEqualsTol((double)tol.ToUnit(bu).Value, (double)y.ToUnit(bu).Value);
    }

    /// <summary>
    /// true if (x LT y) AND NOT ( |x-y| LTE tol )
    /// </summary>     
    public static bool LessThanTol(this IQuantity x, IQuantity tol, IQuantity y)
    {
        var bu = x.QuantityInfo.BaseUnitInfo.Value;

        return ((double)x.ToUnit(bu).Value).LessThanTol((double)tol.ToUnit(bu).Value, (double)y.ToUnit(bu).Value);
    }

    /// <summary>
    /// true if (x LT y) AND ( |x-y| LTE tol )
    /// </summary>     
    public static bool LessThanOrEqualsTol(this IQuantity x, IQuantity tol, IQuantity y)
    {
        var bu = x.QuantityInfo.BaseUnitInfo.Value;

        return ((double)x.ToUnit(bu).Value).LessThanOrEqualsTol((double)tol.ToUnit(bu).Value, (double)y.ToUnit(bu).Value);
    }

    public static int CompareTol(this IQuantity x, IQuantity tol, IQuantity y)
    {
        var bu = x.QuantityInfo.BaseUnitInfo.Value;

        return ((double)x.ToUnit(bu).Value).CompareTol((double)tol.ToUnit(bu).Value, (double)y.ToUnit(bu).Value);
    }

}