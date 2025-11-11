namespace MrRabbit.TextSerializer.Common.Interfaces;

internal interface IObjectFactory
{
    object Get(Type type);
}
