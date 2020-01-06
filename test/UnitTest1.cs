using System;
using Xunit;
using static System.Math;
using System.Threading;
using System.Globalization;
using System.Linq;
using System.Dynamic;
using System.Collections.ObjectModel;
using static SearchAThing.Util.Toolkit;
using System.Linq.Expressions;
using static SearchAThing.UtilExt;

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

        [Fact]
        public void NumberTest11()
        {
            Assert.True("1.2".SmartDoubleParse().EqualsAutoTol(1.2));
            Assert.True("1,2".SmartDoubleParse().EqualsAutoTol(1.2));
            var exceptionCnt = 0;
            try
            {
                "1,200.40".SmartDoubleParse();
            }
            catch
            {
                ++exceptionCnt;
            }
            Assert.True(exceptionCnt == 1);
        }

        [Fact]
        public void NumberTest12()
        {
            Assert.True(111111111111111d.PercentDifference(111111011111111d) == 9.0000081000072986E-07);
            Assert.True(111111111111111d.PercentDifference(111111111111111d) == 0);
            Assert.True(111111111111111d.PercentDifference(191111111111111d) == 0.72000000000000075);
            Assert.True(111111111111111d.PercentDifference(111111111111119d) == 7.2000000000000072E-14);
            Assert.True(1.23e-210.PercentDifference(1.23001e-210) == 8.1300813007679046E-06);
            Assert.True(21.23e-210.PercentDifference(1.23001e-210) == 16.260022276241656);

            Assert.True((-5327234122.34).PercentDifference(-5327234122.341) == 1.8761155613613974E-13);
            Assert.True((+5327234122.34).PercentDifference(+5327234122.341) == 1.8761155613613974E-13);
            Assert.True((+5327234122.34).PercentDifference(-5327234122.341) == 2.0000000000001874);
            Assert.True((-5327234122.34).PercentDifference(+5327234122.341) == 2.0000000000001874);

            Assert.True(1234567890123d.PercentDifference(1234567890023) == 8.1000000735590979E-11);

            Assert.True(double.IsNaN(0d.PercentDifference(2.2)));
            Assert.True(double.IsNaN(2.2.PercentDifference(0d)));
            Assert.True(0d.PercentDifference(0d) == 0);
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

        #region Expression
        [Fact]
        public void GetMembersTest()
        {
            var obj = new MemberTest1();

            {
                var lst = obj.GetMemberNames(x => x.a);
                Assert.True(lst.Count == 1);
                Assert.True(lst.Contains("a"));
            }

            {
                var lst = obj.GetMemberNames(x => new { x.a, x.b });
                Assert.True(lst.Count == 2);
                Assert.True(lst.Contains("a") && lst.Contains("b"));
                lst = GetMemberNames<MemberTest1>(x => new { x.a, x.b }).ToHashSet();
                Assert.True(lst.Count == 2);
                Assert.True(lst.Contains("a") && lst.Contains("b"));
            }

            {
                var res = obj.GetMemberName(x => x.b);
                Assert.True(res == "b");
                res = GetMemberName<MemberTest1>(x => x.b);
                Assert.True(res == "b");
            }

            {
                Assert.Throws<ArgumentException>(() =>
                {
                    var res = obj.GetMemberName(x => new { x.a, x.b });
                });
            }
        }

        void CreateGetterSetterTestFn<T>(T dst, T src, Expression<Func<T, object>> memberExpr)
        {
            var (getter, setter) = memberExpr.CreateGetterSetter();
            setter(dst, getter(src));
        }

        [Fact]
        public void CreateGetterSetterTest()
        {
            var src = new MemberTest1 { a = 1, b = 2 };
            var dst = new MemberTest1();

            Assert.False(dst.a == src.a);
            CreateGetterSetterTestFn<MemberTest1>(dst, src, (x) => x.a);
            Assert.True(dst.a == src.a);
        }
        #endregion

    }

    public class MemberTest1
    {
        public int a { get; set; }
        public int b { get; set; }
    }

}

