using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using ObjectPrinting.Tests;

namespace ObjectPrinting
{
    public class PrintingConfig<TOwner>
    {
        public string PrintToString(TOwner obj)
        {
            return PrintToString(obj, 0);
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
                sb.Append(identation + propertyInfo.Name + " = " +
                          PrintToString(propertyInfo.GetValue(obj),
                              nestingLevel + 1));
            return sb.ToString();
        }

        public PrintingConfig<TOwner> Exclude<T>()
        {
            return this;
        }

        public PropertyPrintingConfig<TOwner, T> Printing<T>()
        {
            return new PropertyPrintingConfig<TOwner, T>(this);
        }


        public PropertyPrintingConfig<TOwner, TPropType> Printing<TPropType>(Expression<Func<TOwner, TPropType>> selector)
        {
            return new PropertyPrintingConfig<TOwner, TPropType>(this);
        }

        public PrintingConfig<TOwner> Exclude<TPropType>(Expression<Func<TOwner, TPropType>> selector)
        {
            return this;
        }
        
    }
}