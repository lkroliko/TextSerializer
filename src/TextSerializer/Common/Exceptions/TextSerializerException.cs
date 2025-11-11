namespace MrRabbit.TextSerializer.Common.Exceptions;

public class TextSerializerException : Exception
{
    internal TextSerializerException(string? message = null) : base(message) { }
    internal TextSerializerException(string? message = null, Exception? innerException = null) : base(message, innerException) { }
}
