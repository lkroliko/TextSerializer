using MrRabbit.TextSerializer.Serialization.Services;

namespace MrRabbit.TextSerializer.UnitTests.Serialization.Services.TextBuilderServiceTests;
[Trait("Category", "TextBuilderService")]
public class Get
{
    private readonly IOptions<TextSerializerOptions> _options = A.Moq.TextSerializerOptions;
    private readonly TextBuilderService _factory;

    public Get()
    {
        _factory = new TextBuilderService(_options);
    }

    [Theory]
    [MemberData(nameof(TestData))]
    internal void WhenCalledThenMessageFrameCreated(SerializationContext context, string expected)
    {
        _factory.Build(context);

        var result = context.Builder!.ToString();
        result.Should().Be(expected);
    }

    public static TheoryData<SerializationContext, string> TestData =>
        new()
        {
            {
                A.SerializationContext.WithType(typeof(TransmitMessage))
                    .WithProperty(builder => builder.WithSerializedValue("Prop1")),
                "<Prop1>"
            },
            {
                A.SerializationContext.WithType(typeof(TransmitMessage))
                    .WithProperty(builder => builder.WithSerializedValue("Prop1"))
                    .WithProperty(builder => builder.WithSerializedValue("Prop2")),
                "<Prop1|Prop2>"
            },
            {
                A.SerializationContext.WithType(typeof(TransmitMessage))
                    .WithProperty(builder => builder.WithSerializedValue("Prop1"))
                    .WithProperty(builder => builder.WithContext(builder => builder
                        .WithProperty(builder => builder.WithSerializedValue("ValueObjectProp1")))),
                "<Prop1|ValueObjectProp1>"
            },
            {
                A.SerializationContext.WithType(typeof(TransmitMessage))
                    .WithProperty(builder => builder.WithSerializedValue("Prop1"))
                    .WithProperty(builder => builder.WithContext(builder => builder
                        .WithProperty(builder => builder.WithSerializedValue("ValueObjectProp1"))
                        .WithProperty(builder => builder.WithSerializedValue("ValueObjectProp2")))),
                "<Prop1|ValueObjectProp1ValueObjectProp2>"
            },
            {
                A.SerializationContext.WithType(typeof(TransmitMessage))
                    .WithProperty(builder => builder.WithSerializedValue("Prop1"))
                    .WithProperty(builder => builder.WithSerializedValue("Prop2"))
                    .WithProperty(builder => builder.WithContext(builder => builder
                        .WithProperty(builder => builder.WithSerializedValue("ValueObjectProp1"))
                        .WithProperty(builder => builder.WithSerializedValue("ValueObjectProp2")))),
                "<Prop1|Prop2|ValueObjectProp1ValueObjectProp2>"
            },
        };
}
