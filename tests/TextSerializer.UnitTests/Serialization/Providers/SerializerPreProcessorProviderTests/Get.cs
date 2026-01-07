using MrRabbit.TextSerializer.Serialization.Providers;

namespace MrRabbit.TextSerializer.UnitTests.Serialization.Providers.SerializerPreProcessorProviderTests;

[Trait("Category", nameof(SerializerPreProcessorProvider))]
public class Get
{
    private readonly SerializerPreProcessorProvider _provider;
    private readonly List<ISerializerPreProcessor> _preProcessors = [];

    public Get()
    {
        _provider = new SerializerPreProcessorProvider(_preProcessors);
    }

    [Fact]
    public void WhenCallThenResultIsValid()
    {
        var result = _provider.Get();

        result.Should().BeSameAs(_preProcessors);
    }
}
