namespace MrRabbit.TextSerializer.EndToEndTests.SerializationTests;

[Trait("Category", "Serialization")]
public class SerializeStringPropertyWithCustomValueConverter : IClassFixture<ServiceProviderFixture>
{
    private readonly ITextSerializer _textSerializer;

    public SerializeStringPropertyWithCustomValueConverter(ServiceProviderFixture fixture)
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
                Property1 = "Value1",
            },
            $"<MessageId|Value1ConvertedByCustomStringValueConverter#CRC>"
        },
    };

    public class TestTransmitMessage : TransmitMessage
    {
        [UseCustomStringValueConverter]
        public string Property1 { get; set; } = default!;

    }
}
