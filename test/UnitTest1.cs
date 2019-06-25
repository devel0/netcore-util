using System;
using Xunit;
using static System.Math;
using System.Threading;
using System.Globalization;
using System.Linq;
using System.Dynamic;
using System.Collections.ObjectModel;

namespace SearchAThing.Util
{
    public class UnitTest1
    {

        #region Date        

        [Fact]
        public void DateTest1()
        {
            Assert.True(new DateTime(2010, 1, 1).UnspecifiedAsUTCDateTime().Kind == DateTimeKind.Utc);
        }

        #endregion

        #region Dynamic

        [Fact]
        public void DynamicTest1()
        {
            var dynobj = Util.MakeDynamic(("a", 10), ("b", 10.2));

            Assert.True(dynobj.a.GetType() == typeof(int));
            Assert.True(dynobj.b.GetType() == typeof(double));
        }

        [Fact]
        public void DynamicTest2()
        {
            var dynobj = Util.MakeDynamic(("a", 10), ("b", 10.2));

            Assert.True(dynobj.a.GetType() == typeof(int));
            Assert.True(dynobj.b.GetType() == typeof(double));
        }

        [Fact]
        public void DynamicTest3()
        {
            var dynobj = Util.MakeDynamic(("a", 10), ("b", 10.2));
            var dyndict = Util.DynamicMakeDictionary((object)dynobj);
            Assert.True(dyndict.ContainsKey("a"));
            Assert.True((int)dyndict["a"] == 10);
            Assert.True(dyndict.ContainsKey("b"));
            Assert.True((double)dyndict["b"] == 10.2);
        }

        [Fact]
        public void DynamicTest4()
        {
            var dynobj = Util.MakeDynamic(("a", 10), ("b", 10.2));
            var expobj = ((object)dynobj).ToExpando();
            Assert.True(expobj.GetType() == typeof(ExpandoObject));
        }

        #endregion

        #region Number        

        [Fact]
        public void NumberTest1()
        {
            Assert.True(0d.EqualsAutoTol(0d));
            Assert.True((-1d).EqualsAutoTol(-1));

            Assert.False(1.4d.EqualsAutoTol(1.39999));
            Assert.True(1.4d.EqualsAutoTol(1.399999));

            Assert.False(1.4d.EqualsAutoTol(1.40001));
            Assert.True(1.4d.EqualsAutoTol(1.400001));
        }

        [Fact]
        public void NumberTest2()
        {
            Assert.True(1.41d.MRound(.2).EqualsAutoTol(1.4));
            Assert.True(1.59d.MRound(.2).EqualsAutoTol(1.6));
        }

        [Fact]
        public void NumberTest3()
        {
            Assert.True(180d.ToRad().EqualsAutoTol(PI));
            Assert.True(PI.ToDeg().EqualsAutoTol(180));
        }

        [Fact]
        public void NumberTest4()
        {
            Assert.False(1.345.Stringify(2) == "1.35");
            Assert.True(1.346.Stringify(2) == "1.35");
        }

        [Fact]
        public void NumberTest5()
        {
            Assert.True(1.34e-210.Magnitude() == -210);
            Assert.True(1.34e+210.Magnitude() == 210);
        }

        [Fact]
        public void NumberTest6()
        {
            var cultureBackup = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("it");
            Assert.True(double.Parse("1,2").EqualsAutoTol(1.2));
            Assert.True(double.Parse("1.2").EqualsAutoTol(12));

            Assert.True("1,2".InvDoubleParse().EqualsAutoTol(12));
            Assert.True("1.2".InvDoubleParse().EqualsAutoTol(1.2));
            Thread.CurrentThread.CurrentCulture = cultureBackup;
        }

        [Fact]
        public void NumberTest7()
        {
            Assert.True(new[] { 1d, 3.4, 5 }.Mean().EqualsTol(1e-2, 3.13));
        }

        [Fact]
        public void NumberTest8()
        {
            Assert.True(2.031d.ToString(2) == "2.03");
        }

        [Fact]
        public void NumberTest9()
        {
            Assert.True(2.4d.IsInRange(1e-1, "[2,2.4]"));
            Assert.False(2.4d.IsInRange(1e-1, "[2,2.4)"));
            Assert.True(2.4d.IsInRange(1e-1, "[2,)"));
        }

        [Fact]
        public void NumberTest10()
        {
            Assert.True(12.Sign().EqualsAutoTol(1.0));
            Assert.False((-12).Sign().EqualsAutoTol(1.0));
            Assert.True((-12).Sign().EqualsAutoTol(-1.0));

            Assert.True(12.3d.Sign().EqualsAutoTol(1.0));
            Assert.False((-0.45).Sign().EqualsAutoTol(1.0));
            Assert.True((-0.45).Sign().EqualsAutoTol(-1.0));
        }

        #endregion

        #region ObserableCollection

        [Fact]
        public void ObserableCollectionTest1()
        {
            var obc = new ObservableCollection<int>(new[] { 3, 1, 4 });

            var obc1 = obc;
            obc.Sort((x) => x);
            Assert.True(obc == obc1);
            Assert.True(obc.ToList().SequenceEqual(new[] { 1, 3, 4 }));

            var obc2 = obc;
            obc.Sort((x) => x, descending: true);
            Assert.True(obc == obc2);
            Assert.True(obc.ToList().SequenceEqual(new[] { 4, 3, 1 }));
        }
        #endregion

    }

}

