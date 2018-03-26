using System.Collections.Generic;
using Shouldly;
using Stronk.ConfigurationSources;
using Stronk.PropertyWriters;
using Xunit;

namespace Stronk.Tests.Scenarios
{
	public class MultiplePropertyWrites
	{
		[Fact]
		public void When_a_property_is_selected_by_two_different_writers()
		{
			var config = new StronkConfig()
				.From.Source(new DictionarySource(new Dictionary<string, string> { { "TestValue", "16" } }))
				.Write.To(new BackingFieldPropertyWriter())
				.Write.To(new PrivateSetterPropertyWriter())
				.Build<MultiWriteConfig>();

			config.AlreadySet.ShouldBe(false);
			config.TestValue.ShouldBe(16);
		}


		private class MultiWriteConfig
		{
			private int _testValue;

			public bool AlreadySet = false;

			public int TestValue
			{
				get => _testValue;
				set
				{
					if (_testValue == value)
						AlreadySet = true;

					_testValue = value;
					
				}
			}
		}
	}
}
