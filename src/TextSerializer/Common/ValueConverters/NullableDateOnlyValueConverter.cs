namespace MrRabbit.TextSerializer.Common.ValueConverters;

internal class NullableDateOnlyValueConverter : IValueConverter<DateOnly?>
{
    public object Convert(string value) => string.IsNullOrWhiteSpace(value) ? null! : DateOnly.Parse(value);

    public string Convert(object value) => value is null ? string.Empty : value.ToString()!;
}
