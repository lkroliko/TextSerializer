namespace MrRabbit.TextSerializer.Common.Interfaces;

internal interface ISerializerPostProcessorProvider
{
    IEnumerable<ISerializerPostProcessor> Get();
}
