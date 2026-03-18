namespace MrRabbit.TextSerializer.Deserialization.Providers;

internal class MessageIdProvider : IMessageIdProvider
{
    private readonly Func<string, string> _func;

    public MessageIdProvider(Func<string, string> func)
    {
        _func = func;
    }

    public bool TryGetMessageId(string text, out string messageId)
    {
        messageId = _func.Invoke(text);
        return true;
    }
}
