namespace MrRabbit.TextSerializer.Common.ValueConverters;
internal class DecimalValueConverter : IValueConverter<decimal>
{
    public object Convert(string value) => decimal.Parse(value);

    public string Convert(object value) => value.ToString()!;
}
