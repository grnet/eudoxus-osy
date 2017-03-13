using EudoxusOsy.BusinessModel.Resources;
using System;

namespace EudoxusOsy.BusinessModel
{
    public static class EnumExtensions
    {
        public static bool IsHtmlEmail(this enEmailType type)
        {
            return false;
        }

        private static string GetResourceKey(this Enum enumeration, string suffix = null)
        {
            if (!string.IsNullOrEmpty(suffix))
                return enumeration.GetType().Name + "_" + enumeration.ToString() + "_" + suffix;
            else
                return enumeration.GetType().Name + "_" + enumeration.ToString();
        }

        public static string GetLabel(this Enum enumeration, string suffix = null)
        {
            var label = Labels.ResourceManager.GetString(enumeration.GetResourceKey(suffix));
            if (string.IsNullOrEmpty(label))
            {
                if (string.IsNullOrEmpty(suffix))
                    return enumeration.ToString();
                else
                    return enumeration.GetLabel();
            }
            else
                return label;
        }

        public static string GetEnglishLabel(this Enum enumeration)
        {
            return enumeration.GetLabel("en");
        }

        public static string GetAcronym(this Enum enumeration)
        {
            return enumeration.GetLabel("Acronym");
        }

        public static string GetIcon(this Enum enumeration)
        {
            return enumeration.GetLabel("Icon");
        }

        public static string GetLongLabel(this Enum enumeration)
        {
            return enumeration.GetLabel("Long");
        }

        public static int GetValue(this Enum enumeration)
        {
            return Convert.ToInt32(enumeration);
        }

        public static bool Has<T>(this Enum type, T value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }

        public static bool Is<T>(this Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }

        public static T Add<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Could not append value from enumerated type '{0}'.", typeof(T).Name), ex);
            }
        }

        public static T Remove<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Could not remove value from enumerated type '{0}'.", typeof(T).Name), ex);
            }
        }
    }
}
