namespace MrRabbit.TextSerializer.Common;
public abstract class ValueConverterBase<TType> : IValueConverter<TType>
{
    public object Convert(string value) => ConvertTo(value)!;

    public string Convert(object value) => ConvertFrom((TType)value);

    protected abstract TType ConvertTo(string value);
    protected abstract string ConvertFrom(TType value);
}
