namespace MrRabbit.TextSerializer.Common.Attributes;
/// <summary>
/// If property is marked as Optional it will be visible in IPropertyInfo.IsOptional. It is used in default validators.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class OptionalAttribute : Attribute { }
