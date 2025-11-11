using MrRabbit.TextSerializer.Common.Enums;

namespace MrRabbit.TextSerializer.EndToEndTests.Common.Implementation;
public abstract class ReceiveMessage : TextSerializer.Common.ReceiveMessage
{
    [Position(Position.First)]
    public string MessageId { get; set; } = default!;
}
