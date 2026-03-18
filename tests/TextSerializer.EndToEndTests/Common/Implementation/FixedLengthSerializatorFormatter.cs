using MrRabbit.TextSerializer.Common;

namespace MrRabbit.TextSerializer.EndToEndTests.Common.Implementation;
internal class FixedLengthSerializatorFormatter : ISerializerFormatter
{
    public void Format(SerializationProperty property)
    {
        if (property.PropertyInfo.PropertyType == typeof(int))
        {
            while (property.SerializedValue!.Length != property.PropertyInfo.FixedLength)
                property.SerializedValue = $"0{property.SerializedValue}";
        }

        if (property.PropertyInfo.PropertyType == typeof(string))
        {
            while (property.SerializedValue!.Length != property.PropertyInfo.FixedLength)
                property.SerializedValue = $"_{property.SerializedValue}";
        }
    }
}
