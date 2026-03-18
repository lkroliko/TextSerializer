namespace MrRabbit.TextSerializer.Common.Interfaces;
public interface ISerializerValidator
{
    void Validate(SerializationContext context);
}

/// <summary>
/// Validators can check object property value and serialized value and throw exception if is not correct.
/// </summary>
/// <typeparam name="TObjectType">Is context property type or is Message type.</typeparam>
public interface ISerializerValidator<TObjectType> : ISerializerValidator { }
