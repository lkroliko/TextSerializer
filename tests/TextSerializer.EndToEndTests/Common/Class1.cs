namespace MrRabbit.TextSerializer.EndToEndTests.Common;
internal abstract class Class1
{
    public abstract string Name { get; }
}

internal class test : Class1
{
    public override string Name => throw new NotImplementedException();
}
