namespace MrRabbit.TextSerializer.EndToEndTests.SerializationTests;

[Trait("Category", "Serialization")]
public class SerializeNullableEnumProperty : IClassFixture<ServiceProviderFixture>
{
    private readonly ITextSerializer _textSerializer;

    public SerializeNullableEnumProperty(ServiceProviderFixture fixture)
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
                Property1 =  TestEnum.Value0,
            },
            $"<MessageId|0#CRC>"
        },
        {
            new()
            {
                Property1 =  TestEnum.Value1,
            },
            $"<MessageId|1#CRC>"
        },
        {
            new()
            {
                Property1 =  TestEnum.Value2,
            },
            $"<MessageId|2#CRC>"
        },
        {
            new()
            {
                Property1 =  null,
            },
            $"<MessageId|#CRC>"
        },
    };

    public class TestTransmitMessage : TransmitMessage
    {
        [Optional]
        public TestEnum? Property1 { get; set; } = default!;
    }

    public enum TestEnum
    {
        Value0 = 0,
        Value1 = 1,
        Value2 = 2,
    }
}
