namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTextSerializer(this IServiceCollection services, Action<TextSerializerOptionsBuilder> builder)
    {
        var builderInstatnce = new TextSerializerOptionsBuilder(services);
        builder.Invoke(builderInstatnce);
        builderInstatnce.Build();

        return services;
    }
}
