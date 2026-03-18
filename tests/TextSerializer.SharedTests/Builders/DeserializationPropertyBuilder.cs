namespace MrRabbit.TextSerializer.SharedTests.Builders;

public class DeserializationPropertyBuilder
{
    private readonly IPropertyInfo _propertyInfo = Mock.Of<IPropertyInfo>();
    private string? _value;
    private object? _propertyParentValue;
    private DeserializationContext? _context;

    public DeserializationPropertyBuilder(object propertyParentValue)
    {
        _propertyParentValue = propertyParentValue;
    }

    public DeserializationPropertyBuilder WithPropertyParentValue(object value)
    {
        _propertyParentValue = value;
        return this;
    }

    public DeserializationPropertyBuilder WithValue(string? value)
    {
        _value = value;
        return this;
    }

    public DeserializationPropertyBuilder WithName(string name)
    {
        Mock.Get(_propertyInfo).Setup(p => p.Name).Returns(name);
        return this;
    }

    public DeserializationPropertyBuilder WithContext()
    {
        _context = A.DeserializationContext;
        return this;
    }

    private DeserializationProperty Build() => new(_propertyInfo, _context, _value, _propertyParentValue!);

    public static implicit operator DeserializationProperty(DeserializationPropertyBuilder builder) => builder.Build();
}
