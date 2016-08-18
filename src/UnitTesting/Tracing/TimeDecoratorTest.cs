using Dynamic.Decorator.Tracing;
using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics;

namespace Dynamic.Decorator.UnitTesting.Tracing
{
    [TestFixture]
    public class TimeDecoratorTest
    {
        private Mock<ILogger> loggerMock;
        private Mock<IFoo> fooMock;

        private Foo_Time_Decorator foo_Time_Decorator;

        [SetUp]
        public void SetUp()
        {
            loggerMock = new Mock<ILogger>(MockBehavior.Strict);
            fooMock = new Mock<IFoo>(MockBehavior.Strict);

            foo_Time_Decorator = new Foo_Time_Decorator(fooMock.Object, loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            loggerMock.VerifyAll();
            fooMock.VerifyAll();
        }

        [Test]
        public void Run_ApplyDecorator_DecoratedIsCalledAndTimeIsLogged()
        {
            fooMock.Setup(x => x.Bar("run")).Returns("run").Verifiable();

            loggerMock.Setup(x => x.Log(It.IsAny<Stopwatch>())).Verifiable();

            string result = foo_Time_Decorator.Bar("run");

            Assert.AreEqual("run", result);
        }
    }


    public interface IFoo
    {
        string Bar(string text);
    }

    internal class Foo : IFoo
    {
        public string Bar(string text)
        {
            return text;
        }
    }

    internal class Foo_Time_Decorator : IFoo
    {
        private readonly IFoo decorated;
        private readonly ILogger logger;

        public Foo_Time_Decorator(IFoo decorated, ILogger logger)
        {
            this.decorated = decorated;
            this.logger = logger;
        }

        public string Bar(string text)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string result = decorated.Bar(text);

            sw.Stop();

            logger.Log(sw);

            return result;
        }
    }

    internal class ConsoleLogger : ILogger
    {
        public void Log(Stopwatch sw)
        {
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}