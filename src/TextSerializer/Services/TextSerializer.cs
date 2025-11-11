
namespace MrRabbit.TextSerializer.Services;

internal class TextSerializer : ITextSerializer
{
    private readonly ISerializer _serializer;
    private readonly IDeserializer _deserializer;

    public TextSerializer(ISerializer serializer, IDeserializer deserializer)
    {
        _serializer = serializer;
        _deserializer = deserializer;
    }

    public ReceiveMessage Deserialize(string text) => _deserializer.Deserialize(text);

    public ReceiveMessage Deserialize<TReceiveMessage>(string text) where TReceiveMessage : ReceiveMessage => _deserializer.Deserialize(typeof(TReceiveMessage), text);

    public ReceiveMessage Deserialize(Type type, string text) => _deserializer.Deserialize(type, text);

    public string Serialize(TransmitMessage value) => _serializer.Serialize(value);
}
