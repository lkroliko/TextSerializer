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

        var value = property.Value;
        if (property.PropertyInfo.PreValueConverter != null)
            value = property.PropertyInfo.PreValueConverter.Invoke(value);
        property.DeserializedValue = valueConverter.Convert(value);
    }
}
