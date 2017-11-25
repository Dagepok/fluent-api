using System;
using System.Globalization;
using System.Reflection;

namespace ObjectPrinting
{
    internal interface IPrintingConfig
    {
        void AddCulture(Type type, CultureInfo culture);
        void AddCustomPropertySerialization(PropertyInfo propertyName, Delegate func);
        void AddCustomTypeSerialization(Type type, Delegate func);
        void SetStringPropertiesLenght(int length);
    }
}