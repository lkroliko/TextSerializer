using MrRabbit.TextSerializer.Common.Exceptions;
using MrRabbit.TextSerializer.Deserialization.Factories;

namespace MrRabbit.TextSerializer.UnitTests.Serialization.Factories.DeserializationPropertyFactoryTests;

[Trait("Category", nameof(DeserializationPropertyFactory))]
public class Get
{
    private readonly IObjectFactory _objectFactory = Mock.Of<IObjectFactory>();
    private readonly DeserializationPropertyFactory _factory;
    private readonly int _index = 0;
    private readonly IPropertyInfo _propertyInfo = Mock.Of<IPropertyInfo>();
    private readonly IDeserializationContextFactory _contextFactory = Mock.Of<IDeserializationContextFactory>();
    private readonly object _targetObject = new();

    public Get()
    {
        _factory = new(_objectFactory);
    }

    [Fact]
    public void WhenCallThenResultIsValid()
    {
        var result = _factory.Get(_index, _propertyInfo, ["Value"], _targetObject, _contextFactory);

        result.Value.Should().Be("Value");
        result.PropertyInfo.Should().Be(_propertyInfo);
        result.TargetObject.Should().Be(_targetObject);
    }

    [Fact]
    public void WhenIndexOoutOffPropertyValuesThenExceptionThrow()
    {
        var result = Record.Exception(() => _factory.Get(1, _propertyInfo, ["Value"], _targetObject, _contextFactory));

        result.Should().NotBeNull();
        result.Should().BeOfType<TextSerializerException>();
    }
}
