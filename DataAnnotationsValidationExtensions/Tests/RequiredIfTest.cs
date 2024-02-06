using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Model;

namespace Tests
{
    public class RequiredIfTest
    {
        private MainDocumentTestModel _model;

        private const string AddressErrorText = "Поле \"Адрес\" обязательно к заполнению для страны Россия";
        [SetUp]
        public void Setup()
        {
            _model = new MainDocumentTestModel();
        }

        [Test]
        [TestCase("Ул. Пушкина д. Колотушкина")]
        [TestCase("Адресс")]
        public void Validate_WhenCountryIsRussiaAndAddressIsValid_ShouldNotReturnError(string address)
        {
            _model.Address = address;
            _model.Country = CountryEnum.Russia;

            var errors = Helper.Validate(_model);
            Assert.IsFalse(errors.Any(e => e.Error == AddressErrorText && e.FieldName == "Address"));

        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Validate_WhenCountryIsRussiaAndAddressIsEmpty_ShouldReturnError(string address)
        {
            _model.Address = address;
            _model.Country = CountryEnum.Russia;

            var errors = Helper.Validate(_model);
            Assert.IsTrue(errors.Any(e => e.Error == AddressErrorText && e.FieldName == "Address"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("ewdwe")]
        [TestCase("ауцауа")]
        public void Validate_WhenCountryIsNotRussia_ShouldNotReturnError(string address)
        {
            _model.Address = address;
            _model.Country = CountryEnum.Germany;

            var errors = Helper.Validate(_model);
            Assert.IsFalse(errors.Any(e => e.Error == AddressErrorText && e.FieldName == "Address"));
        }
    }
}
