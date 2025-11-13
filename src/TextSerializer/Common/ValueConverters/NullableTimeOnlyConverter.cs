namespace MrRabbit.TextSerializer.Common.ValueConverters;

internal class NullableTimeOnlyConverter : IValueConverter<TimeOnly?>
{
    public object Convert(string value) => TimeOnly.Parse(value);

    public string Convert(object value) => value.ToString()!;
}
