using System.Linq;
using BenchmarkDotNet.Attributes;
using Stronk.PropertyWriters;
using Stronk.Tests.PropertyWriters;

namespace Stronk.Benchmarks
{
	public class PropertyWriterBenchmarks
	{
		private PropertyDescriptor[] _backingFields;
		private PropertyDescriptor[] _setters;

		[GlobalSetup]
		public void Setup()
		{
			_backingFields = new BackingFieldPropertyWriter()
				.Select(new PropertyWriterArgs((message, args) => { }, typeof(MassiveBackingFieldConfig)))
				.ToArray();

			_setters = new PrivateSetterPropertyWriter()
				.Select(new PropertyWriterArgs((message, args) => { }, typeof(MassiveConfig)))
				.ToArray();
		}

		[Benchmark]
		public void Writing_to_many_backing_fields()
		{
			var target = new MassiveBackingFieldConfig();

			foreach (var property in _backingFields)
				property.Assign(target, 1234);
		}

		[Benchmark]
		public void Writing_to_many_setters()
		{
			var target = new MassiveConfig();

			foreach (var property in _setters)
				property.Assign(target, 1234);
		}
	}
}
