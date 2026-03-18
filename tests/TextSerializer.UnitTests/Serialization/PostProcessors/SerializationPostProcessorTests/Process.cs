using MrRabbit.TextSerializer.Serialization.PostProcessors;

namespace MrRabbit.TextSerializer.UnitTests.Serialization.PostProcessors.SerializationPostProcessorTests;

[Trait("Category", nameof(SerializationPostProcessor))]
public class Process
{
    private readonly SerializationPostProcessor _postProcessor = new(x => "Test");

    [Fact]
    public void WhenCallThenresultIsValid()
    {
        var result = _postProcessor.Process(A.Fixture.Create<string>());

        result.Should().Be("Test");
    }
}
