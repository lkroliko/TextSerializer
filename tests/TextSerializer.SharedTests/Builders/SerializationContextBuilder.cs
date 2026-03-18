namespace MrRabbit.TextSerializer.SharedTests.Builders;
public class SerializationContextBuilder
{
    private Type _type = typeof(SerializationContextBuilder);
    private List<SerializationProperty> _properties = new();

    public SerializationContextBuilder WithType(Type type)
    {
        _type = type;
        return this;
    }

    public SerializationContextBuilder WithProperty()
    {
        _properties.Add(new SerializationPropertyBuilder());
        return this;
    }

    public SerializationContextBuilder WithProperty(Func<SerializationPropertyBuilder, SerializationProperty> builder)
    {
        _properties.Add(builder.Invoke(new SerializationPropertyBuilder()));
        return this;
    }

    public SerializationContext Build()
    {
        var context = new SerializationContext(_type);
        context.Properties.AddRange(_properties);
        return context;
    }

    public static implicit operator SerializationContext(SerializationContextBuilder builder) => builder.Build();
}
