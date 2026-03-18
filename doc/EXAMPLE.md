## TextSerializer STX ETX example

### Service collection requirements

Service colletion requirements:
- enabled options 
```
ServiceCollection.AddOptions()
```
- enabled logger
``` ServiceCollection.AddLogging();
```

### Protocol

Example protocol requirements:
1) each message starts with '<' and ends with '>' (STX and ETX characters have been replaced to make the example easier) 
2) each value in message is separated by character '|'
3) special value representing type of message is always as first value in message (Represented by MessageId)
4) message have checksum represented as "CRC""
5) checksum is after last value and before message end character '>'
6) checksum have character '#' prefix 
7) values are represented as text with the same format as .net .ToString method

Valid message examples:
- <MessageId|Value1#CRC>
- <MessageId|Value1|Value2#CRC>

### Configuration

Lets add text serializer to IoC and next we will configure options.

```
ServiceCollection.AddTextSerializer(builder => ... );
```

#### Protocol message format

Configure 1st, 2nd protocol requirement.

```
    options.Configure(options =>
        {
            options.Prefix = "<"
            options.Separator = "|"
            options.Suffix = ">"
        })
```

#### Message id

Configure 3rd protocol requirement.

```
    options.AddMessageIdProvider(text => text.TrimStart('<').Split('|')[0]);
```

#### Checksum

Configure 4th, 5th, 6th protocol requirement.

```
    options.AddDeserializerPreProcessor(text => text.Replace("#CRC", string.Empty))
        .AddSerializerPostProcessor(text => text.Replace(">", "#CRC>"));
```

#### Value converters

Configure 7th protocol requirement.

```
    options.UseDefaultValueConverters();
```

#### Message types

Adds ReceiveMessages with are implemented in next step.

```
    options.RegisterReceiveMessages(Assembly.GetExecutingAssembly());
```

### Messages

Create base classes with message id.

```
public abstract class ProtocolReceiveMessage : TextSerializer.Common.ReceiveMessage
{
    [Position(Position.First)]
    public string MessageId { get; set; } = default!;
}
```

```
public abstract class ProtocolTransmitMessage : TextSerializer.Common.TransmitMessage
{
    public abstract string MessageId { get; }
}
```

#### Receive message

For receive message "<Hello|Example|1#CRC>".

Implement message class.

```
[MessageId("Hello")]
public class HelloProtocolReceiveMessage : ProtocolReceiveMessage
{
    public string Name { get; set; } = default!;
    public int Version { get; set; }
}
```

Pass text to deserialize method and it return message of type HelloProtocolReceiveMessage.

```
private readonly ITextSerializer _textSerializer;

...

var text = "<Hello|Example|1#CRC>";
var message = _textSerializer.Deserialize(text);
```

#### Transmit message

For send message "<Hello|Example|1#CRC>".

Implement message class.

```
public class HelloTransmitMessage : ProtocolTransmitMessage
{
    public override string MessageId => "Hello";
    public string Name { get; set; } = default!;
    public int Version { get; set; }
}
```

Pass message object to serialize method and it return text.

```
private readonly ITextSerializer _textSerializer;

...

var message = new HelloTransmitMessage()
{
    Name = "Example",
    Version = 1,
};

var text = _textSerializer.Serialize(message);
```