using MrRabbit.TextSerializer.Deserialization.Services;

namespace MrRabbit.TextSerializer.UnitTests.Deserialization.Services.DeserializerPreProcessorServiceTests;

[Trait("Category", nameof(DeserializerPreProcessorService))]
public class Run
{
    private readonly DeserializerPreProcessorService _preProcessorService;
    private readonly IDeserializerPreProcessorProvider _preProcessorProvider = Mock.Of<IDeserializerPreProcessorProvider>();
    private readonly List<IDeserializerPreProcessor> _preProcessors = new()
    {
        Mock.Of<IDeserializerPreProcessor>(),
        Mock.Of<IDeserializerPreProcessor>(),
        Mock.Of<IDeserializerPreProcessor>(),
    };
    private readonly string _text = A.Fixture.Create<string>();
    private readonly string _textAfter1PreProcessor = A.Fixture.Create<string>();
    private readonly string _textAfter2PreProcessor = A.Fixture.Create<string>();
    private readonly string _textAfter3PreProcessor = A.Fixture.Create<string>();

    public Run()
    {
        _preProcessorService = new(_preProcessorProvider);

        Mock.Get(_preProcessorProvider).Setup(x => x.Get()).Returns(_preProcessors);
        Mock.Get(_preProcessors[0]).Setup(x => x.Process(_text)).Returns(_textAfter1PreProcessor);
        Mock.Get(_preProcessors[1]).Setup(x => x.Process(_textAfter1PreProcessor)).Returns(_textAfter2PreProcessor);
        Mock.Get(_preProcessors[2]).Setup(x => x.Process(_textAfter2PreProcessor)).Returns(_textAfter3PreProcessor);
    }

    [Fact]
    public void WhenCallThenDeserializerPostProcessorsProcessCalled()
    {
        _preProcessorService.Run(_text);

        Mock.Get(_preProcessors[0]).Verify(x => x.Process(_text), Times.Once);
        Mock.Get(_preProcessors[1]).Verify(x => x.Process(_textAfter1PreProcessor), Times.Once);
        Mock.Get(_preProcessors[2]).Verify(x => x.Process(_textAfter2PreProcessor), Times.Once);
    }

    [Fact]
    public void WhenCallThenResultIsValid()
    {
        var result = _preProcessorService.Run(_text);

        result.Should().Be(_textAfter3PreProcessor);
    }
}
