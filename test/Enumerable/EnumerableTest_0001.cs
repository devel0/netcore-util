using Xunit;
using System.Linq;
using System;
using System.Collections.Generic;

namespace SearchAThing.Util.Tests
{
    public partial class EnumerableTests
    {

        public class Sample
        {
            public Sample(int v) { this.Value = v; }
            public int Value { get; private set; }
            public override string ToString() => Value.ToString();
        }

        [Fact]
        public void EnumerableTest_0001()
        {
            // verify it works even with 1 element
            {
                var a = new[] { "sample" };

                var cnt = 0;
                foreach (var x in a.WithNext())
                {
                    Assert.True(x.item == "sample");
                    ++cnt;
                }
                Assert.True(cnt == 1);

                cnt = 0;
                foreach (var x in a.WithNext(repeatFirstAtEnd: true))
                {
                    Assert.True(x.item == "sample" && x.next == "sample");
                    ++cnt;
                }
                Assert.True(cnt == 1);
            }

            {
                var a = new[] { new Sample(1), new Sample(2), new Sample(3), new Sample(4), new Sample(5) };

                foreach (var rfe in new[] { false, true })
                {
                    var q = a.WithNext(repeatFirstAtEnd: rfe).ToList();

                    Assert.True(q.Count == 5);
                    Assert.True(q[0].Eval(y => y.item.Value == 1 && y.next.Value == 2 && y.isLast == false));
                    Assert.True(q[1].Eval(y => y.item.Value == 2 && y.next.Value == 3 && y.isLast == false));
                    Assert.True(q[2].Eval(y => y.item.Value == 3 && y.next.Value == 4 && y.isLast == false));
                    Assert.True(q[3].Eval(y => y.item.Value == 4 && y.next.Value == 5 && y.isLast == false));

                    if (rfe)
                        Assert.True(q[4].Eval(y => y.item.Value == 5 && y.next.Value == 1 && y.isLast == true));
                    else
                        Assert.True(q[4].Eval(y => y.item.Value == 5 && y.next == null && y.isLast == true));
                }
            }
        }

    }
}