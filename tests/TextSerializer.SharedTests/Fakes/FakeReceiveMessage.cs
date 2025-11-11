using MrRabbit.TextSerializer.Common.Attributes;

namespace MrRabbit.TextSerializer.SharedTests.Fakes;
[MessageId("123")]
public class FakeReceiveMessage : ReceiveMessage
{
    public string MessageId { get; set; } = default!;
}
