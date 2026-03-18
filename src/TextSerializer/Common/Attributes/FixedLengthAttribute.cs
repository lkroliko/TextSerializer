namespace MrRabbit.TextSerializer.Common.Attributes;
/// <summary>
/// If property has fixed length it will be visible in IPropertyInfo.FixedLength. It is used in default validator.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FixedLengthAttribute : Attribute
{
    public int Value { get; }

    public FixedLengthAttribute(int length)
    {
        Value = length;
    }
}
