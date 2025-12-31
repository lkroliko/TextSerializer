using MrRabbit.TextSerializer.Common.Enums;

namespace MrRabbit.TextSerializer.Common.Interfaces;

public interface IPropertyInfo
{
    Type PropertyType { get; }
    string Name { get; }
    bool IsNullableType { get; }
    Type? UnderlyingNullableType { get; }
    bool IsCollection { get; }
    bool IsEnum { get; }

    object? GetValue(object? obj);
    void SetValue(object? obj, object? value);

    T? GetCustomAttribute<T>() where T : Attribute;

    bool IsContextProperty { get; }
    bool IsMandatory { get; }
    bool IsOptional { get; }
    bool IsConditional { get; }
    int? MaxLength { get; }
    int? MinLength { get; }
    int? FixedLength { get; }
    Position? Position { get; }

    IEnumerable<Type> SerializationFormatterTypes { get; }
    Type? ValueConverterType { get; }
}
