namespace MrRabbit.TextSerializer.Common.Interfaces;

internal interface IValueConverterProvider
{
    IValueConverter GetByTypeToConvert(Type type);
    IValueConverter GetByValueConverterType(Type type);
    IValueConverter Get(IPropertyInfo propertyInfo);
}

