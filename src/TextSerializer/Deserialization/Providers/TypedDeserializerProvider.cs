namespace MrRabbit.TextSerializer.Deserialization.Providers;

internal class TypedDeserializerProvider : ITypedDeserializerProvider
{
    private Dictionary<Type, ITypedDeserializer> _typedDeserializers = new();

    public TypedDeserializerProvider(IEnumerable<ITypedDeserializer> typedObjects)
    {
        typedObjects.ToList().ForEach(typedDeserializer =>
        {
            var typedDeserializerType = typedDeserializer.GetType().GetGenericInterfaceTypeArgument(typeof(ITypedDeserializer<>), 0);
            if (_typedDeserializers.ContainsKey(typedDeserializerType) == false)
                _typedDeserializers.Add(typedDeserializerType, typedDeserializer);
        });
    }

    public ITypedDeserializer Get(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableTo(typeof(System.Collections.ICollection)))
            return GetForCollection(type);

        return GetForObject(type);
    }

    private ITypedDeserializer GetForCollection(Type type)
    {
        var searchType = type;
        var genericType = type.GetGenericTypeDefinition();
        var genericTypeArgument = type.GetGenericArguments()[0]!;

        do
        {
            if (_typedDeserializers.ContainsKey(searchType!))
            {
                return _typedDeserializers[searchType!];
            }
            genericTypeArgument = genericTypeArgument!.BaseType;
            searchType = genericTypeArgument is null ? null : genericType.MakeGenericType(genericTypeArgument);
        }
        while (searchType is not null);

        throw new TextSerializerException($"No implemented 'ITypedDeserializer<{type.Name}>'.");
    }

    private ITypedDeserializer GetForObject(Type type)
    {
        var searchType = type;
        do
        {
            if (_typedDeserializers.ContainsKey(searchType!))
            {
                return _typedDeserializers[searchType!];
            }
            searchType = searchType!.BaseType;
        }
        while (searchType is not null);

        throw new TextSerializerException($"No implemented 'ITypedDeserializer<{type.Name}>'.");
    }
}
