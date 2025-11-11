namespace MrRabbit.TextSerializer.Common.Interfaces;

internal interface IDeserializationContextFactory
{
    DeserializationContext Get(Type type, string text);
    DeserializationContext GetForValueObject(object targetObject, string propertyValue);
}
