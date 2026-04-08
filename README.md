# TextSerializer

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![NuGet](https://img.shields.io/nuget/v/MrRabbit.TextSerializer.svg)](https://www.nuget.org/packages/MrRabbit.TextSerializer)

A .NET library for text-based serialization and deserialization with no fixed format. The desired format is fully achievable through customization — think of TextSerializer as a **framework for text serialization**. It was designed with STX/ETX protocols in mind, but can be adapted to any text-based protocol.

## Table of Contents

- [Features](#features)
- [Supported Frameworks](#supported-frameworks)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Configuration](#configuration)
  - [Options](#options)
  - [Value Converters](#value-converters)
  - [Message Id Provider](#message-id-provider)
  - [Receive Message Registration](#receive-message-registration)
  - [Serialization Validators](#serialization-validators)
  - [Serializer Formatters](#serializer-formatters)
  - [Pre-Processors & Post-Processors](#pre-processors--post-processors)
  - [Custom Text Builder](#custom-text-builder)
  - [Custom Deserialization Property Factory](#custom-deserialization-property-factory)
  - [Typed Serializers & Deserializers](#typed-serializers--deserializers)
- [Attributes](#attributes)
- [API Reference](#api-reference)
- [Full Example — STX/ETX Protocol](#full-example--stxetx-protocol)
- [Changelog](#changelog)
- [License](#license)

## Features

- Format-agnostic text serialization and deserialization
- Fluent configuration API via `IServiceCollection`
- Configurable prefix, suffix, and separator
- Extensible value converters, formatters, validators, and processors
- Built-in support for common .NET types
- Attribute-driven property metadata (length constraints, positioning, optionality)
- Message ID–based deserialization routing
- Custom text builders and deserialization property factories

## Supported Frameworks

| Framework | Version |
|-----------|---------|
| .NET      | 8.0     |
| .NET      | 9.0     |
| .NET      | 10.0    |

## Installation

```shell
dotnet add package MrRabbit.TextSerializer
```

Or via the NuGet Package Manager:

```powershell
Install-Package MrRabbit.TextSerializer
```

## Quick Start

### 1. Register the serializer

```csharp
services.AddTextSerializer(builder =>
{
    builder
        .Configure(options =>
        {
            options.Prefix = "<";
            options.Separator = "|";
            options.Suffix = ">";
        })
        .UseDefaultValueConverters();
});
```

### 2. Define messages

```csharp
// Message to serialize (transmit)
public class PingTransmitMessage : TransmitMessage
{
    public string Command => "PING";
    public int Sequence { get; set; }
}

// Message to deserialize (receive)
[MessageId("PONG")]
public class PongReceiveMessage : ReceiveMessage
{
    public string Command { get; set; } = default!;
    public int Sequence { get; set; }
}
```

### 3. Serialize & Deserialize

```csharp
private readonly ITextSerializer _textSerializer;

// Serialize
var message = new PingTransmitMessage { Sequence = 1 };
string text = _textSerializer.Serialize(message);
// Result: "<PING|1>"

// Deserialize
var received = _textSerializer.Deserialize<PongReceiveMessage>("<PONG|42>");
// received.Sequence == 42
```

## Configuration

All configuration is done through the `AddTextSerializer` extension method on `IServiceCollection`:

```csharp
services.AddTextSerializer(builder =>
{
    // configure here...
});
```

### Options

Set the global message format using `Configure`:

```csharp
builder.Configure(options =>
{
    options.Prefix = "<";      // message start character(s)
    options.Separator = "|";   // property value separator
    options.Suffix = ">";      // message end character(s)
});
```

| Property    | Type      | Description                              |
|-------------|-----------|------------------------------------------|
| `Prefix`    | `string?` | Characters prepended to the message      |
| `Separator` | `string?` | Delimiter between serialized values      |
| `Suffix`    | `string?` | Characters appended to the message       |

### Value Converters

Value converters handle conversion between property values and their string representations.

#### Default converters

Call `UseDefaultValueConverters()` to register built-in converters for the following types:

`bool`, `bool?`, `decimal`, `decimal?`, `int`, `int?`, `string`, `TimeSpan`, `TimeOnly`, `TimeOnly?`, `DateTime`, `DateTime?`, `DateOnly`, `DateOnly?`

```csharp
builder.UseDefaultValueConverters();
```

> Default converters use `.ToString()` for serialization and `[Type].Parse()` for deserialization.

#### Custom converters

Implement `IValueConverter<TType>` and register it:

```csharp
public class DoubleValueConverter : IValueConverter<double>
{
    public object Convert(string value) => double.Parse(value);
    public string Convert(object value) => ((double)value).ToString("F2");
}
```

```csharp
builder.AddValueConverter<DoubleValueConverter>();
```

> **Note:** For nullable value types, you must register separate converters — e.g., both `IValueConverter<int>` and `IValueConverter<int?>`.

A specific converter can also be applied per-property via the `[UseValueConverter<T>]` attribute.

### Message Id Provider

The message ID provider extracts an identifier from raw text to determine which `ReceiveMessage` type to instantiate during deserialization.

```csharp
// Option 1: Inline delegate
builder.AddMessageIdProvider(text => text.TrimStart('<').Split('|')[0]);

// Option 2: Custom class
public class MyMessageIdProvider : IMessageIdProvider
{
    public bool TryGetMessageId(string text, out string messageId)
    {
        // parse logic...
    }
}

builder.AddMessageIdProvider<MyMessageIdProvider>();
```

### Receive Message Registration

Register all `ReceiveMessage` types decorated with `[MessageId]` from a given assembly:

```csharp
builder.RegisterReceiveMessages(Assembly.GetExecutingAssembly());
```

### Serialization Validators

Validators inspect property values and serialized output, throwing exceptions on invalid data.

#### Default validators

Enable built-in validators that enforce `Mandatory`, `MinLength`, `MaxLength`, and `FixedLength` constraints:

```csharp
builder.UseDefaultSerializationValidators();
```

#### Custom validators

Implement `ISerializerValidator<TObjectType>` where `TObjectType` is the message or context property type:

```csharp
public class MyValidator : ISerializerValidator<MyTransmitMessage>
{
    public void Validate(SerializationContext context)
    {
        // validation logic...
    }
}

builder.AddSerializerValidator<MyValidator, MyTransmitMessage>();
```

### Serializer Formatters

Formatters transform a serialized property value into a specific string format (e.g., zero-padding).

```csharp
public class ZeroPadFormatter : ISerializerFormatter
{
    public void Format(SerializationProperty property)
    {
        property.SerializedValue = property.SerializedValue?.PadLeft(4, '0');
    }
}

builder.AddSerializerFormatter<ZeroPadFormatter>();
```

Apply to a property using `[UseSerializerFormatter<T>]`:

```csharp
public class MyMessage : TransmitMessage
{
    [UseSerializerFormatter<ZeroPadFormatter>]
    public int Code { get; set; }
}
```

> **Tip:** Create a custom attribute for cleaner usage:
> ```csharp
> public class ZeroPadAttribute : UseSerializerFormatterAttribute<ZeroPadFormatter> { }
> ```

### Pre-Processors & Post-Processors

Processors allow you to transform the raw text before deserialization or after serialization. Common uses include checksum handling.

#### Serializer Pre-Processor

Runs before serialization, receives the `SerializationContext`:

```csharp
public class MySerializerPreProcessor : ISerializerPreProcessor
{
    public void Process(SerializationContext context) { /* ... */ }
}

builder.AddSerializerPreProcessor<MySerializerPreProcessor>();
```

#### Serializer Post-Processor

Runs after serialization, receives and returns the final text:

```csharp
// Inline
builder.AddSerializerPostProcessor(text => text.Replace(">", "#CRC>"));

// Or class-based
public class ChecksumAppender : ISerializerPostProcessor
{
    public string Process(string text) { /* ... */ }
}

builder.AddSerializerPostProcessor<ChecksumAppender>();
```

#### Deserializer Pre-Processor

Runs before deserialization, receives and returns the raw text:

```csharp
// Inline
builder.AddDeserializerPreProcessor(text => text.Replace("#CRC", string.Empty));

// Or class-based
public class ChecksumStripper : IDeserializerPreProcessor
{
    public string Process(string text) { /* ... */ }
}

builder.AddDeserializerPreProcessor<ChecksumStripper>();
```

#### Deserializer Post-Processor

Runs after deserialization, receives the `DeserializationContext`:

```csharp
public class MyDeserializerPostProcessor : IDeserializerPostProcessor
{
    public void Process(DeserializationContext context) { /* ... */ }
}

builder.AddDeserializerPostProcessor<MyDeserializerPostProcessor>();
```

### Custom Text Builder

Override the default serialization output (properties joined by separator) with a custom `ITextBuilder`:

```csharp
public class MyTextBuilder : ITextBuilder
{
    public string Build(SerializationContext serializationObject)
    {
        // custom text assembly logic...
    }
}

builder.UseTextBuilder<MyTextBuilder>();
```

### Custom Deserialization Property Factory

Override default property value assignment during deserialization:

```csharp
public class MyPropertyFactory : IDeserializationPropertyFactory
{
    public DeserializationProperty Get(
        int index,
        IPropertyInfo propertyInfo,
        string[] propertyValues,
        object targetObject,
        IDeserializationContextFactory contextFactory)
    {
        // custom property creation logic...
    }
}

builder.UseDeserializationPropertyFactory<MyPropertyFactory>();
```

### Typed Serializers & Deserializers

For properties marked with `[ContextProperty]`, implement `ITypedSerializer<TType>` and/or `ITypedDeserializer<TType>` to handle nested or complex object serialization:

```csharp
public class AddressSerializer : ITypedSerializer<Address>
{
    public void Serialize(SerializationContext context) { /* ... */ }
}

public class AddressDeserializer : ITypedDeserializer<Address>
{
    public void Deserialize(DeserializationContext context) { /* ... */ }
}

builder.AddTypedDeserializer<AddressDeserializer>();
```

## Attributes

| Attribute | Target | Description |
|-----------|--------|-------------|
| `[MessageId("id")]` | Class | Assigns a message ID to a `ReceiveMessage` class for deserialization routing |
| `[Position(Position.First\|Penultimate\|Last)]` | Property | Controls property position in the serialized output |
| `[Optional]` | Property | Marks property as optional (used by default validators) |
| `[Conditional]` | Property | Marks property as conditional (used by default validators) |
| `[FixedLength(n)]` | Property | Enforces exact serialized value length (used by default validators) |
| `[MinLength(n)]` | Property | Enforces minimum serialized value length (used by default validators) |
| `[MaxLength(n)]` | Property | Enforces maximum serialized value length (used by default validators) |
| `[ContextProperty]` | Property | Marks property as a complex object requiring `ITypedSerializer<T>` / `ITypedDeserializer<T>` |
| `[UseSerializerFormatter<T>]` | Property | Applies a specific `ISerializerFormatter` to the property (supports multiple) |
| `[UseValueConverter<T>]` | Property | Applies a specific `IValueConverter` to the property |

## API Reference

### `ITextSerializer`

```csharp
public interface ITextSerializer
{
    string Serialize(TransmitMessage value);
    ReceiveMessage Deserialize(string text);
    TReceiveMessage Deserialize<TReceiveMessage>(string text) where TReceiveMessage : ReceiveMessage;
    ReceiveMessage Deserialize(Type type, string text);
}
```

| Method | Description |
|--------|-------------|
| `Serialize` | Serializes a `TransmitMessage` into a text string |
| `Deserialize(string)` | Deserializes text into a `ReceiveMessage` using the registered `IMessageIdProvider` |
| `Deserialize<T>(string)` | Deserializes text into a specific `ReceiveMessage` type |
| `Deserialize(Type, string)` | Deserializes text into a `ReceiveMessage` of the specified type |

### Base Classes

| Class | Description |
|-------|-------------|
| `Message` | Abstract base class for all messages |
| `TransmitMessage` | Base class for messages to serialize |
| `ReceiveMessage` | Base class for messages to deserialize |

## Full Example — STX/ETX Protocol

The following example demonstrates a complete STX/ETX protocol integration.

**Protocol rules:**
1. Messages start with `<` and end with `>`
2. Values are separated by `|`
3. First value is the message ID
4. Message includes a CRC checksum prefixed with `#` before the closing `>`

### Configuration

```csharp
services.AddTextSerializer(builder =>
{
    builder
        .Configure(options =>
        {
            options.Prefix = "<";
            options.Separator = "|";
            options.Suffix = ">";
        })
        .AddMessageIdProvider(text => text.TrimStart('<').Split('|')[0])
        .AddDeserializerPreProcessor(text => text.Replace("#CRC", string.Empty))
        .AddSerializerPostProcessor(text => text.Replace(">", "#CRC>"))
        .UseDefaultValueConverters()
        .RegisterReceiveMessages(Assembly.GetExecutingAssembly());
});
```

### Message Base Classes

```csharp
public abstract class ProtocolReceiveMessage : ReceiveMessage
{
    [Position(Position.First)]
    public string MessageId { get; set; } = default!;
}

public abstract class ProtocolTransmitMessage : TransmitMessage
{
    public abstract string MessageId { get; }
}
```

### Receive Message

For incoming text `<Hello|Example|1#CRC>`:

```csharp
[MessageId("Hello")]
public class HelloReceiveMessage : ProtocolReceiveMessage
{
    public string Name { get; set; } = default!;
    public int Version { get; set; }
}
```

```csharp
var message = _textSerializer.Deserialize("<Hello|Example|1#CRC>");
// message is HelloReceiveMessage { MessageId = "Hello", Name = "Example", Version = 1 }
```

### Transmit Message

To produce `<Hello|Example|1#CRC>`:

```csharp
public class HelloTransmitMessage : ProtocolTransmitMessage
{
    public override string MessageId => "Hello";
    public string Name { get; set; } = default!;
    public int Version { get; set; }
}
```

```csharp
var text = _textSerializer.Serialize(new HelloTransmitMessage
{
    Name = "Example",
    Version = 1
});
// text == "<Hello|Example|1#CRC>"
```

## Changelog

See [CHANGELOG](doc/CHANGELOG.md) for version history and release notes.

## License

This project is licensed under the [MIT License](https://opensource.org/licenses/MIT).