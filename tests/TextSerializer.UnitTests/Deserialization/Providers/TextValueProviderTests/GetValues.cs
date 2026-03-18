using MrRabbit.TextSerializer.Deserialization.Providers;

namespace MrRabbit.TextSerializer.UnitTests.Deserialization.Providers.TextValueProviderTests;

[Trait("Category", nameof(TextValueProvider))]
public class GetValues
{
    private readonly TextValueProvider _provider;
    private IOptions<TextSerializerOptions> _options = Mock.Of<IOptions<TextSerializerOptions>>();
    private TextSerializerOptions _optionsValue = new()
    {
        Prefix = "<",
        Separator = "|",
        Suffix = ">",
    };

    public GetValues()
    {
        _provider = new(_options);

        Mock.Get(_options).Setup(x => x.Value).Returns(_optionsValue);
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void WhenCallThenResultIsValid(string text, string[] expected)
    {
        var result = _provider.GetValues(text);

        result.Should().BeEquivalentTo(expected);
    }

    public static TheoryData<string, string[]> TestData() => new()
    {
        {
            "<value>",
            ["value"]
        },
        {
            "<value1|value2>",
            ["value1","value2"]
        },
        {
            "<<value1>|<value2>>",
            ["<value1>", "<value2>"]
        },
    };
}
