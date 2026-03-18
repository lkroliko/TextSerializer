using TextSerializer.SharedTests;

namespace MrRabbit.TextSerializer.SharedTests.Builders;

public class TextSerializerOptionsBuilder
{
    private string? _separator = TestConsts.Separator;
    private string? _prefix = TestConsts.Prefix;
    private string? _suffix = TestConsts.Suffix;

    public TextSerializerOptions Build() => new TextSerializerOptions()
    {
        Prefix = _prefix,
        Separator = _separator,
        Suffix = _suffix,
    };

    public static implicit operator TextSerializerOptions(TextSerializerOptionsBuilder builder) => builder.Build();
}
