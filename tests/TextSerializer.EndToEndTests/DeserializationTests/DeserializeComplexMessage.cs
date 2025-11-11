using System.Globalization;

namespace MrRabbit.TextSerializer.EndToEndTests.DeserializationTests;

[Trait("Category", "Deserialization")]
public class DeserializeComplexMessage : IClassFixture<ServiceProviderFixture>
{
    private readonly ITextSerializer _textSerializer;

    public DeserializeComplexMessage(ServiceProviderFixture fixture)
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
            $"<DeserializeComplexMessage|Value1|Value2|3|4|1|1|23.3|44.567#CRC>",
            new()
            {
                MessageId = nameof(DeserializeComplexMessage),
                Property1String = "Value1",
                Property2NullableString= "Value2",
                Property3Int = 3,
                Property4NullableInt = 4,
                Property5Bool = true,
                Property6NullableBool = true,
                Property7Decimal = 23.3m,
                Property8NullableDecimal = 44.567m,
            }
        },
        {
            $"<DeserializeComplexMessage|Value1|Value2|3|4|0|0|23.3|44.567#CRC>",
            new()
            {
                MessageId = nameof(DeserializeComplexMessage),
                Property1String = "Value1",
                Property2NullableString= "Value2",
                Property3Int = 3,
                Property4NullableInt = 4,
                Property5Bool = false,
                Property6NullableBool = false,
                Property7Decimal = 23.3m,
                Property8NullableDecimal = 44.567m,
            }
        },
        {
            $"<DeserializeComplexMessage|Value1|Value2|3||0||23.3|#CRC>",
            new()
            {
                MessageId = nameof(DeserializeComplexMessage),
                Property1String = "Value1",
                Property2NullableString= "Value2",
                Property3Int = 3,
                Property4NullableInt = null,
                Property5Bool = false,
                Property6NullableBool = null,
                Property7Decimal = 23.3m,
                Property8NullableDecimal = null,
            }
        },
    };

    [MessageId(nameof(DeserializeComplexMessage))]
    public class TestReceiveMessage : ReceiveMessage
    {
        public string Property1String { get; set; } = default!;
        public string? Property2NullableString { get; set; }
        public int Property3Int { get; set; }
        public int? Property4NullableInt { get; set; }
        public bool Property5Bool { get; set; }
        public bool? Property6NullableBool { get; set; }
        public decimal Property7Decimal { get; set; }
        public decimal? Property8NullableDecimal { get; set; }
    }
}
