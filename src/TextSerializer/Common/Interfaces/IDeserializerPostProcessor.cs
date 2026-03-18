namespace MrRabbit.TextSerializer.Common.Interfaces;
public interface IDeserializerPostProcessor
{
    void Process(DeserializationContext context);
}
