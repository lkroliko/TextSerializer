namespace MrRabbit.TextSerializer.Deserialization.Factories;

internal class ObjectFactory : IObjectFactory
{
    public object Get(Type type)
    {
        try
        {
            return Activator.CreateInstance(type)!;
        }
        catch (Exception ex)
        {
            throw new TextSerializerException($"Unable to create instance of object of type '{type.FullName}.", ex);
        }
    }
}
