using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SeaEco.Abstractions.Extensions;

public static class EnumExtension
{
    public static string GetDescription(this Enum @enum)
    {
        Type type = @enum.GetType();

        FieldInfo? field = type.GetFields().FirstOrDefault(f => f.Name == @enum.ToString());
        if (field is null)
        {
            return @enum.ToString();
        }

        DescriptionAttribute? attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute is null ? string.Empty : attribute.Description;
    }

    public static string GetDisplay(this Enum @enum)
    {
        Type type = @enum.GetType();

        FieldInfo? field = type.GetFields().FirstOrDefault(f => f.Name == @enum.ToString());
        if (field is null)
        {
            return @enum.ToString();
        }

        DisplayAttribute? attribute = field.GetCustomAttribute<DisplayAttribute>();
        return attribute is null || string.IsNullOrEmpty(attribute.Name) ? @enum.ToString() : attribute.Name;
    }
    
    public static string ToEnumDescription<TEnum>(this int value) where TEnum : Enum
    {
        if (!Enum.IsDefined(typeof(TEnum), value))
        {
            return value.ToString();
        }
        
        var enumValue = (TEnum)(object)value;
        var field = enumValue.GetType().GetField(enumValue.ToString());
        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? enumValue.ToString();
    }

}