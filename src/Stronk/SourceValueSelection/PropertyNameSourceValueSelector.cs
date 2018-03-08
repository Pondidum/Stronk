namespace Stronk.SourceValueSelection
{
	public class PropertyNameSourceValueSelector : ISourceValueSelector
	{
		public string Select(ValueSelectorArgs args) => args.GetValue(args.Property.Name);
	}
}
