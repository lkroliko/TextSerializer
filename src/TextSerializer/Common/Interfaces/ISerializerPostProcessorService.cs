namespace MrRabbit.TextSerializer.Common.Interfaces;

internal interface ISerializerPostProcessorService
{
    void Run(SerializationContext context);
}
