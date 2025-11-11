namespace MrRabbit.TextSerializer.Common.ValueConverters;
internal class BoolValueConverter : IValueConverter<bool>
{
    public object Convert(string value) => value != "0";

    public string Convert(object value) => (bool)value ? "1" : "0";
}
