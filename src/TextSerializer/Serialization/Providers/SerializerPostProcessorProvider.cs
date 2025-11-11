namespace MrRabbit.TextSerializer.Serialization.Providers;

internal class SerializerPostProcessorProvider : ISerializerPostProcessorProvider
{
    private readonly IEnumerable<ISerializerPostProcessor> _serializerPostProcessors;

    public SerializerPostProcessorProvider(IEnumerable<ISerializerPostProcessor> serializerPostProcessors)
    {
        _serializerPostProcessors = serializerPostProcessors;
    }

    public IEnumerable<ISerializerPostProcessor> Get() => _serializerPostProcessors;
}
