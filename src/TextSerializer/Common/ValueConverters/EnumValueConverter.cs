namespace MrRabbit.TextSerializer.Common.ValueConverters;

internal class EnumValueConverter : IValueConverter
{
    public object Convert(string value) => int.Parse(value);

    public string Convert(object value) => ((int)value).ToString();
}
