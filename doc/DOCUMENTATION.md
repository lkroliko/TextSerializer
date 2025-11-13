## TextSerializer

TextSerializer is library for serialization and deserialization without specific format.
Required format is achievable by customization. Look at this library as framework for text serializator.
It was designed for STX ETX protocols.

### Customization

#### Value converter

Value converter is used to convert message property value to string and string to message property value. 
You can add default converters which use .ToString() method to convert to string and [type].Parse(string value) method.
For implementation your format the are useless but for testing and see how library works are fine. 
``` 
services.AddTextSerializer(options => options.UseDefaultValueConverters());
```

To add value converter implement interface IValueConverter<TType> and add class on configuration.
If you use nullable C# value types you need to add two value converters e.g. IValueConverter<int> and IValueConverter<int?>. 
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

services.AddTextSerializer(options => options.AddValueConverter<IntValueConverter>().AddValueConverter<NullableIntValueConverter>());
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

##### UseSerializerFormatterAttribute

##### UseValueConverterAttribute

Value of property will be converted by selected converter.

#### Serialization

##### SerializerFormatter

##### Serializer Validator

##### Serializer Post Processors

##### Typed Serializer

#### Deserialization

##### Deserializer Post Processor

##### Deserializer Pre Processor

##### Message Id Provider

Message id provider is required to determine type of destination object of deserialization. 
It must by configured or implemented when you use Deserialize(string text) method on ITextSerializer.

Option 1
``` 
services.AddTextSerializer(options => options.AddMessageIdProvider(text => ...);
```

Option 2
```
internal class MessageIdProvider : IMessageIdProvider
{
    ...
}

services.AddTextSerializer(options => options.AddMessageIdProvider<MessageIdProvider>());
```

##### Typed Deserializer