namespace MrRabbit.TextSerializer.Serialization.Providers;

internal class SerializerPreProcessorProvider : ISerializerPreProcessorProvider
{
    private readonly IEnumerable<ISerializerPreProcessor> _serializerPreProcessors;

    public SerializerPreProcessorProvider(IEnumerable<ISerializerPreProcessor> serializerPreProcessors)
    {
        _serializerPreProcessors = serializerPreProcessors;
    }

    public IEnumerable<ISerializerPreProcessor> Get() => _serializerPreProcessors;
}
