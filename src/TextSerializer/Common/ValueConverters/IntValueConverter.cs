namespace MrRabbit.TextSerializer.Common.ValueConverters;
internal class IntValueConverter : IValueConverter<int>
{
    public object Convert(string value) => int.Parse(value);

    public string Convert(object value) => ((int)value).ToString();
}
