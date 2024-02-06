using Tests.Model;

namespace Tests
{
    public class InnifTest
    {
        private MainDocumentTestModel _model;

        private const string InnErrorText = "- Некорректно заполнен ИНН/VATIN";
        [SetUp]
        public void Setup()
        {
            _model = new MainDocumentTestModel();
        }

        [Test]
        [TestCase("7727563778")]
        public void Validate_WhenCountryIsRussiaAndInnIsValid_ShouldNotReturnError(string inn)
        {
            _model.Inn = inn;
            _model.Country = CountryEnum.Russia;

            var errors = Helper.Validate(_model);
            Assert.IsFalse(errors.Any(e => e.Error == InnErrorText && e.FieldName == "Inn"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("4543534534")]
        public void Validate_WhenCountryIsRussiaAndInnIsNotValid_ShouldReturnError(string inn)
        {
            _model.Inn = inn;
            _model.Country = CountryEnum.Russia;

            var errors = Helper.Validate(_model);
            Assert.IsTrue(errors.Any(e => e.Error == InnErrorText && e.FieldName == "Inn"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("4543534534")]
        public void Validate_WhenCountryIsNotRussia_ShouldNotReturnError(string inn)
        {
            _model.Inn = inn;
            _model.Country = CountryEnum.Germany;

            var errors = Helper.Validate(_model);
            Assert.IsFalse(errors.Any(e => e.Error == InnErrorText && e.FieldName == "Inn"));
        }
    }
}