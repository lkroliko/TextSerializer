namespace MrRabbit.TextSerializer.Serialization.Services;

internal class SerializerPreProcessorService : ISerializerPreProcessorService
{
    private readonly ISerializerPreProcessorProvider _serializerPreProcessorProvider;

    public SerializerPreProcessorService(ISerializerPreProcessorProvider serializerPreProcessorProvider)
    {
        _serializerPreProcessorProvider = serializerPreProcessorProvider;
    }

    public void Run(SerializationContext context)
    {
        _serializerPreProcessorProvider.Get().ToList().ForEach(p => p.Process(context));
    }
}
