namespace MrRabbit.TextSerializer.Serialization.PostProcessors;
internal class SerializationPostProcessor : ISerializerPostProcessor
{
    private readonly Func<string, string> _process;

    public SerializationPostProcessor(Func<string, string> process)
    {
        _process = process;
    }

    public string Process(string text) => _process(text);
}
