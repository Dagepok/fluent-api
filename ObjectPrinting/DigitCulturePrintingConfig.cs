using System.Globalization;

namespace ObjectPrinting
{
    public class DigitCulturePrintingConfig<TOwner, TDigit>
    {
        private readonly PropertyPrintingConfig<TOwner, TDigit> printingConfig;

        public DigitCulturePrintingConfig(PropertyPrintingConfig<TOwner, TDigit> printingConfig)
        {
            this.printingConfig = printingConfig;
        }

        public PrintingConfig<TOwner> InvariantCulture()
        {
            ((IPrintingConfig)
                    ((IPropertyPrintingConfig<TOwner>) printingConfig).PrintingConfig)
                .AddCulture(typeof(TDigit), CultureInfo.InvariantCulture);
            return ((IPropertyPrintingConfig<TOwner>) printingConfig).PrintingConfig;
        }


        public PrintingConfig<TOwner> InstalledUiCulture()
        {
            ((IPrintingConfig)
                    ((IPropertyPrintingConfig<TOwner>) printingConfig).PrintingConfig)
                .AddCulture(typeof(TDigit), CultureInfo.InstalledUICulture);
            return ((IPropertyPrintingConfig<TOwner>) printingConfig).PrintingConfig;
        }

        public PrintingConfig<TOwner> CustomCulture(string culture)
        {
            ((IPrintingConfig)
                    ((IPropertyPrintingConfig<TOwner>) printingConfig).PrintingConfig)
                .AddCulture(typeof(TDigit), new CultureInfo(culture));
            return ((IPropertyPrintingConfig<TOwner>) printingConfig).PrintingConfig;
        }

        public PrintingConfig<TOwner> CustomCulture(CultureInfo culture)
        {
            ((IPrintingConfig)
                    ((IPropertyPrintingConfig<TOwner>) printingConfig).PrintingConfig)
                .AddCulture(typeof(TDigit), culture);
            return ((IPropertyPrintingConfig<TOwner>) printingConfig).PrintingConfig;
        }
    }
}