namespace MrRabbit.TextSerializer.UnitTests.Services.PropertyInfoProviderTests;
[Trait("Category", "PropertyInfoProvider")]
public class GetProperties
{
    private readonly PropertyInfoProvider _provider = new();

    [Fact]
    public void WhenCalledThenOrderedPropertiesReturned()
    {
        var instance = new TestClass();

        var result = _provider.GetProperties(instance);

        result.Should().HaveCount(5);
        result.ToList()[0].Name.Should().Be(nameof(TestClass.FirstProperty));
        result.ToList()[1].Name.Should().Be(nameof(TestClass.Property1));
        result.ToList()[2].Name.Should().Be(nameof(TestClass.Property2));
        result.ToList()[3].Name.Should().Be(nameof(TestClass.PenultimateProperty));
        result.ToList()[4].Name.Should().Be(nameof(TestClass.LastProperty));
    }

    class TestClass : TestClassBase
    {
        public override int PenultimateProperty { get; set; }
        public int Property1 { get; set; }
        public int Property2 { get; set; }
        public override int LastProperty { get; set; }
    }

    abstract class TestClassBase
    {
        [Position(Position.Penultimate)]
        public abstract int PenultimateProperty { get; set; }
        [Position(Position.Last)]
        public abstract int LastProperty { get; set; }
        [Position(Position.First)]
        public string FirstProperty { get; set; } = default!;
    }
}
