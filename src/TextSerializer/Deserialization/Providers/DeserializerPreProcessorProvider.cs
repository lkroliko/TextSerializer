namespace MrRabbit.TextSerializer.Deserialization.Providers;

internal class DeserializerPreProcessorProvider : IDeserializerPreProcessorProvider
{
    private readonly IEnumerable<IDeserializerPreProcessor> _deserializerPreProcessors;

    public DeserializerPreProcessorProvider(IEnumerable<IDeserializerPreProcessor> deserializerPreProcessors)
    {
        _deserializerPreProcessors = deserializerPreProcessors;
    }

    public IEnumerable<IDeserializerPreProcessor> Get() => _deserializerPreProcessors;
}
