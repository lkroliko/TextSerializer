namespace MrRabbit.TextSerializer.Serialization.Services;
internal class TextBuilderService : ITextBuilderService
{
    private readonly IOptions<TextSerializerOptions> _options;

    public TextBuilderService(IOptions<TextSerializerOptions> options)
    {
        _options = options;
    }

    public string Build(SerializationContext context)
    {
        var builder = new StringBuilder();
        builder.Append(_options.Value.Prefix);
        Build(context);
        builder.Append(_options.Value.Suffix);
        return builder.ToString();

        void Build(SerializationContext context)
        {
            foreach (var property in context.Properties)
            {
                if (property.IsObject)
                {
                    Build(property.Context!);
                }
                else
                {
                    builder.Append(property.SerializedValue);
                }

                if (context.ObjectType.IsMessage() && property != context.Properties.Last())
                    builder.Append(_options.Value.Separator);
            }
        }
    }
}
