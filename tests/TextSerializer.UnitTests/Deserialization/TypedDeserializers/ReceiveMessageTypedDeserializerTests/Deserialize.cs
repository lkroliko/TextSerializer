using MrRabbit.TextSerializer.Deserialization.TypedDeserializers;

namespace MrRabbit.TextSerializer.UnitTests.Deserialization.TypedDeserializers.ReceiveMessageTypedDeserializerTests;

[Trait("Category", nameof(ReceiveMessageTypedDeserializer))]
public class Deserialize
{
    private readonly ReceiveMessageTypedDeserializer _typedDeserializer;
    private readonly IValueConverterProvider _valueConverterProvider = Mock.Of<IValueConverterProvider>();
    private readonly DeserializationContext _context = A.DeserializationContext.WithProperty(builder => builder.WithValue(A.Fixture.Create<string>())).WithProperty(builder => builder);
    private readonly IValueConverter _valueConverter = Mock.Of<IValueConverter>();
    private readonly object _deserializedValue = new();

    public Deserialize()
    {
        _typedDeserializer = new(_valueConverterProvider);

        Mock.Get(_valueConverterProvider).Setup(x => x.Get(It.IsAny<IPropertyInfo>())).Returns(_valueConverter);
        Mock.Get(_valueConverter).Setup(x => x.Convert(It.IsAny<string>())).Returns(_deserializedValue);
    }

    [Fact]
    public void WhenCallThenDeserializedValueSet()
    {
        _typedDeserializer.Deserialize(_context);

        Mock.Get(_context.Properties[0].PropertyInfo).Verify(x => x.SetValue(_context.Properties[0].TargetObject, _deserializedValue), Times.Once);
    }

    [Fact]
    public void WhenValueIsNullThenDeserializedValueNotSet()
    {
        _typedDeserializer.Deserialize(_context);

        Mock.Get(_context.Properties[1].PropertyInfo).Verify(x => x.SetValue(_context.Properties[1].TargetObject, _deserializedValue), Times.Never);
    }
}
