using System.Collections;
using System.Reflection;
using MrRabbit.TextSerializer.Common.Enums;

namespace MrRabbit.TextSerializer.Common;

internal class PropertyInfo : IPropertyInfo
{
    private readonly System.Reflection.PropertyInfo _propertyInfo;

    public Type PropertyType => _propertyInfo.PropertyType;
    public string Name => _propertyInfo.Name;
    public bool IsContextProperty { get; }
    public bool IsNullableType => UnderlyingNullableType is not null;
    public Type? UnderlyingNullableType { get; }
    public bool IsCollection => _propertyInfo.PropertyType.IsAssignableTo(typeof(ICollection));
    public bool IsEnum => _propertyInfo.PropertyType.IsEnum;

    public bool IsMandatory => IsOptional == false && IsConditional == false;
    public bool IsOptional { get; }
    public bool IsConditional { get; }
    public int? MaxLength { get; }
    public int? MinLength { get; }
    public int? FixedLength { get; }
    public Position? Position { get; }
    public IEnumerable<Type> SerializationFormatterTypes { get; }
    public Type? ValueConverterType { get; }

    public PropertyInfo(System.Reflection.PropertyInfo propertyInfo)
    {
        _propertyInfo = propertyInfo;

        UnderlyingNullableType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);

        var optionalAttribute = _propertyInfo.GetCustomAttribute<OptionalAttribute>();
        IsOptional = optionalAttribute is not null;
        var conditionalAttribute = _propertyInfo.GetCustomAttribute<ConditionalAttribute>();
        IsConditional = conditionalAttribute is not null;
        var contextProperty = _propertyInfo.GetCustomAttribute<ContextPropertyAttribute>();
        IsContextProperty = contextProperty is not null;

        MaxLength = _propertyInfo.GetCustomAttribute<MaxLengthAttribute>()?.Value;
        MinLength = _propertyInfo.GetCustomAttribute<MinLengthAttribute>()?.Value;
        FixedLength = _propertyInfo.GetCustomAttribute<FixedLengthAttribute>()?.Value;

        Position = _propertyInfo.GetCustomAttribute<PositionAttribute>()?.Position;

        SerializationFormatterTypes = _propertyInfo.GetCustomAttributes<UseSerializerFormatterAttribute>().Select(a => a.GetType().GetNestedGenericType(typeof(UseSerializerFormatterAttribute<>)).GetGenericArguments()[0]).ToList();
        ValueConverterType = _propertyInfo.GetCustomAttribute<UseValueConverterAttribute>()?.GetType().GetNestedGenericType(typeof(UseValueConverterAttribute<>)).GetGenericArguments()[0];
    }

    public object? GetValue(object? obj) => _propertyInfo.GetValue(obj);
    public void SetValue(object? obj, object? value) => _propertyInfo.SetValue(obj, value);

    public T? GetCustomAttribute<T>() where T : Attribute => _propertyInfo.GetCustomAttribute<T>();
}
