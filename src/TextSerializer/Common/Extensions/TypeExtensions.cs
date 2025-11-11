namespace MrRabbit.TextSerializer.Common.Extensions;
internal static class TypeExtensions
{
    internal static bool IsMessage(this Type type) => type.IsAssignableTo(typeof(Message));

    private static bool IsImplementingGenericType(this Type type, Type genericType)
    {
        var searchType = type;

        while (searchType is not null)
        {
            if (searchType.IsGenericType == true && searchType.GetGenericTypeDefinition() == genericType)
                return true;
            searchType = searchType.BaseType;
        }

        return false;
    }

    internal static bool IsImplementingGenericInterfaceType(this Type type, Type genericType) =>
        type.GetInterfaces().Where(x => x.IsGenericType).Any(x => x.GetGenericTypeDefinition() == genericType);

    internal static Type GetUnderlyingNullableType(this Type type)
    {
        var underlyingType = Nullable.GetUnderlyingType(type);
        if (underlyingType is null)
            throw new TextSerializerException($"Type {type.Name} is not Nullable<> type.");
        return underlyingType;
    }

    internal static Type GetGenericInterfaceTypeArgument(this Type type, Type interfaceGenericType, int index)
        => type.GetInterfaces().Where(i => i.IsGenericType == true && i.GetGenericTypeDefinition() == interfaceGenericType).First().GetGenericArguments()[index];

    internal static bool IsEnumOrGenericTypeIsEnum(this Type type)
    {
        if (type.IsGenericType)
        {
            return type.GenericTypeArguments[0].IsEnum;
        }

        return type.IsEnum;
    }

    internal static Type GetNestedGenericType(this Type type, Type genericType)
    {
        var tempType = type;
        while (tempType is not null)
        {
            if (tempType.IsGenericType && tempType.GetGenericTypeDefinition() == genericType)
                return tempType;
            tempType = tempType.BaseType!;
        }

        throw new TextSerializerException($"Type '{genericType.Name}' is not found in '{type.Name}'.");
    }
}
