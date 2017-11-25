using System;

namespace ObjectPrinting.Solved
{
    public static class ObjectExtensions
    {
        public static string PrintToString<T>(this T obj)
            => ObjectPrinter.For<T>().PrintToString(obj);

        public static string PrintToString<T>(this T obj,
            Func<PrintingConfig<T>, PrintingConfig<T>> setConfig)
            => setConfig(ObjectPrinter.For<T>()).PrintToString(obj);
    }
}