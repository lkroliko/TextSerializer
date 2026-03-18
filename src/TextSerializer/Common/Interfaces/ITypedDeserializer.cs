namespace MrRabbit.TextSerializer.Common.Interfaces;

public interface ITypedDeserializer
{
    void Deserialize(DeserializationContext context);
}

public interface ITypedDeserializer<TType> : ITypedDeserializer { }
