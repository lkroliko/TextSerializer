namespace MrRabbit.TextSerializer.Common;

public class DeserializationProperty
{
    public object TargetObject { get; }

    public IPropertyInfo PropertyInfo { get; }
    public string? Value { get; }

    public object? DeserializedValue { set => PropertyInfo.SetValue(TargetObject, value); }

    public bool IsObject => Context is not null;

    public DeserializationContext? Context { get; }

    public DeserializationProperty(IPropertyInfo propertyInfo, string? value, object targetObject)
    {
        PropertyInfo = propertyInfo;
        Value = value;
        TargetObject = targetObject;
    }

    public DeserializationProperty(IPropertyInfo propertyInfo, DeserializationContext? context, string? value, object targetObject)
    {
        PropertyInfo = propertyInfo;
        Context = context;
        Value = value;
        TargetObject = targetObject;

        if (context is not null)
            context.TargetProperty = this;
    }
}
