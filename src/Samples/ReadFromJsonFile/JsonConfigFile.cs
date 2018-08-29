using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Stronk.ConfigurationSources;

namespace ReadFromJsonFile
{
	public class JsonConfigFile : IConfigurationSource
	{
		private readonly Lazy<Dictionary<string, string>> _file;

		public JsonConfigFile(string filePath)
		{
			_file = new Lazy<Dictionary<string, string>>(() =>
			{
				var json = File.ReadAllText(filePath);
				var map = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

				return new Dictionary<string, string>(map, StringComparer.OrdinalIgnoreCase);
			});
		}

		public IEnumerable<string> GetAvailableKeys() => _file.Value.Keys;
		public string GetValue(string key) => _file.Value.TryGetValue(key, out var value) ? value : null;
	}
}
