using Xunit;
using System.Dynamic;
using UnitsNet;

namespace SearchAThing.Util.Test;

public class UnitTest1
{

    #region Dynamic

#if NET6_0_OR_GREATER

    [Fact]
    public void DynamicTest1()
    {
        var dynobj = MakeDynamic(("a", 10), ("b", 10.2));

        Assert.True(dynobj.a.GetType() == typeof(int));
        Assert.True(dynobj.b.GetType() == typeof(double));
    }

    [Fact]
    public void DynamicTest2()
    {
        var dynobj = MakeDynamic(("a", 10), ("b", 10.2));

        Assert.True(dynobj.a.GetType() == typeof(int));
        Assert.True(dynobj.b.GetType() == typeof(double));
    }

    [Fact]
    public void DynamicTest3()
    {
        var dynobj = MakeDynamic(("a", 10), ("b", 10.2));
        var dyndict = DynamicMakeDictionary((object)dynobj);
        Assert.True(dyndict.ContainsKey("a"));
        Assert.True((int)dyndict["a"] == 10);
        Assert.True(dyndict.ContainsKey("b"));
        Assert.True((double)dyndict["b"] == 10.2);
    }

    [Fact]
    public void DynamicTest4()
    {
        var dynobj = MakeDynamic(("a", 10), ("b", 10.2));
        var expobj = ToExpando(((object)dynobj));
        Assert.True(expobj.GetType() == typeof(ExpandoObject));
    }

#endif

    #endregion

    #region Numbers
    [Fact]
    public void NumberTest12()
    {
        var a = Length.FromMillimeters(1.3d);
        var b = Length.FromMillimeters(1.299d);

        Assert.True(a.EqualsTol(Length.FromMicrometers(1 + 1e-12), b));
        Assert.False(a.EqualsTol(Length.FromNanometers(1), b));

        var c = Force.FromNewtons(1.299d);

        try
        {
            a.EqualsTol(Length.FromMicrometers(1 + 1e-12), c); // should generate exception due to different unit
            Assert.True(false);
        }
        catch
        {
        }

        var d = Length.FromMillimeters(1.31);

        // 1.3 < 1.31
        Assert.True(a.LessThanTol(Length.FromMicrometers(10), d));

        // 1.31 <= 1.3
        Assert.True(d.LessThanOrEqualsTol(Length.FromMillimeters(0.011), a));
        Assert.False(d.LessThanOrEqualsTol(Length.FromMillimeters(0.009), a));

        // 1.3 > 1.31
        Assert.False(a.GreatThanTol(Length.FromMicrometers(10), d));

        // 1.3 >= 1.31
        Assert.True(a.GreatThanOrEqualsTol(Length.FromMillimeters(0.011), d));
        Assert.False(a.GreatThanOrEqualsTol(Length.FromMillimeters(0.009), d));
    }
    #endregion

}