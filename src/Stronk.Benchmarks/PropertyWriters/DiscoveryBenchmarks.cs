using System.Linq;
using BenchmarkDotNet.Attributes;
using Stronk.Benchmarks.Configs;
using Stronk.PropertyWriters;

namespace Stronk.Benchmarks.PropertyWriters
{
	public class DiscoveryBenchmarks
	{
		[Benchmark]
		public PropertyDescriptor[] SetterDiscovery()
		{
			return new PrivateSetterPropertyWriter()
				.Select(new PropertyWriterArgs((message, args) => { }, typeof(MassiveConfig)))
				.ToArray();
		}

		[Benchmark]
		public PropertyDescriptor[] BackingFieldDiscovery()
		{
			return new BackingFieldPropertyWriter()
				.Select(new PropertyWriterArgs((message, args) => { }, typeof(MassiveBackingFieldConfig)))
				.ToArray();
		}
	}
}
