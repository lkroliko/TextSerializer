namespace MrRabbit.TextSerializer.Common;

public class DeserializationContext
{
    public IReadOnlyList<DeserializationProperty> Properties { get; }
    public string? Value { get; }

    public DeserializationProperty? TargetProperty { get; internal set; }
    public object TargetObject { get; }
    public DeserializationContext? ParentContext { get; internal set; }

    public DeserializationContext(IEnumerable<DeserializationProperty> properties, string? value, object targetObject)
    {
        Properties = properties.ToList().AsReadOnly();
        Value = value;
        TargetObject = targetObject;

        properties.Where(x => x.IsObject).ToList().ForEach(c => c.Context!.ParentContext = this);

    }


    public DeserializationProperty? GetProperty(string propertyName) => Properties.FirstOrDefault(p => p.PropertyInfo.Name == propertyName);
}
