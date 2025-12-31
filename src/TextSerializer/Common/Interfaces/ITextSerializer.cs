namespace MrRabbit.TextSerializer.Common.Interfaces;

public interface ITextSerializer
{
    string Serialize(TransmitMessage value);
    ReceiveMessage Deserialize(string text);
    TReceiveMessage Deserialize<TReceiveMessage>(string text) where TReceiveMessage : ReceiveMessage;

    ReceiveMessage Deserialize(Type type, string text);
}
