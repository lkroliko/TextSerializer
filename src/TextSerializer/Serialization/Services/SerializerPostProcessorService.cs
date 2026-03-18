namespace MrRabbit.TextSerializer.Serialization.Services;

internal class SerializerPostProcessorService : ISerializerPostProcessorService
{
    private readonly ISerializerPostProcessorProvider _serializerPreProcessorProvider;

    public SerializerPostProcessorService(ISerializerPostProcessorProvider serializerPostProcessorProvider)
    {
        _serializerPreProcessorProvider = serializerPostProcessorProvider;
    }

    public string Run(string text)
    {
        _serializerPreProcessorProvider.Get().ToList().ForEach(p => text = p.Process(text));
        return text;
    }
}
