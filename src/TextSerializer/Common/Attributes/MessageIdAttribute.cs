namespace MrRabbit.TextSerializer.Common.Attributes;
/// <summary>
/// Assigns a message id to a receive message class. Used to determine the receive message type in deserialization. 
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class MessageIdAttribute : Attribute
{
    public string MessageId { get; }

    public MessageIdAttribute(string messageId)
    {
        MessageId = messageId;
    }
}
