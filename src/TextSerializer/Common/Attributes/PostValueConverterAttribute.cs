namespace MrRabbit.TextSerializer.Common.Attributes;
public class PostValueConverterAttribute : Attribute //TODO add doc
{
    internal Func<string, string> Post { get; }

    public PostValueConverterAttribute(Func<string, string> post)
    {
        Post = post;
    }
}
