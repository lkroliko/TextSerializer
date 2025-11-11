using System.Globalization;

namespace MrRabbit.TextSerializer.EndToEndTests.DeserializationTests;

[Trait("Category", "Deserialization")]
public class DeserializeDecimalPropertyMessage : IClassFixture<ServiceProviderFixture>
{
    private readonly ITextSerializer _textSerializer;

    public DeserializeDecimalPropertyMessage(ServiceProviderFixture fixture)
    {
        _textSerializer = fixture.TextSerializer;

        //TODO culture
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
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
            $"<DeserializeDecimalPropertyMessage|123.321#CRC>",
            new()
            {
                MessageId = nameof(DeserializeDecimalPropertyMessage),
                Property1 = 123.321m,
            }
        },
    };

    [MessageId(nameof(DeserializeDecimalPropertyMessage))]
    public class TestReceiveMessage : ReceiveMessage
    {
        public decimal Property1 { get; set; } = default!;
    }
}
