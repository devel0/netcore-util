using System;
using Xunit;

namespace SearchAThing.NETCoreUtil
{
    public class UnitTest1
    {
        [Fact]
        public void DateTest1()
        {
            Assert.True(new DateTime(2010, 1, 1).UnspecifiedAsUTCDateTime().Kind == DateTimeKind.Utc);
        }

        [Fact]
        public void DynamicTest1()
        {
            var dynobj = Util.MakeDynamic(("a", 10), ("b", 10.2));

            Assert.True(dynobj.a.GetType() == typeof(int));
            Assert.True(dynobj.b.GetType() == typeof(double));
        }

    }
}

