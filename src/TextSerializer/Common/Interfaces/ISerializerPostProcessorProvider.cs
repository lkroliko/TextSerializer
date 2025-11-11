namespace MrRabbit.TextSerializer.Common.Interfaces;

internal interface ISerializerPostProcessorProvider
{
    IEnumerable<ISerializerPostProcessor> Get();
}

internal interface IDeserializerPostProcessorProvider
{
    IEnumerable<IDeserializerPostProcessor> Get();
}