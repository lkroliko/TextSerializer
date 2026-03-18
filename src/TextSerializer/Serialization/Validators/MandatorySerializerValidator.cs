namespace MrRabbit.TextSerializer.Serialization.Validators;
internal class MandatorySerializerValidator : ISerializerValidator<TransmitMessage>
{
    public void Validate(SerializationContext context)
    {
        ValidateContext(context);

        void ValidateContext(SerializationContext context) =>
            context.Properties.ForEach(property =>
            {
                if (property.Context is null)
                {
                    ValidateProperty(property);
                    return;
                }
                ValidateContext(property.Context);
            });

        void ValidateProperty(SerializationProperty property)
        {
            if (property.PropertyInfo.IsMandatory && property.Value is null)
                throw new TextSerializerException($"Mandatory property '{property.PropertyInfo.Name}' is null in '{context.ObjectType.Name}'.");
        }
    }
}
