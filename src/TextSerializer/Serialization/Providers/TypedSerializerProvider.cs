namespace MrRabbit.TextSerializer.Serialization.Providers;

internal class TypedSerializerProvider : ITypedSerializerProvider
{
    private Dictionary<Type, ITypedSerializer> _typedSerializers = new();

    public TypedSerializerProvider(IEnumerable<ITypedSerializer> typedSerializers)
    {
        typedSerializers.ToList().ForEach(typedSerializer =>
        {
            var typedSerializerType = typedSerializer.GetType().GetGenericInterfaceTypeArgument(typeof(ITypedSerializer<>), 0);
            if (_typedSerializers.ContainsKey(typedSerializerType) == false)
                _typedSerializers.Add(typedSerializerType, typedSerializer); ;
        });
    }

    public ITypedSerializer Get(Type type)
    {
        var searchType = type;
        do
        {
            if (_typedSerializers.ContainsKey(searchType!))
            {
                return _typedSerializers[searchType!];
            }
            searchType = searchType!.BaseType;
        }
        while (searchType is not null);

        throw new TextSerializerException($"No implemented 'ITypedSerializer<{type.Name}>'.");
    }
}
