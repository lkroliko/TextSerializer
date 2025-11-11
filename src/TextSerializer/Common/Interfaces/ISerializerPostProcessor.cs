namespace MrRabbit.TextSerializer.Common.Interfaces;

public interface ISerializerPostProcessor
{
    void Process(SerializationContext context);
}
