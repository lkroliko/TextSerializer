using MrRabbit.TextSerializer.Deserialization.Factories;

namespace MrRabbit.TextSerializer.UnitTests.Deserialization.Factories.ObjectFactoryTests;
[Trait("Category", "ObjectFactory")]
public class Get
{
    private readonly ObjectFactory _factory = new();

    [Fact]
    public void WhenCalledThenInstanceCreated()
    {
        var result = _factory.Get(typeof(FakeReceiveMessage));

        result.Should().BeAssignableTo<FakeReceiveMessage>();
    }
}
