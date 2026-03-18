namespace MrRabbit.TextSerializer.Serialization.Services;

internal class SerializerService : ISerializerService
{
    private readonly ITypedSerializerProvider _typedSerializerProvider;

    public SerializerService(ITypedSerializerProvider typedSerializerProvider)
    {
        _typedSerializerProvider = typedSerializerProvider;
    }

    public void Serialize(SerializationContext context)
    {
        var serializer = _typedSerializerProvider.Get(context.ObjectType);
        serializer.Serialize(context);
        context.Properties.Where(s => s.IsObject).ToList().ForEach(s => Serialize(s.Context!));
    }
}
