namespace MrRabbit.TextSerializer.Deserialization.PreProcessor;
internal class DeserializerPreProcessor : IDeserializerPreProcessor
{
    private readonly Func<string, string> _process;

    public DeserializerPreProcessor(Func<string, string> process)
    {
        _process = process;
    }

    public string Process(string text) => _process(text);
}
