using MrRabbit.TextSerializer.Serialization.Providers;

namespace MrRabbit.TextSerializer.UnitTests.Serialization.Providers.SerializerPostProcessorProviderTests;

[Trait("Category", nameof(SerializerPostProcessorProvider))]
public class Get
{
    private readonly SerializerPostProcessorProvider _provider;
    private readonly List<ISerializerPostProcessor> _postProcessors = [];

    public Get()
    {
        _provider = new(_postProcessors);
    }

    [Fact]
    public void WhenCallThenResultIsValid()
    {
        var result = _provider.Get();

        result.Should().BeSameAs(_postProcessors);
    }
}
