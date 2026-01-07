namespace MrRabbit.TextSerializer.Deserialization.Factories;

internal class DeserializationContextFactory : IDeserializationContextFactory
{
    private readonly IPropertyInfoProvider _propertyInfoProvider;
    private readonly ITextValueProvider _textValueProvider;
    private readonly IObjectFactory _objectFactory;
    private readonly IDeserializationPropertyFactory _propertyFactory;

    public DeserializationContextFactory(IPropertyInfoProvider propertyInfoProvider, ITextValueProvider textValueProvider, IObjectFactory objectFactory, IDeserializationPropertyFactory propertyFactory)
    {
        _propertyInfoProvider = propertyInfoProvider;
        _textValueProvider = textValueProvider;
        _objectFactory = objectFactory;
        _propertyFactory = propertyFactory;
    }

    public DeserializationContext GetForValueObject(object targetObject, string propertyValue)//TODO not used
    {
        var properties = _propertyInfoProvider.GetProperties(targetObject);
        var deserializationProperties = GetDeserializationProperties(properties, propertyValue, targetObject);
        return new DeserializationContext(deserializationProperties, propertyValue, targetObject);
    }

    public DeserializationContext Get(Type type, string text)
    {
        var propertyValues = _textValueProvider.GetValues(text);
        var properties = _propertyInfoProvider.GetProperties(type);
        var targetObject = _objectFactory.Get(type);
        var deserializationProperties = GetDeserializationProperties(properties, propertyValues, targetObject!);

        return new DeserializationContext(deserializationProperties, text, targetObject);
    }

    public DeserializationContext Get(IPropertyInfo propertyInfo, object parentObject, object? targetObject, string propertyValue) =>
       propertyInfo.IsCollection ? BuildForCollection(targetObject, propertyValue) : BuildForObject(propertyInfo, parentObject, targetObject, propertyValue);

    private DeserializationContext BuildForObject(IPropertyInfo propertyInfo, object parentObject, object? targetObject, string propertyValue)
    {
        if (targetObject is null)
        {
            targetObject = _objectFactory.Get(propertyInfo.PropertyType);
            propertyInfo.SetValue(parentObject, targetObject);
        }

        var properties = _propertyInfoProvider.GetProperties(targetObject);
        var deserializationProperties = GetDeserializationProperties(properties, propertyValue, targetObject);
        return new DeserializationContext(deserializationProperties, propertyValue, targetObject);
    }

    private DeserializationContext BuildForCollection(object? targetObject, string propertyValue) =>
         new(new List<DeserializationProperty>(), propertyValue, targetObject!);

    private List<DeserializationProperty> GetDeserializationProperties(IEnumerable<IPropertyInfo> properties, string propertyValue, object targetObject) =>
        properties.Select((propertyInfo, i) =>
        {
            if (propertyInfo.IsContextProperty == false && propertyInfo.IsCollection == false)
                return new DeserializationProperty(propertyInfo, propertyValue, targetObject);

            return new DeserializationProperty(propertyInfo, Get(propertyInfo, targetObject, propertyInfo.GetValue(targetObject)!, propertyValue), propertyValue, targetObject);
        }).ToList();

    private List<DeserializationProperty> GetDeserializationProperties(IEnumerable<IPropertyInfo> properties, string[] propertyValues, object targetObject) =>
        properties.Select((propertyInfo, i) => _propertyFactory.Get(i, propertyInfo, propertyValues, targetObject, this)).ToList();
}
