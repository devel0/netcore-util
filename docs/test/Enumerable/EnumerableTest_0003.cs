using Xunit;
using System.Linq;
using System;
using System.Collections.Generic;

namespace SearchAThing.Util.Tests
{
    public partial class EnumerableTests
    {

        [Fact]
        public void EnumerableTest_0003()
        {
            // verify it works even with 1 element
            {
                var a = new[] { "sample" };

                var cnt = 0;
                foreach (var x in a.WithPrev())
                {
                    Assert.True(x.item == "sample");
                    ++cnt;
                }
                Assert.True(cnt == 1);
            }

            {
                var qq = new[] { new Sample(1), new Sample(2) }.WithPrevNext().ToList();
                Assert.True(qq[0].itemIdx == 0);
                Assert.True(qq[1].itemIdx == 1);
            }

            {
                var a = new[] { new Sample(1), new Sample(2), new Sample(3), new Sample(4), new Sample(5) };

                foreach (var rfe in new[] { false, true })
                {
                    var q = a.WithPrevNext(repeatFirstAtEnd: rfe).ToList();

                    Assert.True(q.Count == 5);
                    Assert.True(q[0].Fn(y => y.item.Value == 1 && y.next.Value == 2 && y.isLast == false && y.itemIdx == 0));
                    Assert.True(q[1].Fn(y => y.item.Value == 2 && y.next.Value == 3 && y.isLast == false && y.itemIdx == 1));
                    Assert.True(q[2].Fn(y => y.item.Value == 3 && y.next.Value == 4 && y.isLast == false && y.itemIdx == 2));
                    Assert.True(q[3].Fn(y => y.item.Value == 4 && y.next.Value == 5 && y.isLast == false && y.itemIdx == 3));

                    if (rfe)
                        Assert.True(q[4].Fn(y => y.item.Value == 5 && y.next.Value == 1 && y.isLast == true && y.itemIdx == 4));
                    else
                        Assert.True(q[4].Fn(y => y.item.Value == 5 && y.next == null && y.isLast == true && y.itemIdx == 4));
                }
            }
        }

    }
}