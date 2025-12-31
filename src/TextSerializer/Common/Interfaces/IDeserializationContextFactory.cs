namespace MrRabbit.TextSerializer.Common.Interfaces;

public interface IDeserializationContextFactory
{
    DeserializationContext Get(Type type, string text);
    DeserializationContext GetForValueObject(object targetObject, string propertyValue);
    DeserializationContext Get(IPropertyInfo propertyInfo, object parentObject, object? targetObject, string propertyValue);
}
