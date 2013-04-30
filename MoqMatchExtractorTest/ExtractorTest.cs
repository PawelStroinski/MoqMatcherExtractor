using System;
using System.Linq;
using NUnit.Framework;
using MoqMatcherExtractor;
using Moq;

namespace MoqMatcherExtractorTest
{
    public class ExtractorTest
    {
        [Test]
        public void Single()
        {
            var result = Extractor.Extract(() => It.Is<Foo>(f => f.Bar == 10));
            Assert.IsTrue(result(new Foo { Bar = 10 }));
            Assert.IsFalse(result(new Foo { Bar = 5 }));
        }

        [Test]
        public void Multiple()
        {
            var result1 = Extractor.Extract(() => It.Is<Foo>(f => f.Bar == 5 || f.Bar == 1));
            var result2 = Extractor.Extract(() => It.Is<Foo>(f => f.Bar == 10 || f.Bar == 1));
            Assert.IsFalse(result1(new Foo { Bar = 10 }));
            Assert.IsTrue(result2(new Foo { Bar = 10 }));
            Assert.IsTrue(result1(new Foo { Bar = 5 }));
            Assert.IsFalse(result2(new Foo { Bar = 5 }));
        }

        [Test]
        public void ItIsAny()
        {
            var result = Extractor.Extract(() => It.IsAny<Foo>());
            Assert.IsTrue(result(new Foo()));
            Assert.IsTrue(result(null));
        }

        [Test]
        public void Null()
        {
            var exception = Assert.Throws<ArgumentException>(() => Extractor.Extract<Foo>(null));
            Assert.AreEqual("match", exception.Message);
        }

        [Test]
        public void FromMethod()
        {
            var result = Extractor.Extract(Method);
            Assert.IsTrue(result(new Foo { Bar = 10 }));
        }

        public Foo Method()
        {
            return It.Is<Foo>(f => f.Bar == 10);
        }
    }
}
