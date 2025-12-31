namespace MrRabbit.TextSerializer.Deserialization.Factories;

internal class DeserializationPropertyFactory : IDeserializationPropertyFactory
{
    private readonly IObjectFactory _objectFactory;

    public DeserializationPropertyFactory(IObjectFactory objectFactory)
    {
        _objectFactory = objectFactory;
    }

    public DeserializationProperty Get(int index, IPropertyInfo propertyInfo, string[] propertyValues, object targetObject, IDeserializationContextFactory contextFactory)
    {
        if (index == propertyValues.Length)
            throw new TextSerializerException($"Unable create deserialization property for property '{propertyInfo.Name}'. Properties count not equal values count.");

        if (propertyInfo.IsContextProperty == false && propertyInfo.IsCollection == false)
            return new DeserializationProperty(propertyInfo, propertyValues[index], targetObject);

        var contextTargetObject = propertyInfo.GetValue(targetObject) ?? _objectFactory.Get(propertyInfo.PropertyType);
        propertyInfo.SetValue(targetObject, contextTargetObject);
        var context = contextFactory.Get(propertyInfo, targetObject, contextTargetObject, propertyValues[index]);
        return new DeserializationProperty(propertyInfo, context, propertyValues[index], targetObject);
    }
}
