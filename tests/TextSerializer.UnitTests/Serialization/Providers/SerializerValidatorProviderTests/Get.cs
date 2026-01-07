using MrRabbit.TextSerializer.Serialization.Providers;

namespace MrRabbit.TextSerializer.UnitTests.Serialization.Providers.SerializerValidatorProviderTests;

[Trait("Category", nameof(SerializerValidatorProvider))]
public class Get
{
    private readonly SerializerValidatorProvider _provider;
    List<ISerializerValidator> _validators = [];
    private readonly FakeTransmitMessageSerializerValidator _validator = new();

    public Get()
    {
        _validators.Add(_validator);
        _provider = new(_validators);
    }

    [Fact]
    public void WhenCallThenResultIsValid()
    {
        var result = _provider.Get(typeof(TransmitMessage));

        result.Should().Contain(_validator);
    }
}
