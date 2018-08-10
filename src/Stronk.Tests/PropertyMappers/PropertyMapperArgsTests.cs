using System;
using System.Collections.Generic;
using Shouldly;
using Stronk.ConfigurationSources;
using Stronk.PropertyMappers;
using Stronk.PropertyWriters;
using Xunit;

namespace Stronk.Tests.PropertyMappers
{
	public class PropertyMapperArgsTests
	{
		private readonly List<IConfigurationSource> _sources;
		private readonly PropertyMapperArgs _args;

		private readonly string _key;
		private readonly string _value;

		public PropertyMapperArgsTests()
		{
			_key = Guid.NewGuid().ToString();
			_value = Guid.NewGuid().ToString();

			_sources = new List<IConfigurationSource>
			{
				new DictionarySource(new Dictionary<string, string> { { _key, _value } })
			};

			_args = new PropertyMapperArgs(
				(template, values) => { },
				_sources,
				new TestDescriptor("RabbitMqTestUsername", typeof(string)));
		}

		[Fact]
		public void When_fetching_a_value_by_null_key()
		{
			_args.GetValue((string) null).ShouldBeNull();
		}

		[Fact]
		public void When_fetching_a_value_by_key()
		{
			_args.GetValue(_key).ShouldBe(_value);
		}

		[Fact]
		public void When_fetching_a_value_by_non_existing_key()
		{
			_args.GetValue(Guid.NewGuid().ToString()).ShouldBeNull();
		}

		[Fact]
		public void When_fetching_a_value_by_matching_on_key()
		{
			_args.GetValue(name => name == _key).ShouldBe(_value);
		}

		[Fact]
		public void When_fetching_a_value_by_matching_on_key_which_doesnt_exist()
		{
			var searchKey = Guid.NewGuid().ToString();
			_args.GetValue(name => name == searchKey).ShouldBeNull();
		}

		[Fact]
		public void When_fetching_a_value_by_matching_on_key_and_there_are_multiple_matches()
		{
			_sources.Add(new DictionarySource(new Dictionary<string, string> { { _key, Guid.NewGuid().ToString() } }));

			_args.GetValue(name => name == _key).ShouldBe(_value);
		}

		private class TestDescriptor : PropertyDescriptor
		{
			public TestDescriptor(string name, Type type) : base(name, type)
			{
			}

			public override void Assign(object target, object value)
			{
				throw new NotImplementedException();
			}
		}
	}
}
