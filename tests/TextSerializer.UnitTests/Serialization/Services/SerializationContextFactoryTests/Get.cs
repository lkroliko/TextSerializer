using MrRabbit.TextSerializer.Serialization.Factories;

namespace MrRabbit.TextSerializer.UnitTests.Serialization.Services.SerializationContextFactoryTests;
[Trait("Category", "SerializationContextFactory")]
public class Get
{
    private readonly IPropertyInfoProvider _provider = Mock.Of<IPropertyInfoProvider>();
    private readonly SerializationContextFactory _factory;
    private readonly List<IPropertyInfo> _testClass1Properties = new();
    private readonly IPropertyInfo _testClass1Property1 = Mock.Of<IPropertyInfo>();
    private readonly int _testClass1Property1Value = 13;
    private readonly IPropertyInfo _testClass1Property2 = Mock.Of<IPropertyInfo>();
    private readonly int _testClass1Property2Value = 15;
    private readonly IPropertyInfo _testClass1Property3 = Mock.Of<IPropertyInfo>();
    private readonly List<IPropertyInfo> _testClass2Properties = new();
    private readonly IPropertyInfo _testClass2Property1 = Mock.Of<IPropertyInfo>();
    private readonly int _testClass2Property1Value = 16;
    private readonly TestClass1 _value = new();

    public Get()
    {
        _factory = new(_provider);

        _testClass1Properties.Add(_testClass1Property1);
        _testClass1Properties.Add(_testClass1Property2);
        _testClass1Properties.Add(_testClass1Property3);
        Mock.Get(_testClass1Property1).Setup(s => s.GetValue(_value)).Returns(_testClass1Property1Value);
        Mock.Get(_testClass1Property2).Setup(s => s.GetValue(_value)).Returns(_testClass1Property2Value);
        Mock.Get(_testClass1Property3).Setup(s => s.GetValue(_value)).Returns(_value.Property3);
        Mock.Get(_testClass1Property3).Setup(s => s.IsContextProperty).Returns(true);

        _testClass2Properties.Add(_testClass2Property1);
        Mock.Get(_testClass2Property1).Setup(s => s.GetValue(_value.Property3)).Returns(_testClass2Property1Value);

        Mock.Get(_provider).Setup(p => p.GetProperties(typeof(TestClass1))).Returns(() => _testClass1Properties);
        Mock.Get(_provider).Setup(p => p.GetProperties(typeof(TestClass2))).Returns(() => _testClass2Properties);
    }

    [Fact]
    public void WhenContextCreatedThenObjectTypeIsValid()
    {
        var result = _factory.Get(_value);

        result.ObjectType.Should().Be(typeof(TestClass1));
    }

    [Fact]
    public void WhenContextCreatedThenSerializationPropertysAreValid()
    {
        var result = _factory.Get(_value);

        result.Properties.Should().HaveCount(3);
        result.Properties.Should().Contain(p => p.PropertyInfo == _testClass1Property1 && (int)p.Value! == _testClass1Property1Value && p.Context == null);
        result.Properties.Should().Contain(p => p.PropertyInfo == _testClass1Property2 && (int)p.Value! == _testClass1Property2Value && p.Context == null);
        result.Properties.Should().Contain(p => p.PropertyInfo == _testClass1Property3 && (TestClass2)p.Value! == _value.Property3 && p.Context != null);
    }

    [Fact]
    public void WhenContextCreatedThenValueObjectContextObjectTypeIsValid()
    {
        var result = _factory.Get(_value);

        result.Properties.Last().Context!.ObjectType.Should().Be(typeof(TestClass2));
    }

    [Fact]
    public void WhenContextCreatedThenValueObjectContextSerializationPropertysAreValid()
    {
        var result = _factory.Get(_value);

        result.Properties.Last().Context!.Properties.Should().HaveCount(1);
        result.Properties.Last().Context!.Properties.Should().Contain(p => p.PropertyInfo == _testClass2Property1 && (int)p.Value! == _testClass2Property1Value && p.Context == null);
    }

    class TestClass1
    {
        public int Property1 { get; set; }
        public int Property2 { get; set; }
        public TestClass2 Property3 { get; set; } = new();
    }

    class TestClass2
    {
        public int Property1 { get; set; }
    }
}
