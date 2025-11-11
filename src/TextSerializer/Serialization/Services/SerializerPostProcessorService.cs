namespace MrRabbit.TextSerializer.Serialization.Services;

internal class SerializerPostProcessorService : ISerializerPostProcessorService
{
    private readonly ISerializerPostProcessorProvider _serializerPostProcessorProvider;

    public SerializerPostProcessorService(ISerializerPostProcessorProvider serializerPostProcessorProvider)
    {
        _serializerPostProcessorProvider = serializerPostProcessorProvider;
    }

    public void Run(SerializationContext context)
    {
        _serializerPostProcessorProvider.Get().ToList().ForEach(p => p.Process(context));
    }
}
