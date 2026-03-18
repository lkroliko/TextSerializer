namespace MrRabbit.TextSerializer.Common.Interfaces;

internal interface IDeserializerPostProcessorService
{
    void Run(DeserializationContext context);
}
