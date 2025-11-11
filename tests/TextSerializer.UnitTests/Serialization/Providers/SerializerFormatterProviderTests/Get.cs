//using MrRabbit.TextSerializer.Serialization.Providers;

//namespace MrRabbit.TextSerializer.UnitTests.Serialization.Providers.SerializerFormatterProviderTests;
//[Trait("Category", "SerializerFormatterProvider")]
//public class Get
//{
//    private readonly ISerializerFormatter<int> _formatter1 = Mock.Of<ISerializerFormatter<int>>();
//    private readonly ISerializerFormatter<int> _formatter2 = Mock.Of<ISerializerFormatter<int>>();
//    private readonly SerializerFormatterProvider _provider;
//    private readonly SerializationProperty _property = A.SerializationProperty.WithValue(12);

//    public Get()
//    {
//        _provider = new(new[] { _formatter1, _formatter2 });
//    }

//    [Fact]
//    public void WhenCalledThenFormatersReturned()
//    {
//        var result = _provider.Get(_property.Value!.GetType());

//        result.Should().HaveCount(2);
//        result.Should().Contain(_formatter1);
//        result.Should().Contain(_formatter2);
//    }
//}

//TODO to fix test
