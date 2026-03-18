namespace MrRabbit.TextSerializer.Common.Interfaces;
internal interface IPropertyInfoProvider
{
    IEnumerable<IPropertyInfo> GetProperties(object value);
    IEnumerable<IPropertyInfo> GetProperties(Type type);
}
