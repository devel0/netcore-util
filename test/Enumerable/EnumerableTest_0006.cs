using Xunit;
using System.Linq;
using System;
using System.Collections.Generic;

using SearchAThing;

namespace SearchAThing.Util.Tests
{
    public partial class EnumerableTests
    {

        public class SampleData
        {
            public int data { get; set; }
        };

        [Fact]
        public void EnumerableTest_0006()
        {
            {
                var q = (IEnumerable<int>)(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

                var newset = q.RouteFirst(4).ToList();

                Assert.True(newset.Count == q.Count());

                Assert.True(newset[0] == 4);
                Assert.True(newset[1] == 5);
                Assert.True(newset[2] == 6);
                Assert.True(newset[3] == 7);
                Assert.True(newset[4] == 8);
                Assert.True(newset[5] == 9);
                Assert.True(newset[6] == 10);
                Assert.True(newset[7] == 1);
                Assert.True(newset[8] == 2);
                Assert.True(newset[9] == 3);
            }

            {
                var q = new List<SampleData>()
                {
                    new SampleData { data = 1 },
                    new SampleData { data = 2 },
                    new SampleData { data = 3 }, // q[2]
                    new SampleData { data = 4 },
                };

                var newset = q.RouteFirst(q[2]).ToList();

                Assert.True(newset.Count == q.Count);

                Assert.True(newset[0].data == 3);
                Assert.True(newset[1].data == 4);
                Assert.True(newset[2].data == 1);
                Assert.True(newset[3].data == 2);
            }
        }

    }
}