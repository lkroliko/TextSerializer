using AutoFixture;
using MrRabbit.TextSerializer.Serialization.Services;

namespace MrRabbit.TextSerializer.UnitTests.Serialization.Services.SerializerTests;
[Trait("Category", "Serializer")]
public class Serialize
{
    private readonly ITypedSerializerProvider _serializerProvider = Mock.Of<ITypedSerializerProvider>();
    private readonly ISerializerValidatorService _validator = Mock.Of<ISerializerValidatorService>();
    private readonly ISerializerFormatterService _formatter = Mock.Of<ISerializerFormatterService>();
    private readonly ISerializationContextFactory _contextFactory = Mock.Of<ISerializationContextFactory>();
    private readonly ISerializerService _serializerService = Mock.Of<ISerializerService>();
    private readonly ISerializerPreProcessorService _serializerPreProcessorService = Mock.Of<ISerializerPreProcessorService>();
    private readonly ISerializerPostProcessorService _serializerPostProcessorService = Mock.Of<ISerializerPostProcessorService>();
    private readonly ISerializerValidatorService _serializerValidatorService = Mock.Of<ISerializerValidatorService>();
    private readonly ISerializationContextFactory _serializationContextFactory = Mock.Of<ISerializationContextFactory>();
    private readonly ISerializerFormatterService _serializerFormatterService = Mock.Of<ISerializerFormatterService>();
    private readonly ITextBuilder _textBuilderFactory = Mock.Of<ITextBuilder>();
    private readonly Serializer _serializer;
    private readonly FakeTransmitMessage _transmitMessage = new();
    private readonly SerializationContext _context = A.SerializationContext.WithType(typeof(FakeTransmitMessage))
        .WithProperty(builder => builder.WithContext(builder => builder.WithType(typeof(TestClass))));
    private readonly ITypedSerializer _fakeTransmitMessageSerializer = Mock.Of<ITypedSerializer>();
    private readonly ITypedSerializer _testClassSerializer = Mock.Of<ITypedSerializer>();
    private readonly string _text = A.Fixture.Create<string>();

    public Serialize()
    {
        _serializer = new(_serializerService, _serializerPreProcessorService, _serializerPostProcessorService, _serializerValidatorService, _serializerFormatterService, _serializationContextFactory, _textBuilderFactory);

        Mock.Get(_contextFactory).Setup(s => s.Get(_transmitMessage)).Returns(_context);
        Mock.Get(_serializerProvider).Setup(p => p.Get(_context.ObjectType)).Returns(_fakeTransmitMessageSerializer);
        Mock.Get(_serializerProvider).Setup(p => p.Get(_context.Properties.First().Context!.ObjectType)).Returns(_testClassSerializer);
        Mock.Get(_textBuilderFactory).Setup(f => f.Build(_context)).Returns(_text);
    }

    [Fact]
    public void WhenCalledThenSerializationContextFactoryGetCalled()
    {
        _serializer.Serialize(_transmitMessage);

        Mock.Get(_contextFactory).Verify(f => f.Get(_transmitMessage), Times.Once);
    }

    [Fact]
    public void WhenCalledThenSerializerValidatorServiceValidateCalled()
    {
        _serializer.Serialize(_transmitMessage);

        Mock.Get(_validator).Verify(v => v.Validate(_context), Times.Once);
    }

    [Fact]
    public void WhenCalledThenTypedSerializersSerializeCalled()
    {
        _serializer.Serialize(_transmitMessage);

        Mock.Get(_fakeTransmitMessageSerializer).Verify(s => s.Serialize(_context), Times.Once);
        Mock.Get(_testClassSerializer).Verify(s => s.Serialize(_context.Properties.First().Context!), Times.Once);
    }

    [Fact]
    public void WhenCalledThenSerializerFormaterServiceFormatCalled()
    {
        _serializer.Serialize(_transmitMessage);

        Mock.Get(_formatter).Verify(f => f.Format(_context), Times.Once);
    }

    [Fact]
    public void WhenCalledThenSerializerPostProcessorsServiceCalled()
    {
        _serializer.Serialize(_transmitMessage);

        Mock.Get(_serializerPostProcessorService).Verify(p => p.Run(_text), Times.Once);
    }

    [Fact]
    public void WhenCalledThenSerializerPreProcessorsServiceCalled()
    {
        _serializer.Serialize(_transmitMessage);

        Mock.Get(_serializerPreProcessorService).Verify(p => p.Run(_context), Times.Once);
    }

    class TestClass { }
}
