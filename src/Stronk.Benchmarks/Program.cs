using BenchmarkDotNet.Running;

namespace Stronk.Benchmarks
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			BenchmarkRunner.Run<PropertyWriterBenchmarks>();
		}
	}
}
