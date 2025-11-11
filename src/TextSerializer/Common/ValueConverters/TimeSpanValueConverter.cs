namespace MrRabbit.TextSerializer.Common.ValueConverters;

internal class TimeSpanValueConverter : IValueConverter<TimeSpan>
{
    public object Convert(string value) => TimeSpan.Parse(value);

    public string Convert(object value) => value.ToString()!;
}
