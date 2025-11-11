using MrRabbit.TextSerializer.Common.Enums;

namespace MrRabbit.TextSerializer.Services;
internal class PropertyInfoProvider : IPropertyInfoProvider
{
    private readonly Dictionary<Type, List<IPropertyInfo>> _propertyInfos = new();

    private List<IPropertyInfo> BuildMessagePropertyInfos(Type instatnceType)
    {
        var properties = instatnceType.GetProperties().ToList().Select(p => CreatePropertyInfo(p)).ToList();
        return OrderProperties(properties);
    }

    private List<IPropertyInfo> OrderProperties(List<IPropertyInfo> properties)
    {
        var firstProperty = properties.FirstOrDefault(p => p.Position == Position.First);
        if (firstProperty is not null)
            MoveToStart(firstProperty);
        var penultimateProperty = properties.FirstOrDefault(p => p.Position == Position.Penultimate);
        if (penultimateProperty is not null)
            MoveToEnd(penultimateProperty);
        var lastProperty = properties.FirstOrDefault(p => p.Position == Position.Last);
        if (lastProperty is not null)
            MoveToEnd(lastProperty);

        return properties;

        void MoveToEnd(IPropertyInfo propertyInfo)
        {
            properties.Remove(propertyInfo);
            properties.Add(propertyInfo);
        }

        void MoveToStart(IPropertyInfo propertyInfo)
        {
            properties.Remove(propertyInfo);
            properties.Insert(0, propertyInfo);
        }
    }

    private IPropertyInfo CreatePropertyInfo(System.Reflection.PropertyInfo p)
    {
        return new PropertyInfo(p);
    }

    public IEnumerable<IPropertyInfo> GetProperties(object instance) =>
        GetProperties(instance.GetType());

    public IEnumerable<IPropertyInfo> GetProperties(Type type)
    {
        if (_propertyInfos.ContainsKey(type) == false)
            _propertyInfos[type] = BuildMessagePropertyInfos(type);

        return _propertyInfos[type];
    }
}
