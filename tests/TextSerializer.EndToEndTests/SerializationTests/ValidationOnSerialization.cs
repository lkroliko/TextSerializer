using AutoFixture;
using MrRabbit.TextSerializer.SharedTests;

namespace MrRabbit.TextSerializer.EndToEndTests.SerializationTests;

[Trait("Category", "Serialization")]
public class ValidationOnSerialization : IClassFixture<ServiceProviderFixture>
{
    private readonly ITextSerializer _textSerializer;

    public ValidationOnSerialization(ServiceProviderFixture fixture)
    {
        _textSerializer = fixture.TextSerializer;
    }

    [Theory]
    [MemberData(nameof(InvalidTestData))]
    public void WhenSerializeThenExceptionThrow(TestTransmitMessage message)
    {
        var result = Record.Exception(() => _textSerializer.Serialize(message));

        result.Should().NotBeNull();
    }

    public static TheoryData<TestTransmitMessage> InvalidTestData => new()
    {
        {
            new()
            {
                MandatoryProperty = null!, //invalid
                OptionalProperty = null,
                ConditionalProperty = null,
                FixedLengthProperty = 12345,
                MaxLengthProperty = 12345,
                MinLengthProperty= 12345,
            }
        },
        {
            new()
            {
                MandatoryProperty = A.Fixture.Create<String>(),
                OptionalProperty = null,
                ConditionalProperty = null,
                FixedLengthProperty = 123456,//invalid
                MaxLengthProperty = 12345,
                MinLengthProperty= 12345,
            }
        },
        {
            new()
            {
                MandatoryProperty = A.Fixture.Create<String>(),
                OptionalProperty = null,
                ConditionalProperty = null,
                FixedLengthProperty = 1234,//invalid
                MaxLengthProperty = 12345,
                MinLengthProperty= 12345,
            }
        },
        {
            new()
            {
                MandatoryProperty = A.Fixture.Create<String>(),
                OptionalProperty = null,
                ConditionalProperty = null,
                FixedLengthProperty = 12345,
                MaxLengthProperty = 123456,//invalid
                MinLengthProperty= 12345,
            }
        },
        {
            new()
            {
                MandatoryProperty = A.Fixture.Create<String>(),
                OptionalProperty = null,
                ConditionalProperty = null,
                FixedLengthProperty = 12345,
                MaxLengthProperty = 12345,
                MinLengthProperty= 1234,//invalid
            }
        },
        {
            new()
            {
                MandatoryProperty = A.Fixture.Create<String>(),
                OptionalProperty = null,
                ConditionalProperty = null,
                FixedLengthProperty = 12345,
                MaxLengthProperty = 12345,
                MinLengthProperty= 123456,
                OptionalWithMaxLength = "123",//invalid
            }
        },
    };

    [Theory]
    [MemberData(nameof(ValidTestData))]
    public void WhenSerializeThenNoExceptionThrow(TestTransmitMessage message)
    {
        var result = Record.Exception(() => _textSerializer.Serialize(message));

        result.Should().BeNull();
    }

    public static TheoryData<TestTransmitMessage> ValidTestData => new()
    {
        {
            new()
            {
                MandatoryProperty = A.Fixture.Create<String>(),
                OptionalProperty = null,
                ConditionalProperty = null,
                FixedLengthProperty = 12345,
                MaxLengthProperty = 12345,
                MinLengthProperty= 12345,
            }
        },
        {
            new()
            {
                MandatoryProperty = A.Fixture.Create<String>(),
                OptionalProperty = null,
                ConditionalProperty = null,
                FixedLengthProperty = 12345,
                MaxLengthProperty = 1234,
                MinLengthProperty= 12345,
            }
        },
        {
            new()
            {
                MandatoryProperty = A.Fixture.Create<String>(),
                OptionalProperty = null,
                ConditionalProperty = null,
                FixedLengthProperty = 12345,
                MaxLengthProperty = 12345,
                MinLengthProperty= 123456,
            }
        },
        {
            new()
            {
                MandatoryProperty = A.Fixture.Create<String>(),
                OptionalProperty = null,
                ConditionalProperty = null,
                FixedLengthProperty = 12345,
                MaxLengthProperty = 12345,
                MinLengthProperty= 123456,
                OptionalWithMaxLength = "12",
            }
        },
    };

    public class TestTransmitMessage : TransmitMessage
    {
        public string MandatoryProperty { get; set; } = default!;

        [Optional]
        public string? OptionalProperty { get; set; }

        [Conditional]
        public string? ConditionalProperty { get; set; }

        [FixedLength(5)]
        public int FixedLengthProperty { get; set; } = default!;

        [MinLength(5)]
        public int MinLengthProperty { get; set; } = default!;

        [MaxLength(5)]
        public int MaxLengthProperty { get; set; } = default!;

        [Optional]
        [MaxLength(2)]
        public string? OptionalWithMaxLength { get; set; }
    }
}
