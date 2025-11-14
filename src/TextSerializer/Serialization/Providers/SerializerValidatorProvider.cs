namespace MrRabbit.TextSerializer.Serialization.Providers;
internal class SerializerValidatorProvider : ISerializerValidatorProvider
{
    private List<(Type Type, ISerializerValidator Validator)> _validators = new();

    public SerializerValidatorProvider(IEnumerable<ISerializerValidator> validators)
    {
        validators.ToList().ForEach(v => _validators.Add(new(v.GetType().GetGenericInterfaceTypeArgument(typeof(ISerializerValidator<>), 0), v)));
    }

    public IEnumerable<ISerializerValidator> Get(Type type)
    {
        var validators = new List<ISerializerValidator>();
        while (type is not null)
        {
            validators.AddRange(_validators.Where(v => v.Type == type).Select(v => v.Validator));
            type = type.BaseType!;
        }

        return validators;
    }
}
