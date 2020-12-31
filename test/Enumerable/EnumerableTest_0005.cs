using Xunit;
using System.Linq;
using System;
using System.Collections.Generic;

namespace SearchAThing.Util.Tests
{
    public partial class EnumerableTests
    {

        [Fact]
        public void EnumerableTest_0005()
        {
            var q = new[] { 1, 2, 4 };

            var q2 = q.WithIndexIsLast().Select(w => w.isLast ? 0 : w.item).Sum();

            Assert.True(q2 == 3);
        }

    }
}