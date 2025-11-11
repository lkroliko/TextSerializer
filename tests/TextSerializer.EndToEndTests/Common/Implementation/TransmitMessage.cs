using MrRabbit.TextSerializer.Common.Enums;

namespace MrRabbit.TextSerializer.EndToEndTests.Common.Implementation;
public abstract class TransmitMessage : TextSerializer.Common.TransmitMessage
{
    [Position(Position.First)]
    public string MessageId { get; set; } = "MessageId";
}
