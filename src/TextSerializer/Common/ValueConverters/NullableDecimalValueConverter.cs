namespace MrRabbit.TextSerializer.Common.ValueConverters;

internal class NullableDecimalValueConverter : IValueConverter<decimal?>
{
    public object Convert(string value) => string.IsNullOrEmpty(value) ? null! : decimal.Parse(value);

    public string Convert(object value) => value is null ? string.Empty : value.ToString()!;
}
