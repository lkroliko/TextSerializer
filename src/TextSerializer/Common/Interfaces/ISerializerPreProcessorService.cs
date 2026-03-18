namespace MrRabbit.TextSerializer.Common.Interfaces;

internal interface ISerializerPreProcessorService
{
    void Run(SerializationContext context);
}
