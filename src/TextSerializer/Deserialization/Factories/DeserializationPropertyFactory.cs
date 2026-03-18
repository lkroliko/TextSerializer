namespace MrRabbit.TextSerializer.Deserialization.Factories;

internal class DeserializationPropertyFactory : IDeserializationPropertyFactory
{
    public DeserializationProperty Get(int index, IPropertyInfo propertyInfo, string[] propertyValues, object targetObject, IDeserializationContextFactory contextFactory)
    {
        if (index >= propertyValues.Length)
            throw new TextSerializerException($"Unable create deserialization property for property '{propertyInfo.Name}'. Properties count not equal values count.");

        if (propertyInfo.IsContextProperty == false && propertyInfo.IsCollection == false)
            return new DeserializationProperty(propertyInfo, propertyValues[index], targetObject);

        var contextTargetObject = propertyInfo.GetValue(targetObject);
        var context = contextFactory.Get(propertyInfo, targetObject, contextTargetObject, propertyValues[index]);
        return new DeserializationProperty(propertyInfo, context, propertyValues[index], targetObject);
    }
}
