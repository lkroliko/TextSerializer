namespace MrRabbit.TextSerializer.Common.ValueConverters;

internal class TimeOnlyConverter : IValueConverter<TimeOnly>
{
    public object Convert(string value) => TimeOnly.Parse(value);

    public string Convert(object value) => value.ToString()!;
}
