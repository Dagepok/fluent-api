using System;
using FluentAssertions;
using NUnit.Framework;
using ObjectPrinting.Solved;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class ObjectPrinterAcceptanceTests
    {
        private Person person;

        [SetUp]
        public void SetUp()
        {
            var address = new Address { City = "Yekaterinburg", Street = "Pushkina", House = 1 };
            person = new Person { Name = "Alex", SecondName = "Ivanov", Age = 19, Height = 170.4, Address = address };
        }

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
            //7. Синтаксический сахар в виде метода расширения, сериализующего по-умолчанию
            var s2 = person.PrintToString();
            //8. ...с конфигурированием
            var s3 = person.PrintToString(x => x.Printing(p => p.Address.Street)
                .Using(street => street.ToString() + " street"));
        }

        //6
        [Test]
        public void PrintingConfig_ShouldExclude_Properties()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude(p => p.Address);

            var result = printer.PrintToString(person);

            result.Should()
                .Be(
                    "Person\r\n\tId = 00000000-0000-0000-0000-000000000000\r\n\tName = Alex\r\n\tSecondName = Ivanov\r\n\tHeight = 170,4\r\n\tAge = 19\r\n");
        }

        //1
        [Test]
        public void PrintingConfig_ShouldExclude_Types()
        {
            var printer = ObjectPrinter.For<Person>()
                .Exclude<string>();

            var result = printer.PrintToString(person);
            Console.WriteLine(result);

            result.Should().Be("Person\r\n" +
                                "\tId = 00000000-0000-0000-0000-000000000000\r\n" +
                                "\tHeight = 170,4\r\n" +
                                "\tAge = 19\r\n" +
                                "\tAddress\r\n" +
                                    "\t\tHouse = 1\r\n");
        }

        //4
        [Test]
        public void PrintingConfig_ShouldPrint_PropertiesEspecially()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing(x => x.Name).Using(x => "name is " + x);

            var result = printer.PrintToString(person);

            result.Should().Be("Person\r\n" +
                                   "\tId = 00000000-0000-0000-0000-000000000000\r\n" +
                                   "\tName = name is Alex\r\n" +
                                   "\tSecondName = Ivanov\r\n" +
                                   "\tHeight = 170,4\r\n" +
                                   "\tAge = 19\r\n" +
                                   "\tAddress\r\n" +
                                        "\t\tCity = Yekaterinburg\r\n" +
                                        "\t\tStreet = Pushkina\r\n" +
                                        "\t\tHouse = 1\r\n");
        }

        //2
        [Test]
        public void PrintingConfig_ShouldPrint_TypesEspecially()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing<double>().Using(x => "about " + x);

            var result = printer.PrintToString(person);

            result.Should().Be("Person\r\n" +
                                   "\tId = 00000000-0000-0000-0000-000000000000\r\n" +
                                   "\tName = Alex\r\n" +
                                   "\tSecondName = Ivanov\r\n" +
                                   "\tHeight = about 170,4\r\n" +
                                   "\tAge = 19\r\n" +
                                   "\tAddress\r\n" +
                                       "\t\tCity = Yekaterinburg\r\n" +
                                       "\t\tStreet = Pushkina\r\n" +
                                       "\t\tHouse = 1\r\n");
        }

        //3
        [Test]
        public void PrintingConfig_ShouldSet_DigitsCulture()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing<double>().SetDigitsCulter().InvariantCulture();

            var result = printer.PrintToString(person);

            result.Should().Be("Person\r\n" +
                                   "\tId = 00000000-0000-0000-0000-000000000000\r\n" +
                                   "\tName = Alex\r\n" +
                                   "\tSecondName = Ivanov\r\n" +
                                   "\tHeight = 170.4\r\n" +
                                   "\tAge = 19\r\n" +
                                   "\tAddress\r\n" +
                                       "\t\tCity = Yekaterinburg\r\n" +
                                       "\t\tStreet = Pushkina\r\n" +
                                       "\t\tHouse = 1\r\n");
        }

        //6
        [Test]
        public void PrintingConfig_ShouldTrim_Strings()
        {
            var printer = ObjectPrinter.For<Person>()
                .Printing<string>().Trim(3);

            var result = printer.PrintToString(person);

            result.Should().Be("Person\r\n" +
                                   "\tId = 00000000-0000-0000-0000-000000000000\r\n" +
                                   "\tName = Ale\r\n" +
                                   "\tSecondName = Iva\r\n" +
                                   "\tHeight = 170,4\r\n" +
                                   "\tAge = 19\r\n" +
                                   "\tAddress\r\n" +
                                       "\t\tCity = Yek\r\n" +
                                       "\t\tStreet = Pus\r\n" +
                                       "\t\tHouse = 1\r\n");
        }

        //7
        [Test]
        public void Objects_ShouldBeSerializable_ByDefault()
        {
            var result = person.PrintToString();

            result.Should().Be("Person\r\n" +
                                    "\tId = 00000000-0000-0000-0000-000000000000\r\n" +
                                    "\tName = Alex\r\n" +
                                    "\tSecondName = Ivanov\r\n" +
                                    "\tHeight = 170,4\r\n" +
                                    "\tAge = 19\r\n" +
                                    "\tAddress\r\n" +
                                        "\t\tCity = Yekaterinburg\r\n" +
                                        "\t\tStreet = Pushkina\r\n" +
                                        "\t\tHouse = 1\r\n");
        }

        [Test]
        public void Objects_ShouldBeSerializable_WithConfiguration()
        {

            var result = person.PrintToString(x => x.Exclude(p => p.Address));

            result.Should().Be("Person\r\n" +
                                   "\tId = 00000000-0000-0000-0000-000000000000\r\n" +
                                   "\tName = Alex\r\n" +
                                   "\tSecondName = Ivanov\r\n" +
                                   "\tHeight = 170,4\r\n" +
                                   "\tAge = 19\r\n");
        }

    }
}