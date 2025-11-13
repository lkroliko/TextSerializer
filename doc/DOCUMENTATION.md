## TextSerializer

TextSerializer is library for serialization and deserialization without specific format.
Required format is achievable by customization. Look at this library as framework for text serializator.
It was designed for STX ETX protocols.

### Customization

#### Value converter

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