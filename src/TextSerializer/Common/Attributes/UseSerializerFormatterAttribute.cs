namespace MrRabbit.TextSerializer.Common.Attributes;
/// <summary>
/// Value of property will be formatted on serialization by selected formatter.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class UseSerializerFormatterAttribute<T> : UseSerializerFormatterAttribute where T : class, ISerializerFormatter { }

public abstract class UseSerializerFormatterAttribute : Attribute { }
