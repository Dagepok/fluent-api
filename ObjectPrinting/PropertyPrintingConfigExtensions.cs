using System;

namespace ObjectPrinting
{
    public static class PropertyPrintingConfigExtensions

    {
        public static DigitCulturePrintingConfig<TOwner, int> SetDigitsCulter<TOwner>
            (this PropertyPrintingConfig<TOwner, int> printingConfig)
            => new DigitCulturePrintingConfig<TOwner, int>(printingConfig);

        public static DigitCulturePrintingConfig<TOwner, double> SetDigitsCulter<TOwner>
            (this PropertyPrintingConfig<TOwner, double> printingConfig)
            => new DigitCulturePrintingConfig<TOwner, double>(printingConfig);

        public static DigitCulturePrintingConfig<TOwner, long> SetDigitsCulter<TOwner>
            (this PropertyPrintingConfig<TOwner, long> printingConfig)
            => new DigitCulturePrintingConfig<TOwner, long>(printingConfig);

        public static PrintingConfig<TOwner> Trim<TOwner>
            (this PropertyPrintingConfig<TOwner, string> printingConfig, int lenght)
        {
            if (lenght <= 0)
                throw new ArgumentException("Length should be positive");
            var config = ((IPropertyPrintingConfig<TOwner>)printingConfig).PrintingConfig;
            ((IPrintingConfig)config).SetStringPropertiesLenght(lenght);
            return ((IPropertyPrintingConfig<TOwner>)printingConfig).PrintingConfig;
        } 


    }

}