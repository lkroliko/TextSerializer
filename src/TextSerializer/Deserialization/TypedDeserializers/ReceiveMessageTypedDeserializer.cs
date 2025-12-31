namespace MrRabbit.TextSerializer.Deserialization.TypedDeserializers;

internal class ReceiveMessageTypedDeserializer : ITypedDeserializer<ReceiveMessage>
{
    private readonly IValueConverterProvider _valueConverterProvider;

    public ReceiveMessageTypedDeserializer(IValueConverterProvider valueConverterProvider)
    {
        _valueConverterProvider = valueConverterProvider;
    }

    public void Deserialize(DeserializationContext context)
    {
        context.Properties.Where(p => p.IsObject == false).ToList().ForEach(p => Deserialize(p));
    }

    private void Deserialize(DeserializationProperty property)
    {
        if (string.IsNullOrEmpty(property.Value))
            return;

        var valueConverter = _valueConverterProvider.Get(property.PropertyInfo);
        property.DeserializedValue = valueConverter.Convert(property.Value);
    }
}
