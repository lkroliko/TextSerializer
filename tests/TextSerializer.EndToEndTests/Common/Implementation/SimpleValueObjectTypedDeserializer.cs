using MrRabbit.TextSerializer.Common;

namespace MrRabbit.TextSerializer.EndToEndTests.Common.Implementation;
internal class SimpleValueObjectTypedDeserializer : ITypedDeserializer<SimpleValueObject>
{
    public void Deserialize(DeserializationContext context)
    {
        if (string.IsNullOrEmpty(context.Value))
        {
            context.TargetProperty!.DeserializedValue = null;
            return;
        }

        context.Properties[0].DeserializedValue = context.Value.Substring(0, 6);
        context.Properties[1].DeserializedValue = context.Value.Substring(6, 6);
    }
}
