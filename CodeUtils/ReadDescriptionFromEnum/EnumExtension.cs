using System.ComponentModel;
using System.Reflection;

namespace CodeUtils.ReadDescriptionFromEnum
{
    public static class EnumExtension
    {
        public static List<string> GetEnumDescriptions<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                       .Cast<T>()
                       .Select(e => e.GetEnumDescription())
                       .ToList();
        }

        public static string GetEnumDescription<T>(this T value) where T : Enum
        {
            // value ở đây Status.New, Status.InProgress,...
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            return attributes != null && attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}