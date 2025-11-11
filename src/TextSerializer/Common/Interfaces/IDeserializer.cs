namespace MrRabbit.TextSerializer.Common.Interfaces;
internal interface IDeserializer
{
    ReceiveMessage Deserialize(string text);
    ReceiveMessage Deserialize(Type type, string text);
}

