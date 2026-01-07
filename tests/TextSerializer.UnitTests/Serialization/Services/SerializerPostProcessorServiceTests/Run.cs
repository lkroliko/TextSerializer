using MrRabbit.TextSerializer.Serialization.Services;

namespace MrRabbit.TextSerializer.UnitTests.Serialization.Services.SerializerPostProcessorServiceTests;

[Trait("Category", nameof(SerializerPostProcessorService))]
public class Run
{
    private readonly SerializerPostProcessorService _postProcessorService;
    private readonly ISerializerPostProcessorProvider _postProcessorProvider = Mock.Of<ISerializerPostProcessorProvider>();
    private readonly List<ISerializerPostProcessor> _postProcessors = new()
    {
        Mock.Of<ISerializerPostProcessor>(),
        Mock.Of<ISerializerPostProcessor>(),
        Mock.Of<ISerializerPostProcessor>(),
    };
    private readonly string _text = A.Fixture.Create<string>();
    private readonly string _textAfter1PostProcessor = A.Fixture.Create<string>();
    private readonly string _textAfter2PostProcessor = A.Fixture.Create<string>();
    private readonly string _textAfter3PostProcessor = A.Fixture.Create<string>();
    public Run()
    {
        _postProcessorService = new(_postProcessorProvider);

        Mock.Get(_postProcessorProvider).Setup(x => x.Get()).Returns(_postProcessors);

        Mock.Get(_postProcessors[0]).Setup(x => x.Process(_text)).Returns(_textAfter1PostProcessor);
        Mock.Get(_postProcessors[1]).Setup(x => x.Process(_textAfter1PostProcessor)).Returns(_textAfter2PostProcessor);
        Mock.Get(_postProcessors[2]).Setup(x => x.Process(_textAfter2PostProcessor)).Returns(_textAfter3PostProcessor);
    }

    [Fact]
    public void WhenCallThenResultIsValid()
    {
        var result = _postProcessorService.Run(_text);

        result.Should().Be(_textAfter3PostProcessor);
    }
}
