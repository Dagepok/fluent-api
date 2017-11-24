using System;
using FluentAssertions;
using NUnit.Framework;
using ObjectPrinting.Solved;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class ObjectPrinterAcceptanceTests
    {
        [SetUp]
        public void SetUp()
        {
            var father = new Person { Name = "Alex", SecondName = "Ivanov", Age = 43, Height = 164 };
            person = new Person { Name = "Alex", SecondName = "Ivanov", Age = 19, Height = 170.4 };
        }

        private Person person;


        [Test]
        public void Demo()
        {
            var printer = ObjectPrinter.For<Person>()
                //1. Исключить из сериализации свойства определенного типа
                .Exclude<string>()

                //2. Указать альтернативный способ сериализации для определенного типа
                .Printing<int>().Using(p => p.ToString())

                //3. Для числовых типов указать культуру
                .Printing<int>().SetDigitsCulter().InvariantCulture()
                
                //4. Настроить сериализацию конкретного свойства
                .Printing(p => p.Age).Using(age => age.ToString())

                //5. Настроить обрезание строковых свойств (метод должен быть виден только для строковых свойств)
                .Printing<string>().Trim(2)

                //6. Исключить из сериализации конкретного свойства
                .Exclude(p => p.Age);
            var s1 = printer.PrintToString(person);
            Console.WriteLine(s1);
            //7. Синтаксический сахар в виде метода расширения, сериализующего по-умолчанию
            var s2 = person.PrintToString();
            //8. ...с конфигурированием
            var s3 = person.PrintToString(x => x.Printing(p => p.Father.Age)
                .Using(age => age.ToString()));
        }

        [Test]
        public void PrintingConfig_ShouldTrim_Strings()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing<string>().Trim(3);

            var result = printer.PrintToString(person);

            result.Should()
                .Be("Person\r\n\tId = Guid\r\n\tName = Ale\r\n\tSecondName = Iva\r\n\tHeight = 170,4\r\n\tAge = 19\r\n\tFather = null\r\n");
        }
        [Test]
        public void PrintingConfig_ShouldSet_DigitsCulture()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing<double>().SetDigitsCulter().InvariantCulture();

            var result = printer.PrintToString(person);

            result.Should()
                .Be(
                    "Person\r\n\tId = Guid\r\n\tName = Alex\r\n\tSecondName = Ivanov\r\n\tHeight = 170.4\r\n\tAge = 19\r\n\tFather = null\r\n");
        }

        [Test]
        public void PrintingConfig_ShouldExclude_Properties()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude(p => p.Father);

            var result = printer.PrintToString(person);
            
            result.Should()
                .Be(
                    "Person\r\n\tId = Guid\r\n\tName = Alex\r\n\tSecondName = Ivanov\r\n\tHeight = 170,4\r\n\tAge = 19\r\n");
        }


        [Test]
        public void PrintingConfig_ShouldExclude_Types()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude<string>();

            var result = printer.PrintToString(person);

            result.Should()
                .Be("Person\r\n\tId = Guid\r\n\tHeight = 170,4\r\n\tAge = 19\r\n\tFather = null\r\n");
        }

        [Test]
        public void PrintingConfig_ShouldPrint_PropertiesEspecially()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing(x => x.Name).Using(x => "my name is " + x);

            var result = printer.PrintToString(person);

            result.Should()
                .Be(
                    "Person\r\n\tId = Guid\r\n\tName = my name is Alex\r\n\tSecondName = Ivanov\r\n\tHeight = 170,4\r\n\tAge = 19\r\n\tFather = null\r\n");
        }

        [Test]
        public void PrintingConfig_ShouldPrint_TypesEspecially()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing<double>().Using(x => "about "+x);

            var result = printer.PrintToString(person);

            result.Should()
                .Be(
                    "Person\r\n\tId = Guid\r\n\tName = Alex\r\n\tSecondName = Ivanov\r\n\tHeight = about 170,4\r\n\tAge = 19\r\n\tFather = null\r\n");
        }
    }
}