namespace MrRabbit.TextSerializer.Serialization.Factories;
internal class SerializationContextFactory : ISerializationContextFactory
{
    private readonly IPropertyInfoProvider _propertyInfoProvider;

    public SerializationContextFactory(IPropertyInfoProvider propertyInfoProvider)
    {
        _propertyInfoProvider = propertyInfoProvider;
    }

    public SerializationContext Get(object value)
    {
        var type = value.GetType();
        var properties = _propertyInfoProvider.GetProperties(type);
        var context = new SerializationContext(type);
        foreach (var property in properties)
        {
            SerializationContext? childContext = null;
            var propertyValue = property.GetValue(value);
            if (property.IsContextProperty && propertyValue is not null)
                childContext = Get(propertyValue);
            var serializationProperty = new SerializationProperty(property, propertyValue, childContext);
            context.Properties.Add(serializationProperty);
        }
        return context;
    }
}
