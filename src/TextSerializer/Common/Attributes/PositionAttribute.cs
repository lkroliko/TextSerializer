using MrRabbit.TextSerializer.Common.Enums;

namespace MrRabbit.TextSerializer.Common.Attributes;
/// <summary>
/// Change property position in property list. Helpful in base class of receive messages to set e.g. message id once.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class PositionAttribute : Attribute
{
    public Position Position { get; }

    public PositionAttribute(Position position)
    {
        Position = position;
    }
}
