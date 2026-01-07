using MrRabbit.TextSerializer.Deserialization.Services;

namespace MrRabbit.TextSerializer.UnitTests.Deserialization.Services.DeserializerPostProcessorServiceTests;

[Trait("Category", nameof(DeserializerPostProcessorService))]
public class Run
{
    private readonly DeserializerPostProcessorService _postProcessorService;
    private readonly IDeserializerPostProcessorProvider _postProcessorProvider = Mock.Of<IDeserializerPostProcessorProvider>();
    private readonly List<IDeserializerPostProcessor> _postProcessors = new()
    {
        Mock.Of<IDeserializerPostProcessor>(),
        Mock.Of<IDeserializerPostProcessor>(),
        Mock.Of<IDeserializerPostProcessor>(),
    };
    private readonly DeserializationContext _context = A.DeserializationContext;

    public Run()
    {
        _postProcessorService = new(_postProcessorProvider);

        Mock.Get(_postProcessorProvider).Setup(x => x.Get()).Returns(_postProcessors);
    }

    [Fact]
    public void WhenCallThenDeserializerPostProcessorProcessCalled()
    {
        _postProcessorService.Run(_context);

        _postProcessors.ForEach(x => Mock.Get(x).Verify(x => x.Process(_context), Times.Once));
    }
}
