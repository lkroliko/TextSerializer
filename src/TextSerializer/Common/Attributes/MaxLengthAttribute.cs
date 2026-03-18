namespace MrRabbit.TextSerializer.Common.Attributes;
/// <summary>
/// If property has max length it will be visible in IPropertyInfo.MaxLength. It is used in default validator.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MaxLengthAttribute : Attribute
{
    public int Value { get; }

    public MaxLengthAttribute(int maxLength)
    {
        Value = maxLength;
    }
}
