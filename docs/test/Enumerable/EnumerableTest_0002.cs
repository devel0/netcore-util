using Xunit;
using System.Linq;
using System;
using System.Collections.Generic;

namespace SearchAThing.Util.Tests
{
    public partial class EnumerableTests
    {

        [Fact]
        public void EnumerableTest_0002()
        {
            // verify it works even with 1 element
            {
                var a = new[] { 1 };

                var cnt = 0;
                foreach (var x in a.WithNextPrimitive())
                {
                    Assert.True(x.item == 1);
                    ++cnt;
                }
                Assert.True(cnt == 1);

                cnt = 0;
                foreach (var x in a.WithNextPrimitive(repeatFirstAtEnd: true))
                {
                    Assert.True(x.item == 1 && x.next == 1 && x.itemIdx == 0);
                    ++cnt;
                }
                Assert.True(cnt == 1);
            }

            {
                var qq = new[] { 1, 2 }.WithNextPrimitive().ToList();
                Assert.True(qq[0].itemIdx == 0);
                Assert.True(qq[1].itemIdx == 1);
            }

            {
                var a = new[] { 1, 2, 3, 4, 5 };

                foreach (var rfe in new[] { false, true })
                {
                    var q = a.WithNextPrimitive(repeatFirstAtEnd: rfe).ToList();

                    Assert.True(q.Count == 5);
                    Assert.True(q[0].Eval(y => y.item == 1 && y.next.Value == 2 && y.isLast == false && y.itemIdx == 0));
                    Assert.True(q[1].Eval(y => y.item == 2 && y.next.Value == 3 && y.isLast == false && y.itemIdx == 1));
                    Assert.True(q[2].Eval(y => y.item == 3 && y.next.Value == 4 && y.isLast == false && y.itemIdx == 2));
                    Assert.True(q[3].Eval(y => y.item == 4 && y.next.Value == 5 && y.isLast == false && y.itemIdx == 3));

                    if (rfe)
                        Assert.True(q[4].Eval(y => y.item == 5 && y.next.Value == 1 && y.isLast == true && y.itemIdx == 4));
                    else
                        Assert.True(q[4].Eval(y => y.item == 5 && y.next == null && y.isLast == true && y.itemIdx == 4));
                }
            }
        }

    }
}