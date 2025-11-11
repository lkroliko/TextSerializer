namespace MrRabbit.TextSerializer.Deserialization.Services;

internal class DeserializerPreProcessorService : IDeserializerPreProcessorService
{
    private readonly IDeserializerPreProcessorProvider _deserializerPreProcessorProvider;

    public DeserializerPreProcessorService(IDeserializerPreProcessorProvider deserializerPreProcessorProvider)
    {
        _deserializerPreProcessorProvider = deserializerPreProcessorProvider;
    }

    public string Run(string text)
    {
        _deserializerPreProcessorProvider.Get().ToList().ForEach(p => text = p.Process(text));
        return text;
    }
}
