using MrRabbit.TextSerializer.Serialization.Services;

namespace MrRabbit.TextSerializer.UnitTests.Serialization.Services.SerializerPreProcessorServiceTests;

[Trait("Category", nameof(SerializerPreProcessorService))]
public class Run
{
    private readonly SerializerPreProcessorService _preProcessorService;
    private readonly ISerializerPreProcessorProvider _preProcessorProvider = Mock.Of<ISerializerPreProcessorProvider>();
    private readonly SerializationContext _context = A.SerializationContext;
    private readonly List<ISerializerPreProcessor> _preProcessors = new()
    {
        Mock.Of<ISerializerPreProcessor>(),
        Mock.Of<ISerializerPreProcessor>(),
        Mock.Of<ISerializerPreProcessor>(),
    };

    public Run()
    {
        _preProcessorService = new(_preProcessorProvider);

        Mock.Get(_preProcessorProvider).Setup(x => x.Get()).Returns(_preProcessors);
    }

    [Fact]
    public void WhenCallThenPreProcessorsProcessCalled()
    {
        _preProcessorService.Run(_context);

        _preProcessors.ForEach(x => Mock.Get(x).Verify(x => x.Process(_context), Times.Once));
    }
}
