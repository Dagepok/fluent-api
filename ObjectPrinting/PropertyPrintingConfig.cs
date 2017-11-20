using System;
using System.Globalization;

namespace ObjectPrinting
{
    public class PropertyPrintingConfig<TOwner, TPropType> : IPropertyPrintingConfig<TOwner, TPropType>
    {
        private readonly PrintingConfig<TOwner> printingConfig;

        public PropertyPrintingConfig(PrintingConfig<TOwner> printingConfig)
            => this.printingConfig = printingConfig;

        public PrintingConfig<TOwner> Using(Func<TPropType, string> func)
            => printingConfig;

        public PrintingConfig<TOwner> Using(CultureInfo culture)
            => printingConfig;
        PrintingConfig<TOwner> IPropertyPrintingConfig<TOwner, TPropType>.PrintingConfig => printingConfig;
    }


    public interface IPropertyPrintingConfig<TOwner, TPropType>
    {
        PrintingConfig<TOwner> PrintingConfig { get; }
    }
}