using System.Diagnostics;

namespace Dynamic.Decorator.Tracing
{
    public interface ILogger
    {
        void Log(Stopwatch sw);
    }
}