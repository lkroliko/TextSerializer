using System.Reflection;
using MrRabbit.TextSerializer.Common.ValueConverters;
using MrRabbit.TextSerializer.Deserialization.Factories;
using MrRabbit.TextSerializer.Deserialization.Providers;
using MrRabbit.TextSerializer.Deserialization.Services;
using MrRabbit.TextSerializer.Deserialization.TypedDeserializers;
using MrRabbit.TextSerializer.Serialization.Factories;
using MrRabbit.TextSerializer.Serialization.Providers;
using MrRabbit.TextSerializer.Serialization.Services;
using MrRabbit.TextSerializer.Serialization.TypedSerializers;
using MrRabbit.TextSerializer.Serialization.Validators;
using MrRabbit.TextSerializer.Services;

namespace Microsoft.Extensions.DependencyInjection;

public class TextSerializerOptionsBuilder
{
    private readonly IServiceCollection _services;

    internal TextSerializerOptionsBuilder(IServiceCollection services)
    {
        _services = services;
    }

    internal void Build()
    {
        _services.AddSingleton<IDeserializationContextFactory, DeserializationContextFactory>();
        _services.AddSingleton<IDeserializer, Deserializer>();
        _services.AddSingleton<IObjectFactory, MrRabbit.TextSerializer.Deserialization.Factories.ObjectFactory>();
        _services.AddSingleton<IPropertyInfoProvider, PropertyInfoProvider>();
        _services.AddSingleton<ISerializationContextFactory, SerializationContextFactory>();
        _services.AddSingleton<ISerializer, Serializer>();
        _services.AddSingleton<ISerializerFormatterProvider, SerializerFormatterProvider>();
        _services.AddSingleton<ISerializerFormatterService, SerializerFormatterService>();
        _services.AddSingleton<ISerializerPostProcessorProvider, SerializerPostProcessorProvider>();
        _services.AddSingleton<ISerializerPostProcessorService, SerializerPostProcessorService>();
        _services.AddSingleton<ISerializerService, SerializerService>();
        _services.AddSingleton<ISerializerValidatorService, SerializerValidatorService>();
        _services.AddSingleton<ITextBuilderService, TextBuilderService>();
        _services.AddSingleton<ITextSerializer, MrRabbit.TextSerializer.Services.TextSerializer>();
        _services.AddSingleton<ITypedDeserializer, ReceiveMessageTypedDeserializer>();
        _services.AddSingleton<ITypedDeserializerProvider, TypedDeserializerProvider>();
        _services.AddSingleton<ITypedSerializer, TransmitMessageTypedSerializer>();
        _services.AddSingleton<ITypedSerializerProvider, TypedSerializerProvider>();
        _services.AddSingleton<IValueConverterProvider, ValueConverterProvider>();
        _services.AddSingleton<ITextValueProvider, TextValueProvider>();
        _services.AddSingleton<IDeserializerPostProcessorService, DeserializerPostProcessorService>();
        _services.AddSingleton<IDeserializerPostProcessorProvider, DeserializerPostProcessorProvider>();
        _services.AddSingleton<IDeserializerPreProcessorService, DeserializerPreProcessorService>();
        _services.AddSingleton<IDeserializerPreProcessorProvider, DeserializerPreProcessorProvider>();
        _services.AddSingleton<IValueConverter, EnumValueConverter>();
    }

    public TextSerializerOptionsBuilder AddDeserializerPostProcessor<T>() where T : class, IDeserializerPostProcessor
    {
        _services.AddSingleton<IDeserializerPostProcessor, T>();
        return this;
    }

    public TextSerializerOptionsBuilder AddSerializerPostProcessor<T>() where T : class, ISerializerPostProcessor
    {
        _services.AddSingleton<ISerializerPostProcessor, T>();
        return this;
    }

    public TextSerializerOptionsBuilder AddSerializerValidator<T, TObjectType>() where T : class, ISerializerValidator<TObjectType>
    {
        _services.AddSingleton<ISerializerValidator, T>();
        return this;
    }

    public TextSerializerOptionsBuilder AddDefaultValueConverter<T, TType>() where T : class, IValueConverter<TType>
    {
        _services.AddSingleton<IValueConverter, T>();
        return this;
    }

    public TextSerializerOptionsBuilder AddValueConverter<T>() where T : class, IValueConverter
    {
        _services.AddSingleton<IValueConverter, T>();
        return this;
    }

    public TextSerializerOptionsBuilder UseDefaultValueConverters()
    {
        _services.AddSingleton<IValueConverter, BoolValueConverter>();
        _services.AddSingleton<IValueConverter, NullableBoolValueConverter>();
        _services.AddSingleton<IValueConverter, DecimalValueConverter>();
        _services.AddSingleton<IValueConverter, NullableDecimalValueConverter>();
        _services.AddSingleton<IValueConverter, IntValueConverter>();
        _services.AddSingleton<IValueConverter, NullableIntValueConverter>();
        _services.AddSingleton<IValueConverter, StringValueConverter>();
        _services.AddSingleton<IValueConverter, TimeSpanValueConverter>();
        _services.AddSingleton<IValueConverter, TimeOnlyConverter>();
        _services.AddSingleton<IValueConverter, DateTimeValueConverter>();
        _services.AddSingleton<IValueConverter, NullableDateTimeValueConverter>();
        _services.AddSingleton<IValueConverter, DateOnlyValueConverter>();
        _services.AddSingleton<IValueConverter, NullableDateOnlyValueConverter>();

        if (_services.Any(x => x.ServiceType == typeof(IMessageIdProvider)) == false)
            _services.AddSingleton<IMessageIdProvider>(new MessageIdProvider(_ => throw new TextSerializerException("No configured or implemented IMessageIdProvider.")));

        return this;
    }

    public TextSerializerOptionsBuilder AddMessageIdProvider<T>() where T : class, IMessageIdProvider
    {
        _services.AddSingleton<IMessageIdProvider, T>();
        return this;
    }

    public TextSerializerOptionsBuilder AddMessageIdProvider(Func<string, string> func)
    {
        _services.AddSingleton<IMessageIdProvider>(new MessageIdProvider(func));
        return this;
    }

    public TextSerializerOptionsBuilder Configure(Action<TextSerializerOptions> configureOptions)
    {
        _services.Configure<TextSerializerOptions>(options => configureOptions.Invoke(options));
        return this;
    }

    public TextSerializerOptionsBuilder RegisterReceiveMessages(Assembly assembly)
    {
        _services.AddSingleton<IReceiveMessageFactory>(services =>
        {
            var objectFactory = services.GetRequiredService<IObjectFactory>();
            return ReceiveMessageFactory.Create(assembly, objectFactory);
        });
        return this;
    }

    public TextSerializerOptionsBuilder AddDeserializerPreProcessor<T>() where T : class, IDeserializerPreProcessor
    {
        _services.AddSingleton<IDeserializerPreProcessor, T>();
        return this;
    }

    public TextSerializerOptionsBuilder AddTypedDeserializer<T>() where T : class, ITypedDeserializer
    {
        _services.AddSingleton<ITypedDeserializer, T>();
        return this;
    }

    public TextSerializerOptionsBuilder UseSerializationValidation()
    {
        _services.AddSingleton<ISerializerValidator, MandatorySerializerValidator>();
        _services.AddSingleton<ISerializerValidator, LengthSerializerValidation>();
        return this;
    }

    public TextSerializerOptionsBuilder AddSerializerFormatter<T>() where T : class, ISerializerFormatter
    {
        _services.AddSingleton<ISerializerFormatter, T>();
        return this;
    }

    public TextSerializerOptionsBuilder AddCustomizations(Assembly assembly)
    {
        //TODO write this
        throw new NotImplementedException();
        return this;
    }
}
