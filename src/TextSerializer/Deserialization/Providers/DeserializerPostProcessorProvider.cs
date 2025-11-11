namespace MrRabbit.TextSerializer.Deserialization.Providers;

internal class DeserializerPostProcessorProvider : IDeserializerPostProcessorProvider
{
    private readonly IEnumerable<IDeserializerPostProcessor> _deserializerPostProcessors;

    public DeserializerPostProcessorProvider(IEnumerable<IDeserializerPostProcessor> deserializerPostProcessors)
    {
        _deserializerPostProcessors = deserializerPostProcessors;
    }

    public IEnumerable<IDeserializerPostProcessor> Get() => _deserializerPostProcessors;
}
