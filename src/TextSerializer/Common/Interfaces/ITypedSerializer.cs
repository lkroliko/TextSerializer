namespace MrRabbit.TextSerializer.Common.Interfaces;

public interface ITypedSerializer
{
    void Serialize(SerializationContext context);
}

public interface ITypedSerializer<TType> : ITypedSerializer { }
