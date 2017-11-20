using NUnit.Framework;
using ObjectPrinting.Solved;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class ObjectPrinterAcceptanceTests
    {
        [Test]
        public void Demo()
        {
            var person = new Person {Name = "Alex", Age = 19};

            var printer = ObjectPrinter.For<Person>()
                //1. Исключить из сериализации свойства определенного типа
                .Exclude<string>()

                //2. Указать альтернативный способ сериализации для определенного типа
                .Printing<int>().Using(p => p.ToString())

                //3. Для числовых типов указать культуру
                .Printing<int>().SetDigitsCulter("en-EN")

                //4. Настроить сериализацию конкретного свойства
                .Printing(p => p.Age).Using(age => age.ToString())

                //5. Настроить обрезание строковых свойств (метод должен быть виден только для строковых свойств)
                .Printing<string>().SubString(4, 0)

                //6. Исключить из сериализации конкретного свойства
                .Exclude(p => p.Age);
            var s1 = printer.PrintToString(person);

            //7. Синтаксический сахар в виде метода расширения, сериализующего по-умолчанию
            var s2 = person.PrintToString();
            //8. ...с конфигурированием
            var s3 = person.PrintToString(x=>x.Printing(p=>p.Age)
                                                .Using(age=>age.ToString()));
        }
    }
}