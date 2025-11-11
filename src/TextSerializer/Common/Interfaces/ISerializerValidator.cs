namespace MrRabbit.TextSerializer.Common.Interfaces;
public interface ISerializerValidator
{
    void Validate(SerializationContext context);
}

public interface ISerializerValidator<TObjectType> : ISerializerValidator { }
