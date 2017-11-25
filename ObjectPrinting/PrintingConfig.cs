using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using FluentAssertions.Common;

namespace ObjectPrinting
{
    public class PrintingConfig<TOwner> : IPrintingConfig
    {
        private readonly Dictionary<PropertyInfo, Delegate> customPropertySerialization
            = new Dictionary<PropertyInfo, Delegate>();

        private readonly Dictionary<Type, Delegate> customTypeSerialization
            = new Dictionary<Type, Delegate>();

        private readonly Dictionary<Type, CultureInfo> customDigitCultureInfo
            = new Dictionary<Type, CultureInfo>();

        private readonly HashSet<PropertyInfo> excludedProperties = new HashSet<PropertyInfo>();
        private readonly HashSet<Type> excludedTypes = new HashSet<Type>();
        private int stringPropertiesLenght = -1;

        public string PrintToString(TOwner obj)
            => PrintToString(obj, 0);


        public PropertyPrintingConfig<TOwner, T> Printing<T>()
            => new PropertyPrintingConfig<TOwner, T>(this);

        public PropertyPrintingConfig<TOwner, TPropType> Printing<TPropType>(
            Expression<Func<TOwner, TPropType>> selector)
            => new PropertyPrintingConfig<TOwner, TPropType>(this, selector.GetPropertyInfo());

        public PrintingConfig<TOwner> Exclude<TPropType>(Expression<Func<TOwner, TPropType>> selector)
        {
            excludedProperties.Add(selector.GetPropertyInfo());
            return this;
        }

        public PrintingConfig<TOwner> Exclude<T>()
        {
            excludedTypes.Add(typeof(T));
            return this;
        }


        private string PrintToString(object obj, int nestingLevel)
        {
            //TODO apply configurations
            if (obj == null)
                return "null" + Environment.NewLine;

            var finalTypes = new[]
            {
                typeof(int), typeof(double), typeof(float), typeof(string),
                typeof(DateTime), typeof(TimeSpan)
            };

            if (finalTypes.Contains(obj.GetType()))
                return obj + Environment.NewLine;

            var identation = new string('\t', nestingLevel + 1);
            var sb = new StringBuilder();
            var type = obj.GetType();
            sb.AppendLine(type.Name);
            foreach (var propertyInfo in type.GetProperties())
            {
                if (IsExcluded(propertyInfo)) continue;
                sb.Append(identation + propertyInfo.Name + " = " +
                          PrintProperty(obj, propertyInfo, nestingLevel));
            }
            return sb.ToString();
        }

        private string PrintProperty(object obj, PropertyInfo propertyInfo, int nestingLevel)
        {
            if (customPropertySerialization.ContainsKey(propertyInfo))
                return customPropertySerialization[propertyInfo]
                           .DynamicInvoke(propertyInfo.GetValue(obj))
                           + Environment.NewLine;
            if (customTypeSerialization.ContainsKey(propertyInfo.PropertyType))
                return customTypeSerialization[propertyInfo.PropertyType]
                           .DynamicInvoke(propertyInfo.GetValue(obj))
                           + Environment.NewLine;
            if (customDigitCultureInfo.ContainsKey(propertyInfo.PropertyType))
                return ((IFormattable)propertyInfo.GetValue(obj))
                    .ToString("", customDigitCultureInfo[propertyInfo.PropertyType])
                    + Environment.NewLine;
            if (propertyInfo.PropertyType == typeof(string) && stringPropertiesLenght != -1)
                return ((string)propertyInfo.GetValue(obj)).Substring(0, stringPropertiesLenght)
                    + Environment.NewLine;

            return PrintToString(propertyInfo.GetValue(obj),
                nestingLevel + 1);
        }

        private bool IsExcluded(PropertyInfo propertyInfo)
            => excludedProperties.Contains(propertyInfo)
               || excludedTypes.Contains(propertyInfo.PropertyType);

        private void AddCustomTypeSerialization(Type type, Delegate func)
        {
            if (customTypeSerialization.ContainsKey(type))
                customTypeSerialization[type] = func;
            else
                customTypeSerialization.Add(type, func);
        }

        private void AddCustomPropertySerialization(PropertyInfo propertyName, Delegate func)
        {
            if (customPropertySerialization.ContainsKey(propertyName))
                customPropertySerialization[propertyName] = func;
            else
                customPropertySerialization.Add(propertyName, func);
        }

        private void AddCulture(Type type, CultureInfo culture)
        {
            if (customDigitCultureInfo.ContainsKey(type))
                customDigitCultureInfo[type] = culture;
            else
                customDigitCultureInfo.Add(type, culture);
        }

        void IPrintingConfig.AddCulture(Type type, CultureInfo culture)
            => AddCulture(type, culture);

        void IPrintingConfig.AddCustomPropertySerialization(PropertyInfo propertyName, Delegate func)
            => AddCustomPropertySerialization(propertyName, func);

        void IPrintingConfig.AddCustomTypeSerialization(Type type, Delegate func)
            => AddCustomTypeSerialization(type, func);

        void IPrintingConfig.SetStringPropertiesLenght(int length)
            => SetStringPropertiesLenght(length);

        private void SetStringPropertiesLenght(int length) => stringPropertiesLenght = length;
    }
}