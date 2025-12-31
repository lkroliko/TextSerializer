namespace MrRabbit.TextSerializer.Common.Interfaces;

public interface ITextSerializer
{
    string Serialize(TransmitMessage value);
    ReceiveMessage Deserialize(string text);
    ReceiveMessage Deserialize<TReceiveMessage>(string text) where TReceiveMessage : ReceiveMessage;//TODO powinna zwracać TReceiveMessage
    ReceiveMessage Deserialize(Type type, string text);
}
