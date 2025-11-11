namespace MrRabbit.TextSerializer.EndToEndTests.Common.Implementation;
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
internal class FixedLengthFormatterAttribute : UseSerializerFormatterAttribute<FixedLengthSerializatorFormatter> { }
