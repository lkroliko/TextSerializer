namespace MrRabbit.TextSerializer.Common.ValueConverters;

internal class NullableBoolValueConverter : IValueConverter<bool?>
{
    public object Convert(string value) => string.IsNullOrEmpty(value) ? null! : value != "0";

    public string Convert(object value) => value is null ? string.Empty : (bool)value ? "1" : "0";
}
