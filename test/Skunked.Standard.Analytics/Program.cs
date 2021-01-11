using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Skunked.Benchmarks
{
    class Program
    {
        static void Main(string[] args) =>
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).RunAll(new DebugBuildConfig());
    }
}