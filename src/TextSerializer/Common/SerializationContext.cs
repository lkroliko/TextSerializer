namespace MrRabbit.TextSerializer.Common;

public class SerializationContext
{
    public List<SerializationProperty> Properties { get; } = new();
    public Type ObjectType { get; }

    public SerializationContext(Type objectType)
    {
        ObjectType = objectType;
    }

    public SerializationProperty? GetProperty(string propertyName) => Properties.FirstOrDefault(p => p.PropertyInfo.Name == propertyName);
}
