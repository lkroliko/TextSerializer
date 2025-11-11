using MrRabbit.TextSerializer.Deserialization.Providers;

namespace MrRabbit.TextSerializer.UnitTests.Deserialization.Providers.TypedDeserializerProviderTests;
[Trait("Category", "TypedDeserializerProvider")]
public class Get
{
    private readonly TypedDeserializerProvider _provider;
    private readonly List<ITypedDeserializer> _deserializers = new();
    private readonly FakeTypedDeserializer _typedDeserializer = new();
    private readonly FakeCollectionTypedDeserializer _collectionTypedDeserializer = new();

    public Get()
    {
        _deserializers.Add(_typedDeserializer);
        _deserializers.Add(_collectionTypedDeserializer);
        _provider = new(_deserializers);
    }

    #region
    [Fact]
    public void WhenCalledThenTypedDeserializedReturned()
    {
        var result = _provider.Get(typeof(FakeReceiveMessage));

        result.Should().Be(_typedDeserializer);
    }

    [Theory]
    [InlineData(typeof(InheritFromFakeReceiveMessage))]
    [InlineData(typeof(InharitFromInheritFromFakeReceiveMessage))]
    public void WhenNoTypedDeserializerThenInheritTypedDeserializerReturned(Type type)
    {
        var result = _provider.Get(type);

        result.Should().Be(_typedDeserializer);
    }

    class InheritFromFakeReceiveMessage : FakeReceiveMessage { }
    class InharitFromInheritFromFakeReceiveMessage : InheritFromFakeReceiveMessage { }

    [Fact]
    public void WhenNoTypedDeserializerAndNoInheritTypedDeserializerForTypeThenExceptionThrowned()
    {
        var result = Record.Exception(() => _provider.Get(typeof(Get)));

        result.Should().NotBeNull();
        result.Message.Should().Be("No implemented 'ITypedDeserializer<Get>'.");
    }
    #endregion

    #region Collections
    [Fact]
    public void WhenTypeIsCollectionCalledThenTypedDeserializedReturned()
    {
        var result = _provider.Get(typeof(FakeCollectionReceiveMessage));

        result.Should().Be(_collectionTypedDeserializer);
    }

    [Theory]
    [InlineData(typeof(InheritFromFakeCollectionReceiveMessage))]
    [InlineData(typeof(InharitFromInheritFromFakeCollectionReceiveMessage))]
    public void WhenNoCollectionTypedDeserializerThenInheritTypedDeserializerReturned(Type type)
    {
        var result = _provider.Get(type);

        result.Should().Be(_collectionTypedDeserializer);
    }

    class InheritFromFakeCollectionReceiveMessage : FakeCollectionReceiveMessage { }
    class InharitFromInheritFromFakeCollectionReceiveMessage : InheritFromFakeCollectionReceiveMessage { }

    [Fact]
    public void WhenNoCollectionTypedDeserializerAndNoInheritCollectionTypedDeserializerForTypeThenExceptionThrowned()
    {
        var result = Record.Exception(() => _provider.Get(typeof(Get)));

        result.Should().NotBeNull();
        result.Message.Should().Be("No implemented 'ITypedDeserializer<Get>'.");
    }
    #endregion
}
