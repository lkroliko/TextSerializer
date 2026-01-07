using MrRabbit.TextSerializer.Deserialization.Providers;

namespace MrRabbit.TextSerializer.UnitTests.Deserialization.Providers.DeserializerPostProcessorProviderTests;

[Trait("Category", nameof(DeserializerPostProcessorProvider))]
public class Get
{
    private readonly DeserializerPostProcessorProvider _provider;
    private readonly List<IDeserializerPostProcessor> _postProcessors = [];

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
