namespace MrRabbit.TextSerializer.Common.Interfaces;

internal interface ISerializationContextFactory
{
    SerializationContext Get(object value);
}
