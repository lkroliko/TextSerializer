using MrRabbit.TextSerializer.Serialization.Providers;

namespace MrRabbit.TextSerializer.UnitTests.Serialization.Providers.TypedSerializerProviderTests;
[Trait("Category", "TypedSerializerProvider")]
public class Get
{
    private readonly TypedSerializerProvider _provider;
    private readonly List<ITypedSerializer> _serializers = new();
    private readonly FakeTypedSerializer _typedSerializer = new();
    private readonly Type _typedSerializerSerializationType = typeof(FakeTransmitMessage);

    public Get()
    {
        _serializers.Add(_typedSerializer);
        _provider = new(_serializers);
    }

    [Fact]
    public void WhenCalledThenTypedSerializerReturned()
    {
        var result = _provider.Get(_typedSerializerSerializationType);

        result.Should().Be(_typedSerializer);
    }

    [Theory]
    [InlineData(typeof(InheritFromFakeTransmitMessage))]
    [InlineData(typeof(InharitFromInheritFromFakeTransmitMessage))]
    public void WhenNoTypedSerializerThenInheritTypedSerializerReturned(Type type)
    {
        var result = _provider.Get(type);

        result.Should().Be(_typedSerializer);
    }

    class InheritFromFakeTransmitMessage : FakeTransmitMessage { }
    class InharitFromInheritFromFakeTransmitMessage : InheritFromFakeTransmitMessage { }

    [Fact]
    public void WhenNoTypedSerializedAndNoInheritTypedSerializerForTypeThenExceptionThrowned()
    {
        var result = Record.Exception(() => _provider.Get(typeof(Get)));

        result.Should().NotBeNull();
        result.Message.Should().Be("No implemented 'ITypedSerializer<Get>'.");
    }
}
