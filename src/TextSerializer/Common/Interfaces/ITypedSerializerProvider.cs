namespace MrRabbit.TextSerializer.Common.Interfaces;

internal interface ITypedSerializerProvider
{
    ITypedSerializer Get(Type type);
}