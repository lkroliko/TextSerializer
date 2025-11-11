namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTextSerializer(this IServiceCollection services, Action<TextSerializerOptionsBuilder> options)
    {
        var builder = new TextSerializerOptionsBuilder(services);
        options.Invoke(builder);
        builder.Build();

        return services;
    }
}
