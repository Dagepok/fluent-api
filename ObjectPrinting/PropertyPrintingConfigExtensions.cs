using System;

namespace ObjectPrinting
{
    public static class PropertyPrintingConfigExtensions

    {
        public static PrintingConfig<TOwner> SetDigitsCulter<TOwner>
            (this PropertyPrintingConfig<TOwner, int> printingConfig, string culture)
            => ((IPropertyPrintingConfig<TOwner, int>)printingConfig).PrintingConfig;

        public static PrintingConfig<TOwner> SetDigitsCulter<TOwner>
            (this PropertyPrintingConfig<TOwner, long> printingConfig, string culture)
            => ((IPropertyPrintingConfig<TOwner, long>)printingConfig).PrintingConfig;

        public static PrintingConfig<TOwner> SetDigitsCulter<TOwner>
            (this PropertyPrintingConfig<TOwner, double> printingConfig, string culture)
            => ((IPropertyPrintingConfig<TOwner, double>)printingConfig).PrintingConfig;

        public static PrintingConfig<TOwner> SubString<TOwner>
            (this PropertyPrintingConfig<TOwner, string> printingConfig, int startIndex, int lenght)
            => ((IPropertyPrintingConfig<TOwner, string>)printingConfig).PrintingConfig;


    }

}