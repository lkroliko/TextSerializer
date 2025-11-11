using MrRabbit.TextSerializer.Serialization.Services;

namespace MrRabbit.TextSerializer.UnitTests.Serialization.Services.SerializerValidatorServiceTests;
[Trait("Category", "SerializerValidatorService")]
public class Validate
{
    private readonly ISerializerValidator<TransmitMessage> _validator1 = Mock.Of<ISerializerValidator<TransmitMessage>>();
    private readonly ISerializerValidator<TransmitMessage> _validator2 = Mock.Of<ISerializerValidator<TransmitMessage>>();
    private readonly ISerializerValidator<TestClass> _validator3 = Mock.Of<ISerializerValidator<TestClass>>();
    private readonly SerializerValidatorService _service;
    private readonly SerializationContext _context = A.SerializationContext.WithType(typeof(FakeTransmitMessage))
        .WithProperty()
        .WithProperty(builder => builder.WithContext(builder => builder.WithType(typeof(TestClass))));

    public Validate()
    {
        _service = new(new ISerializerValidator[] { _validator1, _validator2, _validator3 });
    }

    [Fact]
    public void WhenCalledThenValidatorsValidateCalled()
    {
        _service.Validate(_context);

        Mock.Get(_validator1).Verify(v => v.Validate(_context), Times.Once);
        Mock.Get(_validator2).Verify(v => v.Validate(_context), Times.Once);
        Mock.Get(_validator3).Verify(v => v.Validate(_context.Properties.Last().Context!), Times.Once);
    }

    public class TestClass { }
}
