using System;
using System.Linq;
using NUnit.Framework;
using MoqMatcherExtractor;
using Moq;

namespace MoqMatcherExtractorTest
{
    public class MatcherExtractorTest
    {
        [Test]
        public void Single()
        {
            var result = MatcherExtractor.Extract(() => It.Is<Foo>(f => f.Bar == 10));
            Assert.IsTrue(result(new Foo { Bar = 10 }));
            Assert.IsFalse(result(new Foo { Bar = 5 }));
        }

        [Test]
        public void Multiple()
        {
            var result1 = MatcherExtractor.Extract(() => It.Is<Foo>(f => f.Bar == 5 || f.Bar == 1));
            var result2 = MatcherExtractor.Extract(() => It.Is<Foo>(f => f.Bar == 10 || f.Bar == 1));
            Assert.IsFalse(result1(new Foo { Bar = 10 }));
            Assert.IsTrue(result2(new Foo { Bar = 10 }));
            Assert.IsTrue(result1(new Foo { Bar = 5 }));
            Assert.IsFalse(result2(new Foo { Bar = 5 }));
        }

        [Test]
        public void ItIsAny()
        {
            var result = MatcherExtractor.Extract(() => It.IsAny<Foo>());
            Assert.IsTrue(result(new Foo()));
            Assert.IsTrue(result(null));
        }

        [Test]
        public void Null()
        {
            var exception = Assert.Throws<ArgumentException>(() => MatcherExtractor.Extract<Foo>(null));
            Assert.AreEqual("match", exception.Message);
        }

        [Test]
        public void FromMethod()
        {
            var result = MatcherExtractor.Extract(Method);
            Assert.IsTrue(result(new Foo { Bar = 10 }));
        }

        public Foo Method()
        {
            return It.Is<Foo>(f => f.Bar == 10);
        }
    }
}
