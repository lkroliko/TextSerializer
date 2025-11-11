using MrRabbit.TextSerializer.Deserialization.Factories;

namespace MrRabbit.TextSerializer.UnitTests.Deserialization.Factories.DeserializationContextFactoryTests;
[Trait("Category", "DeserializationContextFactory")]
public class GetForValueObjectObject
{
    private readonly IPropertyInfoProvider _propertyInfoProvider = Mock.Of<IPropertyInfoProvider>();
    private readonly ITextValueProvider _textValueProvider = Mock.Of<ITextValueProvider>();
    private readonly IObjectFactory _objectFactory = Mock.Of<IObjectFactory>();
    private readonly DeserializationContextFactory _factory;
    private readonly string _propertyValue = "Value";
    private readonly string[] _values = new[] { "Value" };
    private readonly List<IPropertyInfo> _propertyInfos = new();

    public GetForValueObjectObject()
    {
        _factory = new(_propertyInfoProvider, _textValueProvider, _objectFactory);

        Mock.Get(_propertyInfoProvider).Setup(p => p.GetProperties(It.IsAny<object>())).Returns(() => _propertyInfos);
        Mock.Get(_textValueProvider).Setup(p => p.GetValues(_propertyValue)).Returns(() => _values);
    }

    [Fact]
    public void WhenGivenSimpleObjectThenDeserializationContextIsValid()
    {
        var deserializationObject = new SimpleTestClass();
        var _propertyInfo1 = Mock.Of<IPropertyInfo>();
        var _propertyInfo2 = Mock.Of<IPropertyInfo>();
        _propertyInfos.Add(_propertyInfo1);
        _propertyInfos.Add(_propertyInfo2);

        var result = _factory.GetForValueObject(deserializationObject, _propertyValue);

        result.Value.Should().Be(_propertyValue);
        result.Properties[0].PropertyInfo.Should().Be(_propertyInfo1);
        result.Properties[0].Value.Should().Be("Value");
        result.Properties[0].TargetObject.Should().Be(deserializationObject);
        result.Properties[1].PropertyInfo.Should().Be(_propertyInfo2);
        result.Properties[1].Value.Should().Be("Value");
        result.Properties[1].TargetObject.Should().Be(deserializationObject);
    }

    class SimpleTestClass
    {
        public string Property1 { get; set; } = string.Empty;
        public int Property2 { get; set; }
    }

    [Fact]
    public void WhenGivenComplexObjectThenDeserializationContextIsValid()
    {
        var complexDeserializationObject = new ComplexTestClass();
        Mock.Get(_propertyInfoProvider).Setup(p => p.GetProperties(complexDeserializationObject)).Returns(() => _propertyInfos);
        Mock.Get(_propertyInfoProvider).Setup(p => p.GetProperties(complexDeserializationObject.Property2)).Returns(() => new List<IPropertyInfo>());
        var _propertyInfo1 = Mock.Of<IPropertyInfo>();
        var _propertyInfo2 = Mock.Of<IPropertyInfo>();
        Mock.Get(_propertyInfo2).Setup(p => p.IsContextProperty).Returns(true);
        Mock.Get(_propertyInfo2).Setup(p => p.GetValue(complexDeserializationObject)).Returns(complexDeserializationObject.Property2);
        _propertyInfos.Add(_propertyInfo1);
        _propertyInfos.Add(_propertyInfo2);

        var result = _factory.GetForValueObject(complexDeserializationObject, _propertyValue);

        result.Value.Should().Be(_propertyValue);
        result.Properties[0].PropertyInfo.Should().Be(_propertyInfo1);
        result.Properties[0].Value.Should().Be("Value");
        result.Properties[0].TargetObject.Should().Be(complexDeserializationObject);
        result.Properties[1].PropertyInfo.Should().Be(_propertyInfo2);
        result.Properties[1].TargetObject.Should().Be(complexDeserializationObject);
        result.Properties[1].Context.Should().NotBeNull();
        result.Properties[1].Context!.Value.Should().Be("Value");
    }

    class ComplexTestClass
    {
        public string Property1 { get; set; } = null!;
        public SimpleTestClass Property2 { get; set; } = new();
    }

    [Fact]
    public void WhenGivenCollectionObjectThenDeserializationContextIsValid()
    {
        var collectionDeserializationObject = new CollectionTestClass();
        Mock.Get(_propertyInfoProvider).Setup(p => p.GetProperties(collectionDeserializationObject)).Returns(() => _propertyInfos);
        var _propertyInfo1 = Mock.Of<IPropertyInfo>();
        Mock.Get(_propertyInfo1).Setup(p => p.IsContextProperty).Returns(true);
        Mock.Get(_propertyInfo1).Setup(p => p.IsCollection).Returns(true);
        Mock.Get(_propertyInfo1).Setup(p => p.GetValue(collectionDeserializationObject)).Returns(collectionDeserializationObject.Property1);
        _propertyInfos.Add(_propertyInfo1);

        var result = _factory.GetForValueObject(collectionDeserializationObject, _propertyValue);

        result.Value.Should().Be(_propertyValue);
        result.Properties[0].PropertyInfo.Should().Be(_propertyInfo1);
        result.Properties[0].Value.Should().Be("Value");
        result.Properties[0].TargetObject.Should().Be(collectionDeserializationObject);
        result.Properties[0].Context.Should().NotBeNull();
        result.Properties[0].Context!.TargetObject.Should().Be(collectionDeserializationObject.Property1);
    }

    class CollectionTestClass
    {
        public List<SimpleTestClass> Property1 { get; set; } = new();
    }
}
