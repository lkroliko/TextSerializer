using MrRabbit.TextSerializer.Common;

namespace MrRabbit.TextSerializer.EndToEndTests.Common.Implementation;

internal class CrcSerializerPostProcessor : ISerializerPostProcessor
{
    public void Process(SerializationContext context)
    {
        context.Builder!.Remove(context.Builder.Length - 1, 1);
        context.Builder.Append("#CRC>");
    }
}
