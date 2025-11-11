namespace MrRabbit.TextSerializer.EndToEndTests.DeserializationTests;

[Trait("Category", "Deserialization")]
public class DeserializeStringPropertyMessage : IClassFixture<ServiceProviderFixture>
{
    private readonly ITextSerializer _textSerializer;

    public DeserializeStringPropertyMessage(ServiceProviderFixture fixture)
    {
        _textSerializer = fixture.TextSerializer;
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void WhenDeserializeThenResultIsValid(string text, TestReceiveMessage expected)
    {
        var result = _textSerializer.Deserialize(text);

        result.Should().BeOfType<TestReceiveMessage>();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void WhenDeserializeByTypeThenResultIsValid(string text, TestReceiveMessage expected)
    {
        var result = _textSerializer.Deserialize<TestReceiveMessage>(text);

        result.Should().BeEquivalentTo(expected);
    }

    public static TheoryData<string, TestReceiveMessage> TestData => new()
    {
        {
            $"<DeserializeStringPropertyMessage|Value1#CRC>",
            new()
            {
                MessageId = nameof(DeserializeStringPropertyMessage),
                Property1 = "Value1",
            }
        },
    };

    [MessageId(nameof(DeserializeStringPropertyMessage))]
    public class TestReceiveMessage : ReceiveMessage
    {
        public string Property1 { get; set; } = default!;
    }
}
