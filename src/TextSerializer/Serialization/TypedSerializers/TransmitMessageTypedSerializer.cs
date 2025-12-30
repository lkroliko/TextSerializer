namespace MrRabbit.TextSerializer.Serialization.TypedSerializers;

internal class TransmitMessageTypedSerializer : ITypedSerializer<TransmitMessage>
{
    private readonly IValueConverterProvider _valueConverterProvider;

    public TransmitMessageTypedSerializer(IValueConverterProvider valueConverterProvider)
    {
        _valueConverterProvider = valueConverterProvider;
    }

    public void Serialize(SerializationContext context)
    {
        context.Properties.Where(p => p.IsObject == false).ToList().ForEach(p => Serialize(p));
    }

    private void Serialize(SerializationProperty property)
    {
        if (property.Value is null)
            return;

        var valueConverter = _valueConverterProvider.Get(property.PropertyInfo);
        var value = valueConverter.Convert(property.Value);
        if (property.PropertyInfo.PostValueConverter != null)
            value = property.PropertyInfo.PostValueConverter.Invoke(value);
        property.SerializedValue = value;
    }
}
