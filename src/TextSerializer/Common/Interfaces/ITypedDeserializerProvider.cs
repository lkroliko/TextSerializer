namespace MrRabbit.TextSerializer.Common.Interfaces;

internal interface ITypedDeserializerProvider
{
    ITypedDeserializer Get(Type type);
}
