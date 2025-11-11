namespace MrRabbit.TextSerializer.EndToEndTests.DeserializationTests;

[Trait("Category", "Deserialization")]
public class DeserializeEnumPropertyMessage : IClassFixture<ServiceProviderFixture>
{
    private readonly ITextSerializer _textSerializer;

    public DeserializeEnumPropertyMessage(ServiceProviderFixture fixture)
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
            $"<DeserializeEnumPropertyMessage|0#CRC>",
            new()
            {
                MessageId = nameof(DeserializeEnumPropertyMessage),
                Property1 = TestEnum.Value0,
            }
        },
        {
            $"<DeserializeEnumPropertyMessage|1#CRC>",
            new()
            {
                MessageId = nameof(DeserializeEnumPropertyMessage),
                Property1 = TestEnum.Value1,
            }
        },
        {
            $"<DeserializeEnumPropertyMessage|2#CRC>",
            new()
            {
                MessageId = nameof(DeserializeEnumPropertyMessage),
                Property1 = TestEnum.Value2,
            }
        },
    };

    [MessageId(nameof(DeserializeEnumPropertyMessage))]
    public class TestReceiveMessage : ReceiveMessage
    {
        public TestEnum Property1 { get; set; } = default!;
    }

    public enum TestEnum
    {
        Value0 = 0,
        Value1 = 1,
        Value2 = 2,
    }
}
