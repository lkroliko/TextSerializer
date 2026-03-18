namespace MrRabbit.TextSerializer.Common.Interfaces;

public interface IMessageIdProvider
{
    bool TryGetMessageId(string text, out string messageId);
}
