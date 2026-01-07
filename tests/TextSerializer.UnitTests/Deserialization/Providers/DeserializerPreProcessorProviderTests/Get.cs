using MrRabbit.TextSerializer.Deserialization.Providers;

namespace MrRabbit.TextSerializer.UnitTests.Deserialization.Providers.DeserializerPreProcessorProviderTests;

[Trait("Category", nameof(DeserializerPreProcessorProvider))]
public class Get
{
    private readonly DeserializerPreProcessorProvider _provider;
    private readonly List<IDeserializerPreProcessor> _preProcessors = [];

    public Get()
    {
        _provider = new(_preProcessors);
    }

    [Fact]
    public void WhenCallThenResultIsValid()
    {
        var result = _provider.Get();

        result.Should().BeSameAs(_preProcessors);
    }
}
