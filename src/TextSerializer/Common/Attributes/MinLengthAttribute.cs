namespace MrRabbit.TextSerializer.Common.Attributes;
/// <summary>
/// If property has min length it will be visible in IPropertyInfo.MinLength. It is used in default validator.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MinLengthAttribute : Attribute
{
    public int Value { get; }

    public MinLengthAttribute(int length)
    {
        Value = length;
    }
}
