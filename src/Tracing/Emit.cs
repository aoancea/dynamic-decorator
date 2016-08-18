using System;

namespace Dynamic.Decorator.Tracing
{
    public static class Emit
    {
        /*
        public class Foo_Time_Decorator : IFoo
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
        */


        public static Type EmitTimeLogger()
        {
            throw new NotImplementedException();
        }
    }
}