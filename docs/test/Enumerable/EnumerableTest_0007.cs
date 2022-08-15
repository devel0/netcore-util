using Xunit;
using System.Linq;
using System;
using System.Collections.Generic;

using SearchAThing;

namespace SearchAThing.Util.Tests
{
    public partial class EnumerableTests
    {

        [Fact]
        public void EnumerableTest_0007()
        {

            var effective_en = Enumerable.Range(1, 3);
            var lst = effective_en.ToList();

            Assert.False(effective_en == lst);

            var lst_as_roLst = lst.ToReadOnlyList();
            Assert.True(lst_as_roLst == lst);

            var en_as_roLst = effective_en.ToReadOnlyList();
            Assert.False(en_as_roLst == effective_en);

            Assert.True(en_as_roLst.Count == effective_en.Count());
            int i = 0;
            foreach (var x in effective_en)
            {
                Assert.True(x == en_as_roLst[i]);
                ++i;
            }
        }

    }

}