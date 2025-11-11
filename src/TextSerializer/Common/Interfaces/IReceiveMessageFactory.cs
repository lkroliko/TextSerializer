namespace MrRabbit.TextSerializer.Common.Interfaces;
internal interface IReceiveMessageFactory
{
    ReceiveMessage Get(string messageId);
    ReceiveMessage Get(Type type);
    public Type GetType(string messageId);
}
