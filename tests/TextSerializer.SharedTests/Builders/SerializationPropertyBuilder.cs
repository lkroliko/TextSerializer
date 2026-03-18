namespace MrRabbit.TextSerializer.SharedTests.Builders;
public class SerializationPropertyBuilder
{
    private IPropertyInfo _propertyInfo = Mock.Of<IPropertyInfo>();
    private object? _value;
    private SerializationContext? _context;
    private string? _serializedValue;

    public SerializationPropertyBuilder WithValue(object value)
    {
        _value = value;
        return this;
    }

    public SerializationPropertyBuilder WithFixedLength(int length)
    {
        Mock.Get(_propertyInfo).Setup(p => p.FixedLength).Returns(length);
        return this;
    }

    public SerializationPropertyBuilder IsMandatory()
    {
        Mock.Get(_propertyInfo).Setup(p => p.IsMandatory).Returns(true);
        return this;
    }

    public SerializationPropertyBuilder WithName(string name)
    {
        Mock.Get(_propertyInfo).Setup(p => p.Name).Returns(name);
        return this;
    }

    public SerializationPropertyBuilder WithSerializedValue(string? value)
    {
        _serializedValue = value;
        return this;
    }

    public SerializationPropertyBuilder WithContext()
    {
        _context = new SerializationContext(typeof(SerializationPropertyBuilder));
        return this;
    }

    public SerializationPropertyBuilder WithContext(Func<SerializationContextBuilder, SerializationContext> builder)
    {
        _context = builder.Invoke(new SerializationContextBuilder());
        return this;
    }

    public SerializationProperty Build() => new SerializationProperty(_propertyInfo, _value, _context) { SerializedValue = _serializedValue };

    public static implicit operator SerializationProperty(SerializationPropertyBuilder builder) => builder.Build();
}
