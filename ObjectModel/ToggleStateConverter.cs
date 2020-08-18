using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Telerik.WinControls.Enumerations;

namespace ObjectModel
{
    public class ToggleStateConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(ToggleState);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            string stringValue = (string)value;

            switch (stringValue)
            {
                case "Y":
                    return ToggleState.On;
                case "N":
                    return ToggleState.Off;
                case "M":
                    return ToggleState.Indeterminate;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(ToggleState);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            ToggleState state = (ToggleState)value;

            switch (state)
            {
                case ToggleState.On:
                    return "Y";
                case ToggleState.Off:
                    return "N";
                case ToggleState.Indeterminate:
                    return "M";
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
