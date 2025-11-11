namespace MrRabbit.TextSerializer.EndToEndTests.SerializationTests;

[Trait("Category", "Serialization")]
public class SerializeFormattedMessage : IClassFixture<ServiceProviderFixture>
{
    private readonly ITextSerializer _textSerializer;

    public SerializeFormattedMessage(ServiceProviderFixture fixture)
    {
        _textSerializer = fixture.TextSerializer;
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void WhenSerializeThenResultIsValid(TestTransmitMessage message, string expected)
    {
        var result = _textSerializer.Serialize(message);

        result.Should().Be(expected);
    }

    public static TheoryData<TestTransmitMessage, string> TestData => new()
    {
        {
            new()
            {
                Property1 = 123,
                Property2 = "123",
            },
            $"<MessageId|00123|__123#CRC>"
        },
    };

    public class TestTransmitMessage : TransmitMessage
    {
        [FixedLength(5)]
        [FixedLengthFormatter]
        public int Property1 { get; set; }

        [FixedLength(5)]
        [FixedLengthFormatter]
        public string Property2 { get; set; } = default!;
    }
}
