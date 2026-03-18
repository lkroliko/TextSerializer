using MrRabbit.TextSerializer.Deserialization.PreProcessor;

namespace MrRabbit.TextSerializer.UnitTests.Deserialization.PreProcessor.DeserializerPreProcessorTests;

[Trait("Category", nameof(DeserializerPreProcessor))]
public class Process
{
    private readonly DeserializerPreProcessor _preProcessor = new(x => "Test");

    [Fact]
    public void WhenCallThenResultIsValid()
    {
        var result = _preProcessor.Process(A.Fixture.Create<string>());

        result.Should().Be("Test");
    }
}
