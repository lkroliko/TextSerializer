namespace MrRabbit.TextSerializer.Serialization.Services;
internal class SerializerFormatterService : ISerializerFormatterService
{
    private readonly ISerializerFormatterProvider _provider;

    public SerializerFormatterService(ISerializerFormatterProvider provider)
    {
        _provider = provider;
    }

    public void Format(SerializationContext context) =>
        FormatContext(context);

    private void FormatContext(SerializationContext context)
    {
        context.Properties.Where(p => p.IsObject == false && p.PropertyInfo.SerializationFormatterTypes.Count() > 0).ToList().ForEach(p => FormatProperty(p));
        context.Properties.Where(p => p.IsObject).ToList().ForEach(p => FormatContext(p.Context!));
    }

    private void FormatProperty(SerializationProperty serializationProperty) =>
        serializationProperty.PropertyInfo.SerializationFormatterTypes.ToList().ForEach(formatterType =>
        {
            var formatter = _provider.Get(formatterType);
            formatter.Format(serializationProperty);
        });
}
