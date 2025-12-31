namespace MrRabbit.TextSerializer.Common.Interfaces;

public interface IDeserializationPropertyFactory
{
    DeserializationProperty Get(int index, IPropertyInfo propertyInfo, string[] propertyValues, object tarbetObject, IDeserializationContextFactory contextFactory);
}
