namespace MrRabbit.TextSerializer.Common.Interfaces;

public interface IValueConverter
{
    object Convert(string value);
    string Convert(object value);
}

public interface IValueConverter<TType> : IValueConverter
{

}
