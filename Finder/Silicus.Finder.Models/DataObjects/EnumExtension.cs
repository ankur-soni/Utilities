using System;
using System.ComponentModel;
using System.Reflection;

namespace Silicus.Finder.Models.DataObjects
{
    public static class EnumExtension
    {

        public static string GetDescription(this Enum value)
        {
            if (value != null)
            {
                FieldInfo fieldinfo = value.GetType().GetField(value.ToString());

                DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
                else
                {
                    return value.ToString();
                }
            }

            return "Not Assigned";
        }
    }
}
