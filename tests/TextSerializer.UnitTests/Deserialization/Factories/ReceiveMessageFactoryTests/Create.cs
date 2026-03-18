using MrRabbit.TextSerializer.Deserialization.Factories;

namespace MrRabbit.TextSerializer.UnitTests.Deserialization.Factories.ReceiveMessageFactoryTests;
[Trait("Category", "ReceiveMessageFactory")]
public class Create
{
    private readonly IObjectFactory _objectFactory = Mock.Of<IObjectFactory>();

    [Fact]
    public void WhenCalledThenInstanceCreated()
    {
        var result = ReceiveMessageFactory.Create(typeof(Create).Assembly, _objectFactory);

        result.Should().NotBeNull();
    }
}
