using BenchmarkDotNet.Running;
using Stronk.Benchmarks.PropertyWriters;

namespace Stronk.Benchmarks
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			BenchmarkRunner.Run<DiscoveryBenchmarks>();
		}
	}
}
