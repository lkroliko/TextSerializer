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
            text = text.StartsWith(_options.Value.Prefix) ? text[_options.Value.Prefix.Length..] : text;
        if (string.IsNullOrEmpty(_options.Value.Suffix) == false)
            text = text.EndsWith(_options.Value.Suffix) ? text[..^_options.Value.Suffix.Length] : text;
        return text.Split(_options.Value.Separator);
    }
}
