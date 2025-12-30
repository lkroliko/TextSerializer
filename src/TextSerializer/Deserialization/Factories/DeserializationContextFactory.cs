namespace MrRabbit.TextSerializer.Deserialization.Factories;
internal class DeserializationContextFactory : IDeserializationContextFactory
{
    private readonly IPropertyInfoProvider _propertyInfoProvider;
    private readonly ITextValueProvider _textValueProvider;
    private readonly IObjectFactory _objectFactory;

    public DeserializationContextFactory(IPropertyInfoProvider propertyInfoProvider, ITextValueProvider textValueProvider, IObjectFactory objectFactory)
    {
        _propertyInfoProvider = propertyInfoProvider;
        _textValueProvider = textValueProvider;
        _objectFactory = objectFactory;
    }

    public DeserializationContext GetForValueObject(object targetObject, string propertyValue)
    {
        var properties = _propertyInfoProvider.GetProperties(targetObject);
        var deserializationProperties = GetDeserializationProperties(properties, propertyValue, targetObject);
        return new DeserializationContext(deserializationProperties, propertyValue, targetObject);
    }

    public DeserializationContext Get(Type type, string text)
    {
        var propertyValues = _textValueProvider.GetValues(text);
        var properties = _propertyInfoProvider.GetProperties(type);
        //TODO trzeba dodać możliwość innego doasowania vartości do pól , teraz jest w kolejności klasy a trzeba zrobić dopasowanie po wartości np przedtostku wartości
        if (properties.Count() != propertyValues.Length)
            throw new TextSerializerException($"Unable create deserialization context for data to '{type.Name}'. Properties count not equal values count.");

        var targetObject = _objectFactory.Get(type);
        var deserializationProperties = GetDeserializationProperties(properties, propertyValues, targetObject!);

        return new DeserializationContext(deserializationProperties, text, targetObject);
    }

    private DeserializationContext Build(IPropertyInfo propertyInfo, object parentObject, object? targetObject, string propertyValue) =>
       propertyInfo.IsCollection ? BuildForCollection(targetObject, propertyValue) : BuildForObject(propertyInfo, parentObject, targetObject, propertyValue);

    private DeserializationContext BuildForObject(IPropertyInfo propertyInfo, object parentObject, object? targetObject, string propertyValue)
    {

        if (targetObject is null)
        {
            targetObject = _objectFactory.Get(propertyInfo.PropertyType);
        }
        // throw new TextSerializerException("Unable to build deserialization context for null target object.");
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

            return new DeserializationProperty(propertyInfo, Build(propertyInfo, targetObject, propertyInfo.GetValue(targetObject)!, propertyValue), propertyValue, targetObject);
        }).ToList();

    private List<DeserializationProperty> GetDeserializationProperties(IEnumerable<IPropertyInfo> properties, string[] propertyValues, object targetObject) =>
        properties.Select((propertyInfo, i) =>
        {
            if (propertyInfo.IsContextProperty == false && propertyInfo.IsCollection == false)
                return new DeserializationProperty(propertyInfo, propertyValues[i], targetObject);

            var contextTargetObject = propertyInfo.GetValue(targetObject) ?? _objectFactory.Get(propertyInfo.PropertyType);
            propertyInfo.SetValue(targetObject, contextTargetObject);
            var context = Build(propertyInfo, targetObject, contextTargetObject, propertyValues[i]);
            return new DeserializationProperty(propertyInfo, context, propertyValues[i], targetObject);
        }).ToList();
}
