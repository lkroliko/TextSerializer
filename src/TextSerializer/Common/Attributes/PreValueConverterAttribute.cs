namespace MrRabbit.TextSerializer.Common.Attributes;
public class PreValueConverterAttribute : Attribute//TODO add to doc
{
    internal Func<string, string> Pre { get; }

    public PreValueConverterAttribute(Func<string, string> pre)
    {
        Pre = pre;
    }
}
