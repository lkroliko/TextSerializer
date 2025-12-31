## TextSerializer

TextSerializer is library for serialization and deserialization without specific format.
Required format is achievable by customization. Look at this library as framework for text serializator.
It was designed for STX ETX protocols.

### Customization

#### Value converter

Value converter is used to convert message property value to string and string to message property value. 
To add default converters which use <i>.ToString()</i> method to convert to string and [type].Parse(string value) method use <i>UseDefaultValueConverters()</i> method when configuring.
For implementation your format the are useless but for testing and see how library works are fine. 
``` 
services.AddTextSerializer(builder => builder.UseDefaultValueConverters());
```

To add value converter implement interface IValueConverter<TType> and add class on configuration.
If messages contains nullable C# value types then is required to add two value converters e.g. IValueConverter<int> and IValueConverter<int?>. 
```
internal class IntValueConverter : IValueConverter<int>
{
    public object Convert(string value) => int.Parse(value);

    public string Convert(object value) => ((int)value).ToString();
}

internal class NullableIntValueConverter : IValueConverter<int?>
{
    public object Convert(string value) => string.IsNullOrEmpty(value) ? null! : int.Parse(value);

    public string Convert(object value) => value is null ? string.Empty : value.ToString()!;
}

services.AddTextSerializer(builder => builder.AddValueConverter<IntValueConverter>().AddValueConverter<NullableIntValueConverter>());
```

#### Attributes

##### Conditional

If property is marked as conditional it will be visible in IPropertyInfo.IsConditional. It is used in default validators.

##### Context Property

If property is marked as Context it will require implementation of ITypedDeserializer<TType> and ITypedSerializer<TType>.

##### FixedLengthAttribute

If property has fixed length it will be visible in IPropertyInfo.FixedLength. It is used in default validator.

##### MaxLengthAttribute

If property has max length it will be visible in IPropertyInfo.MaxLength. It is used in default validator.

##### MessageIdAttribute

Assigns a message id to a receive message class. Used to determine the receive message type in deserialization. 

##### MinLengthAttribute

If property has min length it will be visible in IPropertyInfo.MinLength. It is used in default validator.

##### OptionalAttribute

If property is marked as Optional it will be visible in IPropertyInfo.IsOptional. It is used in default validators.

##### PositionAttribute

Change property position in property list. Helpful in base class of receive messages to set e.g. message id once.

##### UseSerializerFormatterAttribute<T>

If property is marked with UseSerializerFormatterAttribute<T> where T is type of ISerializerFormatter then property will be formatted by this formatter. 

##### UseValueConverterAttribute<T>

If property is marked with UseValueConverterAttribute<T> where T is type of IValueConverter then property will be converted by this value converter. 

#### Serialization

##### SerializerFormatter

It allows to format serialized value. For example property type is int but serialized value must have always length 4 and empty space are filled with 0 (From value 12 serialized value must by 0012).

To add formatter implement class with interface ISerializerFormatter and add class on configuration.
```
public class SerializerFormatter : ISerializerFormatter
{
    void Format(SerializationProperty property)
    {
        
        ...
    }
}

services.AddTextSerializer(builder => builder.AddSerializerFormatter<SerializerFormatter>());
```

Mark property with <i>UseSerializerFormatter<T></i> attribute.
```
internal class TransmitMessage : TextSerializer.Common.TransmitMessage
{
    [UseSerializerFormatterAttribute<SerializerFormatter>]
    public int Property1 { get; set; }
}
```

It recommended to create own attribute and use it to mark property.
```
internal class SerializerFormatterAttribute : UseSerializerFormatterAttribute<SerializerFormatter> { }
```
##### Serializer Validator

Validators can check object property value and serialized value and throw exception if is not correct.
To enable default validators which validating propertys with <i>Mandatory</i>, <i>MinLength</i> ,<i>MaxLength</i>, <i>FixedLength</i> attributes use <i>UseDefaultSerializationValidators()</i> method when configuring. 
```
services.AddTextSerializer(builder => builder.UseDefaultSerializationValidators());
```

To add custom validator implement interface ISerializerValidator<TType> where TType is context property or is Message type and add class on configuration.
```
internal class SerializerValidator : ISerializerValidator<Message>
{
    void Validate(SerializationContext context)
    {
        ...
        throw new Exception();
    }
}

services.AddTextSerializer(builder => builder.AddSerializerValidator<SerializerValidator>());
```

##### Serializer Post Processors

Serializer post processors is last place to change serialized message, it receive value e.g. <i><Value1|Value2|Value3></i>. 
It is usable to calculate checksum and add to serialized message.


Option 1
```
services.AddTextSerializer(builder => builder.AddDeserializerPreProcessor(text => ...));
```

Option 2
```
internal class SerializerPostProcessor : ISerializerPostProcessor
{
    public string Process(string text)
    {
        ...
    }
}

services.AddTextSerializer(builder => builder.AddSerializerPostProcessor<SerializerPostProcessor>());
```

##### Typed Serializer

##### Text Builder

By default serializer use all properties in serialized value separated by separator option.
This behavior can by changed by implementing own <i>ITextBuilder<i>.

```
internal class TextBuilder :  ITextBuilder
{
    string Build(SerializationContext serializationObject)
    {
        ...
    }
}

services.UseTextBuilder<TextBuilder>();
```

#### Deserialization

##### Deserializer Pre Processor

Deserializer pre processors is place to change text message, it receive value e.g. <i><Value1|Value2|Value3></i>. 
It is usable to validate checksum and remove it from text.

Option 1
```
services.AddTextSerializer(builder => builder.AddDeserializerPreProcessor(text => ...));
```

Option 2
```
internal class DeserializerPreProcessor : IDeserializerPreProcessor
{
    public string Process(string text)
    {
        ...
    }
}

services.AddTextSerializer(builder => builder.AddDeserializerPreProcessor<DeserializerPreProcessor>());
```

##### Deserializer Post Processor

Deserializer post processors give access to deserialization context and deserialized object.

```
internal class DeserializerPostProcessor : IDeserializerPostProcessor
{
    public void Process(DeserializationContext context)
    {
        ...
    }
}

services.AddTextSerializer(builder => builder.AddDeserializerPostProcessor<DeserializerPostProcessor>());
```

##### Message Id Provider

Message id provider is required to determine type of destination object of deserialization. 
It must be configured or implemented to use Deserialize(string text) method on ITextSerializer.

Option 1
``` 
services.AddTextSerializer(builder => builder.AddMessageIdProvider(text => ...);
```

Option 2
```
internal class MessageIdProvider : IMessageIdProvider
{
    ...
}

services.AddTextSerializer(builder => builder.AddMessageIdProvider<MessageIdProvider>());
```

##### Typed Deserializer

##### Deserialization property factory

By default deserializer set properties in order with values separeted by separator option.
This behavior can by changed by implementing own <i>IDeserializationPropertyFactory<i>.

```
internal class DeserializationPropertyFactory : IDeserializationPropertyFactory
{
    DeserializationProperty Get(int index, IPropertyInfo propertyInfo, string[] propertyValues, object tarbetObject, IDeserializationContextFactory contextFactory)
    {
        ...
    }
}

services.UseDeserializationPropertyFactory<DeserializationPropertyFactory>();
```