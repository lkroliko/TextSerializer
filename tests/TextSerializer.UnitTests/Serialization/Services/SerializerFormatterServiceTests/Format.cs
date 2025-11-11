//using MrRabbit.TextSerializer.Serialization.Services;

//namespace MrRabbit.TextSerializer.UnitTests.Serialization.Services.SerializerFormatterServiceTests;
//[Trait("Category", "SerializerFormatterService")]
//public class Format
//{
//    private readonly ISerializerFormatterProvider _provider = Mock.Of<ISerializerFormatterProvider>();
//    private readonly ISerializerFormatter _formatter1 = Mock.Of<ISerializerFormatter>();
//    private readonly ISerializerFormatter _formatter2 = Mock.Of<ISerializerFormatter>();
//    private readonly SerializerFormatterService _formatter;
//    private readonly SerializationContext _context = A.SerializationContext
//        .WithProperty(builder => builder)
//        .WithProperty(builder => builder.WithContext());

//    public Format()
//    {
//        _formatter = new SerializerFormatterService(_provider);

//        Mock.Get(_provider).Setup(p => p.Get(_context.ObjectType)).Returns(new[] { _formatter1, _formatter2 });
//    }

//    [Fact]
//    public void WhenCalledThenFormattersFormatCalledForProperty1()
//    {
//        _formatter.Format(_context);

//        Mock.Get(_formatter1).Verify(f => f.Format(_context.Properties.First()), Times.Once);
//        Mock.Get(_formatter2).Verify(f => f.Format(_context.Properties.First()), Times.Once);
//    }

//    [Fact]
//    public void WhenCalledThenFormattersFormatNotCalledForProperty2()
//    {
//        _formatter.Format(_context);

//        Mock.Get(_formatter1).Verify(f => f.Format(_context.Properties.Last()), Times.Never);
//        Mock.Get(_formatter2).Verify(f => f.Format(_context.Properties.Last()), Times.Never);
//    }
//}


//TODO to fix tests
