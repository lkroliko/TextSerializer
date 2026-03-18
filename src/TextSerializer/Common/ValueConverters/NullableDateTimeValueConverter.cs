namespace MrRabbit.TextSerializer.Common.ValueConverters;

internal class NullableDateTimeValueConverter : IValueConverter<DateTime?>
{
    public object Convert(string value) => string.IsNullOrWhiteSpace(value) ? null! : DateTime.Parse(value);

    public string Convert(object value) => value is null ? string.Empty : value.ToString()!;
}
