namespace MrRabbit.TextSerializer.Deserialization.Factories;

internal class DeserializationPropertyFactory : IDeserializationPropertyFactory
{
    private readonly IObjectFactory _objectFactory;

    public DeserializationPropertyFactory(IObjectFactory objectFactory)
    {
        _objectFactory = objectFactory;
    }

    public DeserializationProperty Get(int order, IPropertyInfo propertyInfo, string[] propertyValues, object targetObject, IDeserializationContextFactory contextFactory)
    {
        if (propertyInfo.IsContextProperty == false && propertyInfo.IsCollection == false)
            return new DeserializationProperty(propertyInfo, propertyValues[order], targetObject);

        var contextTargetObject = propertyInfo.GetValue(targetObject) ?? _objectFactory.Get(propertyInfo.PropertyType);
        propertyInfo.SetValue(targetObject, contextTargetObject);
        var context = contextFactory.Get(propertyInfo, targetObject, contextTargetObject, propertyValues[order]);
        return new DeserializationProperty(propertyInfo, context, propertyValues[order], targetObject);
    }
}
