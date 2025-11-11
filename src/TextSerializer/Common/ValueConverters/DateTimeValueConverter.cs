namespace MrRabbit.TextSerializer.Common.ValueConverters;

internal class DateTimeValueConverter : IValueConverter<DateTime>
{
    public object Convert(string value) => DateTime.Parse(value);

    public string Convert(object value) => value.ToString()!;
}
