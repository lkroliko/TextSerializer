namespace MrRabbit.TextSerializer.Serialization.Providers;
internal class SerializerPostProcessorProvider : ISerializerPostProcessorProvider
{
    private readonly IEnumerable<ISerializerPostProcessor> _serializerPostProcessors;

    public SerializerPostProcessorProvider(IEnumerable<ISerializerPostProcessor> serializerPreProcessors)
    {
        _serializerPostProcessors = serializerPreProcessors;
    }

    public IEnumerable<ISerializerPostProcessor> Get() => _serializerPostProcessors;
}
