using System.Globalization;

namespace ObjectPrinting
{
    public class DigitCulturePrintingConfig<TOwner, TDigit>
    {
        private readonly IPrintingConfig printingConfig;

        public DigitCulturePrintingConfig(IPropertyPrintingConfig<TOwner> propertyPrintingConfig)
        {
            printingConfig = propertyPrintingConfig.PrintingConfig;
        }

        public PrintingConfig<TOwner> InvariantCulture()
        {
            printingConfig.AddCulture(typeof(TDigit), CultureInfo.InvariantCulture);
            return (PrintingConfig<TOwner>)printingConfig;
        }

        public PrintingConfig<TOwner> RussianCulture()
        {
            printingConfig.AddCulture(typeof(TDigit),new CultureInfo("ru-RU"));
            return (PrintingConfig<TOwner>)printingConfig;
        }

        public PrintingConfig<TOwner> FrenchCulture()
        {
            printingConfig.AddCulture(typeof(TDigit),new CultureInfo("fr-FR"));
            return (PrintingConfig<TOwner>)printingConfig;
        }


        public PrintingConfig<TOwner> CustomCulture(string culture)
        {
            printingConfig.AddCulture(typeof(TDigit), new CultureInfo(culture));
            return (PrintingConfig<TOwner>)printingConfig;
        }

        public PrintingConfig<TOwner> CustomCulture(CultureInfo culture)
        {
            printingConfig.AddCulture(typeof(TDigit), culture);
            return (PrintingConfig<TOwner>)printingConfig;
        }
    }
}