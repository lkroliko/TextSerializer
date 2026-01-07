namespace MrRabbit.TextSerializer.Deserialization.Services;

internal class Deserializer : IDeserializer
{
    private readonly IReceiveMessageFactory _messageFactory;
    private readonly ITypedDeserializerProvider _typedDeserializerProvider;
    private readonly IDeserializerPreProcessorService _deserializerPreProcessorService;
    private readonly IDeserializerPostProcessorService _deserializerPostProcessorService;
    private readonly IDeserializationContextFactory _contextFactory;
    private readonly IMessageIdProvider _messageIdProvider;

    public Deserializer(IReceiveMessageFactory messageFactory, ITypedDeserializerProvider typedDeserializerProvider, IDeserializerPreProcessorService deserializerPreProcessorService,
        IDeserializerPostProcessorService deserializerPostProcessorService, IDeserializationContextFactory contextFactory, IMessageIdProvider messageIdProvider)
    {
        _messageFactory = messageFactory;
        _typedDeserializerProvider = typedDeserializerProvider;
        _deserializerPreProcessorService = deserializerPreProcessorService;
        _deserializerPostProcessorService = deserializerPostProcessorService;
        _contextFactory = contextFactory;
        _messageIdProvider = messageIdProvider;
    }

    public ReceiveMessage Deserialize(string text)
    {
        var receiveMessageType = GetReceiveMessageType(text);
        return Deserialize(receiveMessageType, text);
    }

    public ReceiveMessage Deserialize(Type type, string text)
    {
        text = RunPreProcess(text);
        var context = GetDeserializationContext(text, type);
        Deserialize(context);
        RunPostProcess(context);
        return (ReceiveMessage)context.TargetObject;
    }

    private DeserializationContext GetDeserializationContext(string text, Type type) => _contextFactory.Get(type, text);

    private void Deserialize(DeserializationContext context)
    {
        var deserializer = _typedDeserializerProvider.Get(context.TargetObject!.GetType());
        deserializer.Deserialize(context);
        context.Properties.Where(p => p.IsObject).ToList().ForEach(p => Deserialize(p.Context!));
    }

    private Type GetReceiveMessageType(string text)
    {
        if (_messageIdProvider.TryGetMessageId(text, out var messageId) == false)
            throw new TextSerializerException($"Unable to identify message id from data:{text}.");
        return _messageFactory.GetType(messageId);
    }

    private void RunPostProcess(DeserializationContext context) => _deserializerPostProcessorService.Run(context);

    private string RunPreProcess(string text) => _deserializerPreProcessorService.Run(text);
}
