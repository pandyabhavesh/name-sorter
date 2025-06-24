using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace name_sorter.banchmarks
{
    public abstract class BenchmarkWithDi
    {
        protected IServiceProvider Services => Program.GetServices();

        protected T Get<T>() where T : notnull =>
            Services.GetRequiredService<T>();

        [GlobalCleanup]
        public virtual void Cleanup()
        {
            // Clean up afterwards
            if (Services is IDisposable d)
                d.Dispose();
        }
    }
}
