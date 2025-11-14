namespace MrRabbit.TextSerializer.Serialization.Services;
internal class Serializer : ISerializer
{
    private readonly ISerializerService _serializer;
    private readonly ISerializerPreProcessorService _serializerPreProcessor;
    private readonly ISerializerPostProcessorService _serializerPostProcessor;
    private readonly ISerializerValidatorService _serializerValidator;
    private readonly ISerializerFormatterService _serializerFormatter;
    private readonly ISerializationContextFactory _serializationContextFactory;
    private readonly ITextBuilderService _textBuilder;


    public Serializer(ISerializerService serializer, ISerializerPreProcessorService serializerPreProcessor, ISerializerPostProcessorService serializerPostProcessor, ISerializerValidatorService serializerValidator,
        ISerializerFormatterService serializationFormatter, ISerializationContextFactory serializationContextFactory, ITextBuilderService textBuilderService)
    {
        _serializer = serializer;
        _serializerPreProcessor = serializerPreProcessor;
        _serializerPostProcessor = serializerPostProcessor;
        _serializerValidator = serializerValidator;
        _serializerFormatter = serializationFormatter;
        _serializationContextFactory = serializationContextFactory;
        _textBuilder = textBuilderService;
    }

    public string Serialize(TransmitMessage value)
    {
        var context = GetSerializationContext(value);
        Serialize(context);
        Format(context);
        Validate(context);
        RunPreProcess(context);
        var text = BuildText(context);
        return RunPostProcess(text);
    }

    private void Serialize(SerializationContext context) => _serializer.Serialize(context);

    private void Format(SerializationContext context) => _serializerFormatter.Format(context);

    private void Validate(SerializationContext context) => _serializerValidator.Validate(context);

    private void RunPreProcess(SerializationContext context) => _serializerPreProcessor.Run(context);

    private string RunPostProcess(string text) => _serializerPostProcessor.Run(text);

    private string BuildText(SerializationContext context) => _textBuilder.Build(context);

    private SerializationContext GetSerializationContext(object value) => _serializationContextFactory.Get(value);
}
