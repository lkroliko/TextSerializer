namespace MrRabbit.TextSerializer.Deserialization.Services;

internal class DeserializerPostProcessorService : IDeserializerPostProcessorService
{
    private readonly IDeserializerPostProcessorProvider _deserializerPostProcessorProvider;

    public DeserializerPostProcessorService(IDeserializerPostProcessorProvider deserializerPostProcessorProvider)
    {
        _deserializerPostProcessorProvider = deserializerPostProcessorProvider;
    }

    public void Run(DeserializationContext context)
    {
        _deserializerPostProcessorProvider.Get().ToList().ForEach(p => p.Process(context));
    }
}
