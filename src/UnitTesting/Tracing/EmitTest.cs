using Dynamic.Decorator.Tracing;
using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Dynamic.Decorator.UnitTesting.Tracing
{
    [TestFixture]
    public class EmitTest
    {
        private Mock<ILogger> loggerMock;
        private Mock<IFoo> fooMock;

        [SetUp]
        public void SetUp()
        {
            loggerMock = new Mock<ILogger>(MockBehavior.Strict);
            fooMock = new Mock<IFoo>(MockBehavior.Strict);
        }

        [TearDown]
        public void TearDown()
        {
            loggerMock.VerifyAll();
            fooMock.VerifyAll();
        }

        [Test]
        public void EmitTimeLogger_EmitTypeAndInstantiate_TypeGetsInstantiated()
        {
            Type fooType = Emit.EmitTimeLogger<IFoo>();

            ConstructorInfo ctor = fooType.GetConstructor(new Type[] { typeof(IFoo), typeof(ILogger) });

            object customFoo = ctor.Invoke(new object[] { fooMock.Object, loggerMock.Object });

            Assert.IsNotNull(customFoo);
            Assert.AreEqual(fooMock.Object, customFoo.GetType().GetField("foo").GetValue(customFoo));
            Assert.AreEqual(loggerMock.Object, customFoo.GetType().GetField("logger").GetValue(customFoo));
        }
    }

    internal class Logger : ILogger
    {
        public void Log(Stopwatch sw)
        {
            throw new NotImplementedException();
        }
    }
}