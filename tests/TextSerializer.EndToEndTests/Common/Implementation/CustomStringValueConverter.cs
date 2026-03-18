namespace MrRabbit.TextSerializer.EndToEndTests.Common.Implementation;

internal class CustomStringValueConverter : IValueConverter
{
    public object Convert(string value) => $"{value}ConvertedByCustomStringValueConverter";

    public string Convert(object value) => $"{value}ConvertedByCustomStringValueConverter";
}
