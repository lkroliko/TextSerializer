using MrRabbit.TextSerializer.Deserialization.Factories;

namespace MrRabbit.TextSerializer.UnitTests.Deserialization.Factories.DeserializationContextFactoryTests;
[Trait("Category", "DeserializationContextFactory")]
public class Get
{
    private readonly IPropertyInfoProvider _propertyInfoProvider = Mock.Of<IPropertyInfoProvider>();
    private readonly ITextValueProvider _textValueProvider = Mock.Of<ITextValueProvider>();
    private readonly IObjectFactory _objectFactory = Mock.Of<IObjectFactory>();
    private readonly DeserializationContextFactory _factory;
    private readonly string _text = "|Value1|Value2|";
    private readonly string[] _values = new[] { "Value1", "Value2" };
    private readonly List<IPropertyInfo> _propertyInfos = new();

    public Get()
    {
        _factory = new(_propertyInfoProvider, _textValueProvider, _objectFactory);

        Mock.Get(_propertyInfoProvider).Setup(p => p.GetProperties(It.IsAny<object>())).Returns(() => _propertyInfos);
        Mock.Get(_textValueProvider).Setup(p => p.GetValues(_text)).Returns(() => _values);
    }

    [Fact]
    public void WhenMessageAndProviderPropertyCountNotEqualThenExceptionThrowned()
    {
        var result = Record.Exception(() => _factory.Get(typeof(SimpleTestClass), _text));

        result.Should().NotBeNull();
        result.Message.Should().Be("Unable create deserialization context for data to 'SimpleTestClass'. Properties count not equal values count.");
    }

    [Fact]
    public void WhenGivenSimpleObjectThenDeserializationContextIsValid()
    {
        var type = typeof(SimpleTestClass);
        var deserializationObject = new SimpleTestClass();
        Mock.Get(_objectFactory).Setup(f => f.Get(type)).Returns(deserializationObject);
        var _propertyInfo1 = Mock.Of<IPropertyInfo>();
        var _propertyInfo2 = Mock.Of<IPropertyInfo>();
        _propertyInfos.Add(_propertyInfo1);
        _propertyInfos.Add(_propertyInfo2);

        var result = _factory.Get(type, _text);

        result.Value.Should().Be(_text);
        result.Properties[0].PropertyInfo.Should().Be(_propertyInfo1);
        result.Properties[0].Value.Should().Be("Value1");
        result.Properties[0].TargetObject.Should().Be(deserializationObject);
        result.Properties[1].PropertyInfo.Should().Be(_propertyInfo2);
        result.Properties[1].Value.Should().Be("Value2");
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
        var type = typeof(ComplexTestClass);
        var deserializationObject = new SimpleTestClass();
        Mock.Get(_objectFactory).Setup(f => f.Get(type)).Returns(deserializationObject);
        Mock.Get(_propertyInfoProvider).Setup(p => p.GetProperties(deserializationObject)).Returns(() => _propertyInfos);
        Mock.Get(_propertyInfoProvider).Setup(p => p.GetProperties(deserializationObject.Property2)).Returns(() => new List<IPropertyInfo>());
        var _propertyInfo1 = Mock.Of<IPropertyInfo>();
        var _propertyInfo2 = Mock.Of<IPropertyInfo>();
        Mock.Get(_propertyInfo2).Setup(p => p.IsContextProperty).Returns(true);
        Mock.Get(_propertyInfo2).Setup(p => p.GetValue(deserializationObject)).Returns(deserializationObject.Property2);
        _propertyInfos.Add(_propertyInfo1);
        _propertyInfos.Add(_propertyInfo2);

        var result = _factory.Get(type, _text);

        result.Value.Should().Be(_text);
        result.Properties[0].PropertyInfo.Should().Be(_propertyInfo1);
        result.Properties[0].Value.Should().Be("Value1");
        result.Properties[0].TargetObject.Should().Be(deserializationObject);
        result.Properties[1].PropertyInfo.Should().Be(_propertyInfo2);
        result.Properties[1].TargetObject.Should().Be(deserializationObject);
        result.Properties[1].Context.Should().NotBeNull();
        result.Properties[1].Context!.Value.Should().Be("Value2");
    }

    class ComplexTestClass
    {
        public string Property1 { get; set; } = null!;
        public SimpleTestClass Property2 { get; set; } = new();
    }

    [Fact]
    public void WhenGivenCollectionObjectThenDeserializationContextIsValid()
    {
        var type = typeof(CollectionTestClass);
        var deserializationObject = new CollectionTestClass();
        Mock.Get(_objectFactory).Setup(f => f.Get(type)).Returns(deserializationObject);
        Mock.Get(_propertyInfoProvider).Setup(p => p.GetProperties(deserializationObject)).Returns(() => _propertyInfos);
        var _propertyInfo1 = Mock.Of<IPropertyInfo>();
        Mock.Get(_propertyInfo1).Setup(p => p.IsCollection).Returns(true);
        Mock.Get(_propertyInfo1).Setup(p => p.GetValue(It.IsAny<object?>())).Returns((object?)deserializationObject.Property1);
        var _propertyInfo2 = Mock.Of<IPropertyInfo>();
        _propertyInfos.Add(_propertyInfo1);
        _propertyInfos.Add(_propertyInfo2);

        var result = _factory.Get(type, _text);

        result.Value.Should().Be(_text);
        result.Properties[0].PropertyInfo.Should().Be(_propertyInfo1);
        result.Properties[0].Value.Should().Be("Value1");
        result.Properties[0].TargetObject.Should().Be(deserializationObject);
        result.Properties[0].PropertyInfo.Should().NotBeNull();
        result.Properties[0].Context!.TargetObject.Should().Be((object?)deserializationObject.Property1);
        result.Properties[1].PropertyInfo.Should().Be(_propertyInfo2);
        result.Properties[1].Value.Should().Be("Value2");
        result.Properties[1].TargetObject.Should().Be(deserializationObject);
    }

    class CollectionTestClass
    {
        public List<SimpleTestClass> Property1 { get; set; } = new();
        public int Property2 { get; set; }
    }
}
