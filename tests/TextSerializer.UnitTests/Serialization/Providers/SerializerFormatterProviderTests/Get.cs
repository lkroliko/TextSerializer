using Microsoft.Extensions.Logging;
using MrRabbit.TextSerializer.Serialization.Providers;

namespace MrRabbit.TextSerializer.UnitTests.Serialization.Providers.SerializerFormatterProviderTests;

[Trait("Category", nameof(SerializerFormatterProvider))]
public class Get
{
    private readonly ISerializerFormatter _formatter1 = Mock.Of<ISerializerFormatter>();
    private readonly FakeSerializerFormatter _fakeFormatter = new();
    private readonly ISerializerFormatter _formatter2 = Mock.Of<ISerializerFormatter>();

    private readonly ILogger<SerializerFormatterProvider> _logger = Mock.Of<ILogger<SerializerFormatterProvider>>();
    private readonly SerializerFormatterProvider _provider;

    public Get()
    {
        _provider = new(new[] { _formatter1, _fakeFormatter, _formatter2 }, _logger);
    }

    [Fact]
    public void WhenCalledThenFormaterReturned()
    {
        var result = _provider.Get(_fakeFormatter.GetType());

        result.Should().Be(_fakeFormatter);
    }
}
