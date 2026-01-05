namespace MrRabbit.TextSerializer.Serialization.Validators;

internal class LengthSerializerValidation : ISerializerValidator<TransmitMessage>
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
            if (property.PropertyInfo.IsOptional && property.SerializedValue == null)
                return;
            if ((property.PropertyInfo.FixedLength is not null || property.PropertyInfo.MinLength is not null || property.PropertyInfo.MaxLength is not null) && property.SerializedValue == null)
                throw new TextSerializerException($"Property with length attribute'{property.PropertyInfo.Name}' is null in '{context.ObjectType.Name}'.");
            if (property.PropertyInfo.MinLength is not null && property.PropertyInfo.MinLength.Value > property.SerializedValue!.Length)
                throw new TextSerializerException($"Property '{property.PropertyInfo.Name}' in '{context.ObjectType.Name}' have length of {property.SerializedValue.Length} but required minimal length is {property.PropertyInfo.MinLength.Value}.");
            if (property.PropertyInfo.MaxLength is not null && property.PropertyInfo.MaxLength.Value < property.SerializedValue!.Length)
                throw new TextSerializerException($"Property '{property.PropertyInfo.Name}' in '{context.ObjectType.Name}' have length of {property.SerializedValue.Length} but maximal length is {property.PropertyInfo.MaxLength.Value}.");
            if (property.PropertyInfo.FixedLength is not null && property.PropertyInfo.FixedLength.Value != property.SerializedValue!.Length)
                throw new TextSerializerException($"Property '{property.PropertyInfo.Name}' in '{context.ObjectType.Name}' have length of {property.SerializedValue.Length} but required fixed length is {property.PropertyInfo.FixedLength.Value}.");
        }
    }
}
