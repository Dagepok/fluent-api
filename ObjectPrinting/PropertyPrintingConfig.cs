using System;
using System.Globalization;
using System.Reflection;

namespace ObjectPrinting
{
    public class PropertyPrintingConfig<TOwner, TPropType> : IPropertyPrintingConfig<TOwner>
    {
        private readonly PrintingConfig<TOwner> printingConfig;
        private readonly PropertyInfo propertyInfo;

        public PropertyPrintingConfig(PrintingConfig<TOwner> printingConfig, PropertyInfo propertyInfo = null)
        {
            this.printingConfig = printingConfig;
            this.propertyInfo = propertyInfo;
        }

        PrintingConfig<TOwner> IPropertyPrintingConfig<TOwner>.PrintingConfig => printingConfig;

        public PrintingConfig<TOwner> Using(Func<TPropType, string> func)
        {
            if (propertyInfo == null)
                ((IPrintingConfig)printingConfig).AddCustomTypeSerialization(typeof(TPropType), func);
            else
                ((IPrintingConfig)printingConfig).AddCustomPropertySerialization(propertyInfo, func);
            return printingConfig;
        }

        public PrintingConfig<TOwner> Using(CultureInfo culture)
        {
            return printingConfig;
        }
    }


    public interface IPropertyPrintingConfig<TOwner>
    {
        PrintingConfig<TOwner> PrintingConfig { get; }
    }
}