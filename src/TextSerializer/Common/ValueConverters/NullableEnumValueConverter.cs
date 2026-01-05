namespace MrRabbit.TextSerializer.Common.ValueConverters;

internal class NullableEnumValueConverter : IValueConverter
{
    public object Convert(string value) => string.IsNullOrEmpty(value) ? null! : int.Parse(value);

    public string Convert(object value) => value is null ? string.Empty : ((int)value).ToString();
}
