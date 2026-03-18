namespace MrRabbit.TextSerializer.Common.Interfaces;

internal interface ISerializerFormatterProvider
{
    ISerializerFormatter Get(Type type);
}
