using System.Reflection;

namespace MrRabbit.TextSerializer.Deserialization.Factories;

internal class ReceiveMessageFactory : IReceiveMessageFactory//TODO it is provider of type message
{
    private readonly Dictionary<string, Type> _messageTypes;
    private readonly IObjectFactory _objectFactory;

    private ReceiveMessageFactory(Dictionary<string, Type> messageTypes, IObjectFactory objectFactory)
    {
        _messageTypes = messageTypes;
        _objectFactory = objectFactory;
    }

    internal static ReceiveMessageFactory Create(Assembly assembly, IObjectFactory messageObjectFactory)
    {
        Dictionary<string, Type> dictionary = new();
        var receiveMessageTypes = assembly.GetTypes().Where(t => t.IsAbstract == false & t.IsAssignableTo(typeof(ReceiveMessage))).ToList();
        foreach (var receiveMessageType in receiveMessageTypes)
        {
            var messageIdAttribute = receiveMessageType.GetCustomAttribute<MessageIdAttribute>();
            if (messageIdAttribute is null)
                throw new TextSerializerException($"Receive message with type {receiveMessageType.Name} is missing MessageIdAttribute.");

            dictionary[messageIdAttribute.MessageId] = receiveMessageType;
        }

        return new ReceiveMessageFactory(dictionary, messageObjectFactory);
    }

    internal static ReceiveMessageFactory Create(IObjectFactory messageObjectFactory) =>
        new ReceiveMessageFactory([], messageObjectFactory);

    public ReceiveMessage Get(string messageId)
    {
        if (_messageTypes.ContainsKey(messageId))
        {
            var receiveMessage = (ReceiveMessage)_objectFactory.Get(_messageTypes[messageId])!;
            var instanceType = receiveMessage.GetType();
            return receiveMessage;
        }

        throw new TextSerializerException($"No implemented class for receive message with id '{messageId}'.");
    }

    public Type GetType(string messageId)
    {
        if (_messageTypes.ContainsKey(messageId))
            return _messageTypes[messageId];

        throw new TextSerializerException($"No implemented class for receive message with id '{messageId}'.");
    }

    public ReceiveMessage Get(Type type) => (ReceiveMessage)_objectFactory.Get(type)!;
}
