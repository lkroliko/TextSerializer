namespace MrRabbit.TextSerializer.Serialization.Services;
internal class Serializer : ISerializer
{
    private readonly ISerializerService _serializer;
    private readonly ISerializerPostProcessorService _serializerPostProcessor;
    private readonly ISerializerValidatorService _serializerValidator;
    private readonly ISerializerFormatterService _serializerFormatter;
    private readonly ISerializationContextFactory _serializationContextFactory;
    private readonly ITextBuilderService _textBuilder;


    public Serializer(ISerializerService serializer, ISerializerPostProcessorService serializerPostProcessor, ISerializerValidatorService serializerValidator,
        ISerializerFormatterService serializationFormatter, ISerializationContextFactory serializationContextFactory, ITextBuilderService textBuilderService)
    {
        _serializer = serializer;
        _serializerPostProcessor = serializerPostProcessor;
        _serializerValidator = serializerValidator;
        _serializerFormatter = serializationFormatter;
        _serializationContextFactory = serializationContextFactory;
        _textBuilder = textBuilderService;
    }

    public string Serialize(TransmitMessage value)
    {
        var context = GetSerializationContext(value);
        Validate(context);
        Serialize(context);
        Format(context);
        BuildText(context);
        RunPostProcess(context);//TODO zminić na pre process? 
        return context.Builder!.ToString();
    }

    private void Serialize(SerializationContext context) => _serializer.Serialize(context);

    private void Format(SerializationContext context) => _serializerFormatter.Format(context);

    private void Validate(SerializationContext context) => _serializerValidator.Validate(context);

    private void RunPostProcess(SerializationContext context) => _serializerPostProcessor.Run(context);

    private void BuildText(SerializationContext context) => _textBuilder.Build(context);

    private SerializationContext GetSerializationContext(object value) => _serializationContextFactory.Get(value);
}
