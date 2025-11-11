namespace MrRabbit.TextSerializer.Common.Interfaces;

internal interface IDeserializerPreProcessorProvider
{
    IEnumerable<IDeserializerPreProcessor> Get();
}
