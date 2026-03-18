using AutoFixture;
using Microsoft.Extensions.Options;
using MrRabbit.TextSerializer.SharedTests.Builders;

namespace MrRabbit.TextSerializer.SharedTests;

public class A
{
    public static Fixture Fixture => new Fixture();
    public static DeserializationContextBuilder DeserializationContext => new();
    public static SerializationContextBuilder SerializationContext => new();
    public static SerializationPropertyBuilder SerializationProperty => new();
    public static TextSerializerOptionsBuilder TextSerializerOptions => new();

    public static class Moq
    {
        public static IOptions<TextSerializerOptions> TextSerializerOptions => GetTextSerializerOptions();
        private static IOptions<TextSerializerOptions> GetTextSerializerOptions()
        {
            var options = Mock.Of<IOptions<TextSerializerOptions>>();
            Mock.Get(options).Setup(x => x.Value).Returns(A.TextSerializerOptions);
            return options;
        }
    }
}
