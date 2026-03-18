namespace MrRabbit.TextSerializer.Common;

public class SerializationProperty
{
    public IPropertyInfo PropertyInfo { get; }
    public object? Value { get; }
    public string? SerializedValue { get; set; }
    public SerializationContext? Context { get; }
    public bool IsObject => Context is not null;

    public SerializationProperty(IPropertyInfo propertyInfo, object? value, SerializationContext? context)
    {
        PropertyInfo = propertyInfo;
        Value = value;
        Context = context;
    }
}
