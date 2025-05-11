using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace SeaEco.Client.Components
{
    public class CustomInputNumber<TValue> : InputNumber<TValue>
    {
        protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
        {
            if (BindConverter.TryConvertTo<TValue>(value, CultureInfo.CurrentCulture, out var parsedValue))
            {
                result = parsedValue;
                validationErrorMessage = null!;
                return true;
            }
            
            result = default!;

            var fieldName = FieldIdentifier.FieldName;
            validationErrorMessage = $"{fieldName} påkrevd";
            return false;
        }
    }
}