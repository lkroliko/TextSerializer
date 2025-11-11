namespace TextSerializer.SharedTests;

public class TestConsts
{
    public static readonly string StxPrefix = $"{'\x02'}";

    public static readonly string EtxSuffix = $"{'\x03'}";

    public static readonly string Prefix = "<";
    public static readonly string Suffix = ">";

    public const string Separator = "|";
}
