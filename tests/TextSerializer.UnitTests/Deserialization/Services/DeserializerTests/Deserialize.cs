using MrRabbit.TextSerializer.Deserialization.Services;

namespace MrRabbit.TextSerializer.UnitTests.Deserialization.Services.DeserializerTests;

[Trait("Category", "Deserializer")]
public class Deserialize
{
    private string _messageId = "123";
    private readonly IReceiveMessageFactory _messageFactory = Mock.Of<IReceiveMessageFactory>();
    private readonly ITypedDeserializerProvider _typedDeserializerProvider = Mock.Of<ITypedDeserializerProvider>();
    private readonly IDeserializationContextFactory _contextFactory = Mock.Of<IDeserializationContextFactory>();
    private readonly IDeserializerPostProcessorService _deserializerPostProcessorService = Mock.Of<IDeserializerPostProcessorService>();
    private readonly IDeserializerPreProcessorService _deserializerPreProcessorService = Mock.Of<IDeserializerPreProcessorService>();
    private readonly ITypedDeserializer _typedDeserializer = Mock.Of<ITypedDeserializer>();
    private readonly IMessageIdProvider _messageIdProvider = Mock.Of<IMessageIdProvider>();
    private readonly Deserializer _deserializer;
    private readonly string _text = $"<123>";
    private readonly Type _messageType = typeof(FakeReceiveMessage);
    private readonly FakeReceiveMessage _message = new();
    private readonly DeserializationContext _context;

    public Deserialize()
    {
        _deserializer = new(_messageFactory, _typedDeserializerProvider, _deserializerPreProcessorService, _deserializerPostProcessorService, _contextFactory, _messageIdProvider);

        _context = A.DeserializationContext.WithTargetObject(_message);

        Mock.Get(_messageFactory).Setup(f => f.GetType(_messageId)).Returns(_messageType);
        Mock.Get(_messageIdProvider).Setup(p => p.TryGetMessageId(_text, out _messageId)).Returns(true);
        Mock.Get(_deserializerPreProcessorService).Setup(s => s.Run(_text)).Returns(_text);
        Mock.Get(_contextFactory).Setup(f => f.Get(_messageType, _text)).Returns(_context);
        Mock.Get(_typedDeserializerProvider).Setup(p => p.Get(It.IsAny<Type>())).Returns(_typedDeserializer);
    }

    [Fact]
    public void WhenCalledThenReceiveMessageReturned()
    {
        var result = _deserializer.Deserialize(_text);

        result.Should().Be(_message);
    }

    [Fact]
    public void WhenCalledThenTypedDeserializerDeserializeCalled()
    {
        var result = _deserializer.Deserialize(_text);

        Mock.Get(_typedDeserializer).Verify(d => d.Deserialize(_context), Times.Once);
    }

    [Fact]
    public void WhenCalledThendeserializerPreProcessorServiceRunCalled()
    {
        var result = _deserializer.Deserialize(_text);

        Mock.Get(_deserializerPreProcessorService).Verify(p => p.Run(_text), Times.Once);
    }

    [Fact]
    public void WhenCalledThendeserializerPostProcessorServiceRunCalled()
    {
        var result = _deserializer.Deserialize(_text);

        Mock.Get(_deserializerPostProcessorService).Verify(p => p.Run(_context), Times.Once);
    }
}
