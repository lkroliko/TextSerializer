namespace MrRabbit.TextSerializer.Serialization.Providers;

internal class SerializerFormatterProvider : ISerializerFormatterProvider
{
    private Dictionary<Type, ISerializerFormatter> _formatters = new();

    public SerializerFormatterProvider(IEnumerable<ISerializerFormatter> formatters, ILogger<SerializerFormatterProvider> logger)
    {
        foreach (var formatter in formatters)
        {
            var type = formatter.GetType();
            if (_formatters.ContainsKey(type))
            {
                logger.LogWarning("ISerializerFormatter of type {@Name} is registered multiple times.", type.Name);
                continue;
            }
            _formatters[type] = formatter;
        }
    }

    public ISerializerFormatter Get(Type type)
    {
        if (_formatters.ContainsKey(type))
            return _formatters[type];
        throw new TextSerializerException($"ISerializerFormatter with type '{type.Name}' is not registered.");
    }
}
