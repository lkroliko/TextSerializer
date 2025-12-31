namespace MrRabbit.TextSerializer.Common.Interfaces;

public interface IDeserializationPropertyFactory //TODO add to doc
{
    DeserializationProperty Get(int index, IPropertyInfo propertyInfo, string[] propertyValues, object tarbetObject, IDeserializationContextFactory contextFactory);
}
