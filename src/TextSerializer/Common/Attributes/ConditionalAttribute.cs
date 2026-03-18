namespace MrRabbit.TextSerializer.Common.Attributes;
/// <summary>
/// If property is marked as conditional it will be visible in IPropertyInfo.IsConditional. It is used in default validators.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ConditionalAttribute : Attribute { }
