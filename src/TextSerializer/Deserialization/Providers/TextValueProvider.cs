namespace MrRabbit.TextSerializer.Deserialization.Providers;

internal class TextValueProvider : ITextValueProvider
{
    private readonly IOptions<TextSerializerOptions> _options;

    public TextValueProvider(IOptions<TextSerializerOptions> options)
    {
        _options = options;
    }

    public string[] GetValues(string text)
    {
        if (string.IsNullOrEmpty(_options.Value.Prefix) == false)
            text = text.TrimStart(_options.Value.Prefix.ToArray());
        if (string.IsNullOrEmpty(_options.Value.Suffix) == false)
            text = text.TrimEnd(_options.Value.Suffix.ToArray());
        return text.Split(_options.Value.Separator);
    }
}
