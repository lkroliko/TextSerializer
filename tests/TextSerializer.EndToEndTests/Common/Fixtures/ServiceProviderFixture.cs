using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using TextSerializer.SharedTests;

namespace MrRabbit.TextSerializer.EndToEndTests.Common.Fixtures;
public class ServiceProviderFixture
{
    public ServiceProvider ServiceProvider { get; private set; } = default!;
    protected ServiceCollection ServiceCollection = new();
    public ITextSerializer TextSerializer => ServiceProvider.GetRequiredService<ITextSerializer>();

    public ServiceProviderFixture()
    {
        BuildServiceCollection();
        BuildServiceProvider();
    }

    private void BuildServiceCollection()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Debug()
            .CreateLogger();
        ServiceCollection.AddLogging(options => options.SetMinimumLevel(LogLevel.Debug).ClearProviders().AddSerilog());

        ServiceCollection.AddTextSerializer(options =>
            options.UseDefaultValueConverters()
                .UseDefaultSerializationValidators()
                .Configure(options =>
                {
                    options.Prefix = TestConsts.Prefix;
                    options.Separator = TestConsts.Separator;
                    options.Suffix = TestConsts.Suffix;
                })
                .AddMessageIdProvider(text => text.TrimStart('<').Split('|')[0])
                .RegisterReceiveMessages(Assembly.GetExecutingAssembly())
                .AddDeserializerPreProcessor(text => text.Replace("#CRC", string.Empty))
                .AddSerializerPostProcessor(text => text.Replace(">", "#CRC>"))
                .AddTypedDeserializer<SimpleValueObjectTypedDeserializer>()
                .AddSerializerFormatter<FixedLengthSerializatorFormatter>()
                .AddValueConverter<CustomStringValueConverter>());

    }

    protected virtual void BuildServiceProvider() => ServiceProvider = ServiceCollection.BuildServiceProvider();
}
