namespace MrRabbit.TextSerializer.Serialization.Services;
internal class SerializerValidatorService : ISerializerValidatorService
{
    private readonly ISerializerValidatorProvider _serializerValidatorProvider;

    public SerializerValidatorService(ISerializerValidatorProvider serializerValidatorProvider)
    {
        _serializerValidatorProvider = serializerValidatorProvider;
    }

    public void Validate(SerializationContext context)
    {
        foreach (var property in context.Properties)
        {
            if (property.IsObject)
            {
                Validate(property.Context!);
            }
        }

        var validators = _serializerValidatorProvider.Get(context.ObjectType);

        validators.ToList().ForEach(i => i.Validate(context));
    }
}
