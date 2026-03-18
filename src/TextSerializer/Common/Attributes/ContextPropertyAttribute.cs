namespace MrRabbit.TextSerializer.Common.Attributes;
/// <summary>
/// If property is marked as Context it will require implementation of ITypedDeserializer<TType> and ITypedSerializer<TType>.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ContextPropertyAttribute : Attribute { }
