namespace MrRabbit.TextSerializer.EndToEndTests.DeserializationTests;

[Trait("Category", "Deserialization")]
public class DeserializeAdvancedMessage : IClassFixture<ServiceProviderFixture>
{
    private readonly ITextSerializer _textSerializer;

    public DeserializeAdvancedMessage(ServiceProviderFixture fixture)
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
    public void WhenDeserializeToTypeThenResultIsValid(string text, TestReceiveMessage expected)
    {
        var result = _textSerializer.Deserialize<TestReceiveMessage>(text);

        result.Should().BeEquivalentTo(expected);
    }

    public static TheoryData<string, TestReceiveMessage> TestData => new()
    {
        {
            $"<DeserializeAdvancedMessage|Value1|Value2Value3|Value4|>",
            new()
            {
                MessageId = nameof(DeserializeAdvancedMessage),
                Property1 = "Value1",
                Property2 = new SimpleValueObject(){ Property1 = "Value2", Property2 = "Value3"},
                Property3 = "Value4",
                Property4 = null,
            }
        },
        {
            $"<DeserializeAdvancedMessage|Value1|Value2Value3|Value4|Value5Value6>",
            new()
            {
                MessageId = nameof(DeserializeAdvancedMessage),
                Property1 = "Value1",
                Property2 = new SimpleValueObject(){ Property1 = "Value2", Property2 = "Value3"},
                Property3 = "Value4",
                Property4 = new SimpleValueObject(){ Property1 = "Value5", Property2 = "Value6"},
            }
        },
    };

    [MessageId(nameof(DeserializeAdvancedMessage))]
    public class TestReceiveMessage : ReceiveMessage
    {
        public string Property1 { get; set; } = default!;
        [ContextProperty]
        public SimpleValueObject Property2 { get; set; } = default!;
        public string Property3 { get; set; } = default!;
        [ContextProperty]
        public SimpleValueObject? Property4 { get; set; } = default!;
    }

}
