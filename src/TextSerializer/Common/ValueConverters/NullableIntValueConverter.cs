namespace MrRabbit.TextSerializer.Common.ValueConverters;

internal class NullableIntValueConverter : IValueConverter<int?>
{
    public object Convert(string value) => string.IsNullOrEmpty(value) ? null! : int.Parse(value);

    public string Convert(object value) => value is null ? string.Empty : value.ToString()!;
}
