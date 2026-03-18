//using FluentAssertions;
//using MrRabbit.TextSerializer.Common.Interfaces;
//using MrRabbit.TextSerializer.EndToEndTests.Common.Fixtures;
//using MrRabbit.TextSerializer.EndToEndTests.Common.Implementation;

//namespace MrRabbit.TextSerializer.EndToEndTests.DeserializationTests;

//[Trait("Category", "Deserialization")]
//public class DeserializeCollectionReceiveMessage : IClassFixture<ServiceProviderFixture>
//{
//    private readonly ITextSerializer _textSerializer;

//    public DeserializeCollectionReceiveMessage(ServiceProviderFixture fixture)
//    {
//        _textSerializer = fixture.TextSerializer;
//    }

//    [Theory]
//    [MemberData(nameof(TestData))]
//    public void WhenDeserializeThenResultIsValid(string text, TestReceiveMessage expected)
//    {
//        var result = _textSerializer.Deserialize(text);

//        result.Should().BeOfType<TestReceiveMessage>();
//        result.Should().BeEquivalentTo(expected);
//    }

//    [Theory]
//    [MemberData(nameof(TestData))]
//    public void WhenDeserializeByTypeThenResultIsValid(string text, TestReceiveMessage expected)
//    {
//        var result = _textSerializer.Deserialize<TestReceiveMessage>(text);

//        result.Should().BeEquivalentTo(expected);
//    }

//    public static TheoryData<string, TestReceiveMessage> TestData => new()
//    {
//        {
//            $"<DeserializeCollectionReceiveMessage|Value1Value2|Value3Value4>",
//            new()
//            {
//                MessageId = nameof(DeserializeCollectionReceiveMessage),
//                Property1 = new(){ new SimpleValueObject(){ Property1 = "Value1", Property2 = "Value2"} },
//            }
//        },
//    };

//    [MessageId(nameof(DeserializeCollectionReceiveMessage))]
//    public class TestReceiveMessage : ReceiveMessage
//    {
//        public List<SimpleValueObject> Property1 { get; set; } = default!;
//    }

//}

//TODO implement collection support
