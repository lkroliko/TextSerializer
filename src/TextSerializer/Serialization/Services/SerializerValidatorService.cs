namespace MrRabbit.TextSerializer.Serialization.Services;
internal class SerializerValidatorService : ISerializerValidatorService
{
    private List<(Type Type, ISerializerValidator Validator)> _validators = new();

    public SerializerValidatorService(IEnumerable<ISerializerValidator> validators)
    {
        validators.ToList().ForEach(v => _validators.Add(new(v.GetType().GetGenericInterfaceTypeArgument(typeof(ISerializerValidator<>), 0), v)));
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

        var searchType = context.ObjectType;
        var validators = new List<ISerializerValidator>();
        while (searchType is not null)
        {
            validators.AddRange(_validators.Where(v => v.Type == searchType).Select(v => v.Validator));
            searchType = searchType.BaseType;
        }

        validators.ForEach(i => i.Validate(context));
    }
}
