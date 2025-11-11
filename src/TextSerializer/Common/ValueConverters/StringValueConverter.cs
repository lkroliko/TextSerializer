namespace MrRabbit.TextSerializer.Common.ValueConverters;
internal class StringValueConverter : IValueConverter<string>
{
    public object Convert(string value) => value;

    public string Convert(object value) => (string)value;
}
