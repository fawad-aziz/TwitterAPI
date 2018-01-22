using System;
using System.Globalization;

namespace TwitterAPI.Helper
{
    public static class Guard
    {
        public static IFormatProvider CurrentStringFormatProvider
        {
            get
            {
                return CultureInfo.CurrentCulture;
            }
        }

        public static void ArgumentNotNull(object value, string valueName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(FormatResourceString("Argument {0} cannot be null", valueName));
            }
        }

        public static void ArgumentNotNullOrEmptyString(string value, string valueName, bool ignoreWhiteSpace = false)
        {
            if ((ignoreWhiteSpace && String.IsNullOrWhiteSpace(value)) || (!ignoreWhiteSpace && String.IsNullOrEmpty(value)))
            {
                throw new ArgumentException(FormatResourceString("Argument {0} cannot be null or empty", valueName));
            }
        }

        public static string FormatResourceString(string resourceValue, params object[] args)
        {
            return String.Format(CurrentStringFormatProvider, resourceValue, args);
        }
    }
}