using MrRabbit.TextSerializer.Deserialization.Providers;

namespace MrRabbit.TextSerializer.UnitTests.Deserialization.Providers.MessageIdProviderTests;

[Trait("Category", nameof(MessageIdProvider))]
public class TryGetMessageId
{
    private readonly MessageIdProvider _provider = new(x => "Test");

    [Fact]
    public void WhenCallThenResultIsTrue()
    {
        var result = _provider.TryGetMessageId(A.Fixture.Create<string>(), out var messageId);

        result.Should().BeTrue();
    }

    [Fact]
    public void WhenCallThenMessageIdIsValid()
    {
        _provider.TryGetMessageId(A.Fixture.Create<string>(), out var messageId);

        messageId.Should().Be("Test");
    }
}
