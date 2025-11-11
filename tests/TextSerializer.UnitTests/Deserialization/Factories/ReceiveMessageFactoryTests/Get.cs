using MrRabbit.TextSerializer.Deserialization.Factories;

namespace MrRabbit.TextSerializer.UnitTests.Deserialization.Factories.ReceiveMessageFactoryTests;
[Trait("Category", "ReceiveMessageFactory")]
public class Get
{
    private const string FakeReceiveMessageMessageId = "23423123";
    private const string FakeReceiveMessageWithValueObjectMessageId = "43545545";
    private const string NotImplementedReceiveMessageMessageId = "32131";
    private readonly IObjectFactory _objectFactory = Mock.Of<IObjectFactory>();
    private readonly ReceiveMessageFactory _factory;

    public Get()
    {
        _factory = ReceiveMessageFactory.Create(typeof(FakeReceiveMessageWithValueObject).Assembly, _objectFactory);
    }

    [Fact]
    public void WhenCalledThenInstanceReturned()
    {
        Mock.Get(_objectFactory).Setup(f => f.Get(It.IsAny<Type>())).Returns(new FakeReceiveMessageWithValueObject());

        var result = _factory.Get(FakeReceiveMessageMessageId);

        result.Should().NotBeNull();
        result.Should().BeOfType<FakeReceiveMessageWithValueObject>();
    }

    [Fact]
    public void WhenCalledThenInstanceWithInstanceValueObjectReturned()
    {
        Mock.Get(_objectFactory).Setup(f => f.Get(It.IsAny<Type>())).Returns(new FakeReceiveMessageWithValueObject())
            .Callback(() =>
            {
                Mock.Get(_objectFactory).Setup(f => f.Get(It.IsAny<Type>())).Returns(new ValueObject());
            });

        var result = _factory.Get(FakeReceiveMessageWithValueObjectMessageId);

        result.Should().NotBeNull();
        result.Should().BeOfType<FakeReceiveMessageWithValueObject>();
        result.As<FakeReceiveMessageWithValueObject>().ValueObject.Should().NotBeNull();
    }

    [Fact]
    public void WhenNoImplementedreceiveMessageClassForMessageIdThenExceptionThrowned()
    {
        var result = Record.Exception(() => _factory.Get(NotImplementedReceiveMessageMessageId));

        result.Should().NotBeNull();
        result.Message.Should().Be($"No implemented class for receive message with id '{NotImplementedReceiveMessageMessageId}'.");
    }

    [MessageId(FakeReceiveMessageMessageId)]
    class FakeReceiveMessage : ReceiveMessage { }

    [MessageId(FakeReceiveMessageWithValueObjectMessageId)]
    class FakeReceiveMessageWithValueObject : ReceiveMessage
    {
        public int? TagLength { get; set; }
        public ValueObject? ValueObject { get; set; }
        public int MessageId { get; set; }
    }

    class ValueObject { }
}


