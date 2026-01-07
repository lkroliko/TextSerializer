using MrRabbit.TextSerializer.Deserialization.Factories;

namespace MrRabbit.TextSerializer.UnitTests.Deserialization.Factories.DeserializationPropertyFactoryTest;

[Trait("Category", nameof(DeserializationPropertyFactory))]
public class Get
{
    private readonly DeserializationPropertyFactory _factory = new();
    private readonly IDeserializationContextFactory _contextFactory = Mock.Of<IDeserializationContextFactory>();
    private readonly object _targetObject = new();
    private readonly IPropertyInfo _propertyInfo = Mock.Of<IPropertyInfo>();
    private readonly int _index = 0;
    private readonly string[] _values = [A.Fixture.Create<string>(), A.Fixture.Create<string>()];
    private readonly Type _propertyType = typeof(string);
    private readonly object _contextTargetObject = new();
    private readonly DeserializationContext _deserializationContext = A.DeserializationContext;

    public Get()
    {
        Mock.Get(_contextFactory).Setup(x => x.Get(_propertyInfo, _targetObject, null, _values[_index]))
            .Returns(_deserializationContext);
        Mock.Get(_propertyInfo).Setup(x => x.PropertyType).Returns(_propertyType);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    public void WhenIndexIsInvalidThenExceptionThrow(int index)
    {
        var result = Record.Exception(() => _factory.Get(index, _propertyInfo, _values, _targetObject, _contextFactory));

        result.Should().NotBeNull();
        result.Should().BeOfType<TextSerializerException>();
    }

    [Fact]
    public void WhenCallThenDeserializationPropertyIsValid()
    {
        var result = _factory.Get(_index, _propertyInfo, _values, _targetObject, _contextFactory);

        result.PropertyInfo.Should().Be(_propertyInfo);
        result.TargetObject.Should().Be(_targetObject);
        result.Value.Should().Be(_values[_index]);
    }

    [Fact]
    public void WhenIsContexPropertyThenDeserializationPropertyWithContextIsValid()
    {
        Mock.Get(_propertyInfo).Setup(x => x.IsContextProperty).Returns(true);

        var result = _factory.Get(_index, _propertyInfo, _values, _targetObject, _contextFactory);

        result.PropertyInfo.Should().Be(_propertyInfo);
        result.TargetObject.Should().Be(_targetObject);
        result.Value.Should().Be(_values[_index]);
        result.Context.Should().Be(_deserializationContext);
    }
}
