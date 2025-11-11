namespace MrRabbit.TextSerializer.EndToEndTests.Common.Implementation;
internal class CrcDeserializerPreProcessor : IDeserializerPreProcessor
{
    public string Process(string text) => text.Replace("#CRC", string.Empty);
}
