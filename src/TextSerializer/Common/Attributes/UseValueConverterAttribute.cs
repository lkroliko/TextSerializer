namespace MrRabbit.TextSerializer.Common.Attributes;
/// <summary>
/// Value of property will be converted by selected converter.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class UseValueConverterAttribute<T> : UseValueConverterAttribute where T : class, IValueConverter { }

public abstract class UseValueConverterAttribute : Attribute { }
