namespace MrRabbit.TextSerializer.Common.ValueConverters;

internal class DateOnlyValueConverter : IValueConverter<DateOnly>
{
    public object Convert(string value) => DateOnly.Parse(value);

    public string Convert(object value) => value.ToString()!;
}
