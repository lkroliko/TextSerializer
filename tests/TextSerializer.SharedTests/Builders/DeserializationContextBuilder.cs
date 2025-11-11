namespace MrRabbit.TextSerializer.SharedTests.Builders;
public class DeserializationContextBuilder
{
    private string? _value;
    private List<DeserializationProperty> _properties = new();
    private object _targetObject = new object();

    internal DeserializationContextBuilder WithValue(string value)
    {
        _value = value;
        return this;
    }

    internal DeserializationContextBuilder WithProperty(Func<DeserializationPropertyBuilder, DeserializationProperty> builder)
    {
        _properties.Add(builder.Invoke(new DeserializationPropertyBuilder(_targetObject)));
        return this;
    }

    internal DeserializationContextBuilder WithTargetObject(object targetObject)
    {
        _targetObject = targetObject;
        return this;
    }

    private DeserializationContext Build() => new DeserializationContext(_properties, _value, _targetObject);

    public static implicit operator DeserializationContext(DeserializationContextBuilder builder) => builder.Build();
}
