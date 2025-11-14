namespace MrRabbit.TextSerializer.Common.Interfaces;

internal interface ISerializerValidatorProvider
{
    IEnumerable<ISerializerValidator> Get(Type type);
}
