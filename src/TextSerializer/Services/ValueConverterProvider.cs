using MrRabbit.TextSerializer.Common.ValueConverters;

namespace MrRabbit.TextSerializer.Services;

internal class ValueConverterProvider : IValueConverterProvider
{
    private readonly Dictionary<Type, IValueConverter> _valueConvertersByTypeToConvert = [];
    private readonly Dictionary<Type, IValueConverter> _valueConvertersByConverterType = [];

    public ValueConverterProvider(IEnumerable<IValueConverter> valueConverters, ILogger<ValueConverterProvider> logger)
    {
        valueConverters.ToList().ForEach(valueConverter =>
        {
            var valueConverterType = valueConverter.GetType();
            if (_valueConvertersByConverterType.ContainsKey(valueConverterType))
            {
                logger.LogWarning("IValueConverter is allready registered. Implemented value converter of type {1} is skipped.", valueConverterType.Name);
            }
            else
            {
                _valueConvertersByConverterType.Add(valueConverter.GetType(), valueConverter);
            }

            if (valueConverterType.IsImplementingGenericInterfaceType(typeof(IValueConverter<>)))
            {
                var valueConverterTypeToConvert = valueConverterType.GetGenericInterfaceTypeArgument(typeof(IValueConverter<>), 0);
                if (_valueConvertersByTypeToConvert.ContainsKey(valueConverterTypeToConvert))
                {
                    logger.LogWarning("IValueConverter<{0}> is allready registered. Implemented value converter of type {1} is skipped.", valueConverterTypeToConvert.Name, valueConverter.GetType().Name);
                }
                else
                {
                    _valueConvertersByTypeToConvert.Add(valueConverterTypeToConvert, valueConverter);
                }
            }
        });
    }

    public IValueConverter Get(IPropertyInfo propertyInfo)
    {
        if (propertyInfo.ValueConverterType is not null)
            return GetByValueConverterType(propertyInfo.ValueConverterType);
        if (_valueConvertersByTypeToConvert.ContainsKey(propertyInfo.PropertyType))
            return _valueConvertersByTypeToConvert[propertyInfo.PropertyType];
        if (propertyInfo.IsEnum && propertyInfo.IsNullableType == false)
            return _valueConvertersByConverterType[(typeof(EnumValueConverter))];
        if (propertyInfo.IsEnum && propertyInfo.IsNullableType)
            return _valueConvertersByConverterType[(typeof(NullableEnumValueConverter))];

        throw new TextSerializerException($"No implemented 'IValueConverter<{propertyInfo.PropertyType.Name}>'.");
    }

    public IValueConverter GetByTypeToConvert(Type type)
    {
        if (_valueConvertersByTypeToConvert.TryGetValue(type, out var valueConverter))
            return valueConverter;
        throw new TextSerializerException($"No implemented 'IValueConverter<{type.Name}>'.");
    }

    public IValueConverter GetByValueConverterType(Type type)
    {
        if (_valueConvertersByConverterType.TryGetValue(type, out var valueConverter))
            return valueConverter;
        throw new TextSerializerException($"No registerd IValueConverter of type '{type.Name}'.");
    }
}

