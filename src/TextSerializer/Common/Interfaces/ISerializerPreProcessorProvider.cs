namespace MrRabbit.TextSerializer.Common.Interfaces;

internal interface ISerializerPreProcessorProvider
{
    IEnumerable<ISerializerPreProcessor> Get();
}

internal interface IDeserializerPostProcessorProvider
{
    IEnumerable<IDeserializerPostProcessor> Get();
}